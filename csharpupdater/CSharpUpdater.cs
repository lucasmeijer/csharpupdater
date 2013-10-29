using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;
using ScriptUpdating;

namespace CSharpUpdater
{
	public class CSharpUpdater : IScriptUpdater
	{
		public string Update(string input)
		{
			return Update(input, null);
		}

		public string UpdateSmall(string input)
		{
			return Update(input, SmallPipeline);
		}

		internal string Update(string input, Func<ReplacementCollector, CSharpAstResolver,IEnumerable<ReplacingAstVisitor>> pipeLineProvider)
		{
			var parser = new CSharpParser();
			var doc = new ReadOnlyDocument(input);
			var syntaxTree = parser.Parse(doc,"myfile.cs");
		
			IProjectContent project = new CSharpProjectContent();
			var cecilLoader = new CecilLoader { LazyLoad = true };
			var assembly = cecilLoader.LoadAssemblyFile("C:/Program Files (x86)/Unity/Editor/Data/PlaybackEngines/windowsstandaloneplayer/Managed/UnityEngine.dll");
			project = project.AddAssemblyReferences(assembly);

			project.AddAssemblyReferences(new CecilLoader().LoadAssemblyFile(typeof (object).Assembly.Location));
			var unresolvedFile = syntaxTree.ToTypeSystem();
			project = project.AddOrUpdateFiles(unresolvedFile);

			var resolver = new CSharpAstResolver(project.CreateCompilation(), syntaxTree, unresolvedFile);
			var replacementCollector = new ReplacementCollector();

			if (pipeLineProvider == null)
				pipeLineProvider = BuildPipeline;

			foreach (var visitor in pipeLineProvider(replacementCollector, resolver))
				syntaxTree.AcceptVisitor(visitor);

			return replacementCollector.ApplyTo(doc);
		}

		private IEnumerable<ReplacingAstVisitor> BuildPipeline(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
		{
			yield return new MemberReferenceReplacer(replacementCollector, resolver);
			yield return new TypeReferenceReplacer(replacementCollector, resolver);
			yield return new DepricatedComponentPropertyGetterReplacer(replacementCollector, resolver);
			yield return new PropertyUpperCaser(replacementCollector, resolver);
			yield return new StringBasedGetComponentReplacer(replacementCollector,resolver);
		}

		private IEnumerable<ReplacingAstVisitor> SmallPipeline(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
		{
			yield return new PropertyUpperCaser(replacementCollector, resolver, true);
		}
	}
}