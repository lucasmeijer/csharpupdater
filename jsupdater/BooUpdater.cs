﻿using System;
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
	protected BooCompiler _compiler;
	protected const string OldUnityEngineLocation = "C:\\Program Files (x86)\\Unity\\Editor\\Data\\PlaybackEngines\\windowsstandaloneplayer\\Managed\\UnityEngine.dll";

	public string Update(string input)
	{
		return Update(input, null);
	}

	internal string Update(string input, Func<ReplacementCollector, IEnumerable<DepthFirstVisitor>> updatingPipeline)
	{
		_compiler = CreateCompiler();
		SetupCompilerParameters();
		SetupCompilerPipeline();
		_compiler.Parameters.Input.Add(new StringInput("main.js", input));

		var result = _compiler.Run();

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

	protected virtual void SetupCompilerPipeline()
	{
		_compiler.Parameters.Pipeline = new Boo.Lang.Compiler.Pipelines.ResolveExpressions();
	}

	protected virtual void SetupCompilerParameters()
	{
		_compiler.Parameters.GenerateInMemory = true;
		_compiler.Parameters.References.Add(OldUnityEngineAssembly());
	}

	protected Assembly OldUnityEngineAssembly()
	{
		return Assembly.LoadFrom(OldUnityEngineLocation);
	}

	protected virtual BooCompiler CreateCompiler()
	{
		return new BooCompiler();
	}

	private IEnumerable<DepthFirstVisitor> UpdatingPipeline(ReplacementCollector collector)
	{
		yield return new PropertyUpperCaser(collector);
		yield return new DepricatedComponentPropertyGetterReplacer(collector);
		yield return new MemberReferenceReplacer(collector);
		yield return new TypeReferenceReplacer(collector);
	}
}
