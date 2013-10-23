using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

internal class CSharpUpdater
{
	public string Update(string input, Func<ReplacementCollector, CSharpAstResolver,IEnumerable<ReplacingAstVisotor>> pipeLineProvider)
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

	private IEnumerable<ReplacingAstVisotor> BuildPipeline(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
	{
		yield return new TypeReferenceReplacer(replacementCollector, resolver);
		yield return new DepricatedComponentPropertyGetterReplacer(replacementCollector, resolver);
		yield return new PropertyUpperCaser(replacementCollector, resolver);
	}
}