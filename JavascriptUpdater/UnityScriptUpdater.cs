using System;
using System.Linq;
using System.Reflection;
using Boo.Lang.Compiler;
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
