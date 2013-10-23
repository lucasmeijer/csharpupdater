using System;
using System.Collections.Generic;
using csharpupdater;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using NUnit.Framework;

internal class CSharpUpdater
{
	public string Update(string input, Func<ReplacementCollector, CSharpAstResolver,IEnumerable<ReplacingAstVisotor>> pipeLineProvider)
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

internal class PropertyUpperCaser : ReplacingAstVisotor
{
	public PropertyUpperCaser(ReplacementCollector replacementCollector, CSharpAstResolver resolver) : base(replacementCollector,resolver)
	{
	}

	public override void VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression)
	{
		base.VisitMemberReferenceExpression(memberReferenceExpression);
		ReplaceReferenceWIthUppercaseIfRequired(memberReferenceExpression, memberReferenceExpression.MemberNameToken);
	}

	public override void VisitIdentifierExpression(IdentifierExpression identifierExpression)
	{
		base.VisitIdentifierExpression(identifierExpression);
		ReplaceReferenceWIthUppercaseIfRequired(identifierExpression, identifierExpression);
	}

	private void ReplaceReferenceWIthUppercaseIfRequired(AstNode astNodeToResolve, AstNode astNodeToReplace)
	{
		var property = base.ResolveMember(astNodeToResolve) as DefaultResolvedProperty;
		if (property == null)
			return;

		if (property.ParentAssembly.AssemblyName != "UnityEngine")
			return;

		if (char.IsLower(property.Name[0]))
			_replacementCollector.Add(astNodeToReplace, UpperCaseFirstChar(property.Name));
	}

	private string UpperCaseFirstChar(string name)
	{
		return char.ToUpper(name[0]) + name.Substring(1);
	}
}

[TestFixture]
class PropertyUpperCaserTests : CSharpUpdaterTestsBase
{
	[Test]
	public void CanUpdatePropertyUsedAsIdentifier()
	{
		var input    = "class C : UnityEngine.MonoBehaviour { void Start() { Debug.Log(name); } }";
		var expected = "class C : UnityEngine.MonoBehaviour { void Start() { Debug.Log(Name); } }";
		Test(expected,input);
	}

	[Test]
	public void CanUpdatePropertyUsedAsMemberReference()
	{
		var input    = "class C { void Start() { UnityEngine.GameObject go = null; string a = go.tag; } }";
		var expected = "class C { void Start() { UnityEngine.GameObject go = null; string a = go.Tag; } }";
		Test(expected, input);
	}

	protected override IEnumerable<ReplacingAstVisotor> GetPipeline(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
	{
		yield return new PropertyUpperCaser(replacementCollector,resolver);
	}
}