using System;
using Boo.Lang.Compiler;
using UnityScript;

namespace JavascriptUpdater
{
	public class JavascriptUpdater : BooUpdater.BooUpdater
	{
		class MyUnityScriptCompiler : UnityScriptCompiler
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

		override protected void SetupCompilerPipeline()
		{
			base.SetupCompilerPipeline();
			_compiler.Parameters.Pipeline = UnityScriptCompiler.Pipelines.AdjustBooPipeline(_compiler.Parameters.Pipeline);
		}


		protected override void SetupCompilerParameters()
		{
			base.SetupCompilerParameters();

			var parameters = (UnityScriptCompilerParameters) _compiler.Parameters;
			parameters.ScriptMainMethod = "MyMain";
			parameters.Imports = new Boo.Lang.List<String>() { "UnityEngine"};
			
			var monobehaviour = OldUnityEngineAssembly().GetType("UnityEngine.MonoBehaviour");
			if (monobehaviour == null)
				throw new Exception();

			parameters.ScriptBaseType = monobehaviour;
		}
	}
}
