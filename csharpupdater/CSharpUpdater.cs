using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;

internal class CSharpUpdater
{
	public string Update(string input)
	{
		var parser = new CSharpParser();
		var doc = new ReadOnlyDocument(input);
		var syntaxTree = parser.Parse(doc,"myfile.cs");
		
		IProjectContent project = new CSharpProjectContent();
		var cecilLoader = new CecilLoader { LazyLoad = true };
		var assembly = cecilLoader.LoadAssemblyFile("c:/work/build/WindowsEditor/Data/Managed/UnityEngine.dll");
		project = project.AddAssemblyReferences(assembly);

		project.AddAssemblyReferences(new CecilLoader().LoadAssemblyFile(typeof (object).Assembly.Location));
		var unresolvedFile = syntaxTree.ToTypeSystem();
		project = project.AddOrUpdateFiles(unresolvedFile);

		var resolver = new CSharpAstResolver(project.CreateCompilation(), syntaxTree, unresolvedFile);
		
		var replacementCollector = new ReplacementCollector();

		var gameObjectReferenceCollector = new TypeReferenceReplacer(replacementCollector, resolver);
		syntaxTree.AcceptVisitor(gameObjectReferenceCollector);

		var rigidBodyPropertyGetterCollector = new DepricatedComponentPropertyGetterReplacer(replacementCollector, resolver);
		syntaxTree.AcceptVisitor(rigidBodyPropertyGetterCollector);

		return replacementCollector.ApplyTo(doc);
	}
}