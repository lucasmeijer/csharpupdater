using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.TypeSystem;

public class BooUpdater : IScriptUpdater
{
	public class MyUnityScriptCompiler : BooCompiler
	{
		public Boo.Lang.Compiler.CompilerContext MyRun()
		{
			return Run();
		}
	}

	public string Update(string input)
	{
		return Update(input, null);
	}

	internal string Update(string input, Func<ReplacementCollector, IEnumerable<DepthFirstVisitor>> updatingPipeline)
	{
		var compiler = new MyUnityScriptCompiler();
		//compiler.Parameters.Pipeline = UnityScriptCompiler.Pipelines.CompileToBoo();
		compiler.Parameters.Pipeline = new Boo.Lang.Compiler.Pipelines.ResolveExpressions();
		compiler.Parameters.Input.Add(new StringInput("main.js", input));
		compiler.Parameters.GenerateInMemory = true;
		//compiler.Parameters.ScriptMainMethod = "MyMain";

		const string unityengine = "C:\\Program Files (x86)\\Unity\\Editor\\Data\\PlaybackEngines\\windowsstandaloneplayer\\Managed\\UnityEngine.dll";
		var assembly = Assembly.LoadFrom(unityengine);
		var monobehaviour = assembly.GetType("UnityEngine.MonoBehaviour");
		if (monobehaviour == null)
			throw new Exception();

		//compiler.Parameters.Pipeline.BeforeStep += PipelineOnBeforeStep;

		compiler.Parameters.References.Add(assembly);
		//compiler.Parameters.ScriptBaseType = monobehaviour;
		var result = compiler.MyRun();

		
		//if (result.Errors.Any())
		//	throw new Exception("Compile error:"+result.Errors.First());


		if (updatingPipeline == null)
			updatingPipeline = UpdatingPipeline;
		var collector = new ReplacementCollector();
		foreach (DepthFirstVisitor step in updatingPipeline(collector))
		{
			step.Visit(result.CompileUnit);
		}

		return collector.ApplyOn(input);

	}

	private IEnumerable<DepthFirstVisitor> UpdatingPipeline(ReplacementCollector collector)
	{
		yield return new PropertyUpperCaser(collector);
		yield return new DepricatedComponentPropertyGetterReplacer(collector);
		yield return new MemberReferenceReplacer(collector);
		yield return new TypeReferenceReplacer(collector);
	}
}