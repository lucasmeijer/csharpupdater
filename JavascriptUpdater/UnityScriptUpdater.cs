using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.Ast;
using BooUpdater;
using UnityScript;

namespace JavascriptUpdater
{
	public class JavascriptUpdater : BooUpdater.BooUpdater
	{
		private class MyUnityScriptCompiler : UnityScriptCompiler
		{
			public BooCompiler GetCompiler()
			{
				return _compiler;
			}
		}

		protected override BooCompiler CreateCompiler()
		{
			return new MyUnityScriptCompiler().GetCompiler();
		}

		protected override void SetupCompilerPipeline()
		{
			base.SetupCompilerPipeline();
			_compiler.Parameters.Pipeline = UnityScriptCompiler.Pipelines.AdjustBooPipeline(_compiler.Parameters.Pipeline);
		}

		protected override void SetupCompilerParameters()
		{
			base.SetupCompilerParameters();

			var parameters = (UnityScriptCompilerParameters) _compiler.Parameters;
			parameters.ScriptMainMethod = "MyMain";
			parameters.Imports = new Boo.Lang.List<String>() {"UnityEngine"};

			parameters.ScriptBaseType = FindMonoBehaviour();
		}

		protected override DepthFirstVisitor[] PostProcessPipeline(DepthFirstVisitor[] pipeline, ReplacementCollector collector)
		{
			return Replace(pipeline, typeof (DepricatedComponentPropertyGetterReplacer),new UnityScriptDepricatedComponentPropertyGetterReplacer(collector));
		}

		private static DepthFirstVisitor[] Replace(IEnumerable<DepthFirstVisitor> pipeline, Type type, ReplacingAstVisitor replacement)
		{
			var pipe = pipeline.ToArray();
			for (int i=0; i!= pipe.Length; i++)
				if (type.IsInstanceOfType(pipe[i]))
					pipe[i] = replacement;
			return pipe;
		}

		private System.Type FindMonoBehaviour()
		{
			Assembly[] myassemblies = LoadAssembliesToReference();
			foreach (var assembly in myassemblies)
			{
				var monobehaviour = assembly.GetType("UnityEngine.MonoBehaviour");
				if (monobehaviour != null)
					return monobehaviour;
			}
			throw new Exception("MonoBehaviour not found");
		}
	}
}
