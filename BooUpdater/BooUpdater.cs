using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.IO;
using ScriptUpdating;

namespace BooUpdater
{
	public class BooUpdater : IScriptUpdater
	{
		protected BooCompiler _compiler;
		protected const string OldUnityEngineLocation = "C:\\Program Files (x86)\\Unity\\Editor\\Data\\PlaybackEngines\\windowsstandaloneplayer\\Managed\\UnityEngine.dll";

		public IEnumerable<SourceFile> Update(SourceFile[] input)
		{
			return Update(input, null);
		}

		public IEnumerable<SourceFile> UpdateSmall(SourceFile[] input)
		{
			return Update(input,SmallPipeline);
		}

		internal SourceFile[] Update(SourceFile[] inputs, Func<ReplacementCollector, IEnumerable<DepthFirstVisitor>> updatingPipeline)
		{
			_compiler = CreateCompiler();
			SetupCompilerParameters();
			SetupCompilerPipeline();
			foreach(var input in inputs)
				_compiler.Parameters.Input.Add(new StringInput(input.FileName, input.Contents));

			var result = _compiler.Run();

			//if (result.Errors.Any())
			//	throw new Exception("Compile error:"+result.Errors.First());

			if (updatingPipeline == null)
				updatingPipeline = UpdatingPipeline;

			var collector = new ReplacementCollector();
			var pipeline = updatingPipeline(collector).ToArray();
			foreach (DepthFirstVisitor step in pipeline)
			{
				step.Visit(result.CompileUnit);
			}
			collector.ApplyOn(inputs);

			return inputs;
		}

		protected virtual void SetupCompilerPipeline()
		{
			_compiler.Parameters.Pipeline = new Boo.Lang.Compiler.Pipelines.ResolveExpressions();
			_compiler.Parameters.Pipeline.BreakOnErrors = false;
		}

		protected virtual void SetupCompilerParameters()
		{
			_compiler.Parameters.GenerateInMemory = true;

			foreach(var define in PreprocessorDefines.Get())
				_compiler.Parameters.Defines.Add(define, "1");

			foreach (var assembly in LoadAssembliesToReference())
				_compiler.Parameters.References.Add(assembly);
		}

		protected Assembly[] LoadAssembliesToReference()
		{
			return AssembliesToReference.Get().Select(Assembly.LoadFrom).ToArray();
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
			yield return new StringBasedGetComponentReplacer(collector);
		}

		private IEnumerable<DepthFirstVisitor> SmallPipeline(ReplacementCollector collector)
		{
			yield return new DepricatedComponentPropertyGetterReplacer(collector);
		}
	}
}
