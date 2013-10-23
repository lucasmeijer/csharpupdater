using System.Collections.Generic;
using csharpupdater;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using NUnit.Framework;

class MemberReferenceReplacer : ReplacingAstVisotor
{
	public MemberReferenceReplacer(ReplacementCollector replacementCollector, CSharpAstResolver resolver) : base(replacementCollector, resolver)
	{
	}

	public override void VisitIdentifierExpression(IdentifierExpression identifierExpression)
	{
		base.VisitIdentifierExpression(identifierExpression);
		ReplaceIfRequired(identifierExpression, identifierExpression);
	}

	public override void VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression)
	{
		base.VisitMemberReferenceExpression(memberReferenceExpression);
		ReplaceIfRequired(memberReferenceExpression, memberReferenceExpression.MemberNameToken);
	}

	private void ReplaceIfRequired(AstNode nodeToResolve, AstNode nodeToReplace)
	{
		if (IsMemberReferenceTo(nodeToResolve, "UnityEngine.Component.gameObject"))
			_replacementCollector.Add(nodeToReplace, "SceneObject");
	}
}

	
class MemberReferenceReplacerTests : CSharpUpdaterTestsBase
{
	[Test]
	public void WillReplaceInIdentifierForm()
	{
		var i = "using UnityEngine; class L : MonoBehaviour { void Start() { Destroy(gameObject); } } ";
		var e = "using UnityEngine; class L : MonoBehaviour { void Start() { Destroy(SceneObject); } } ";
		Test(e,i);
	}

	[Test]
	public void WillReplaceInMemberReferenceForm()
	{
		var i = "class C { void F() { UnityEngine.MonoBehaviour mb = null; mb.gameObject = null; } }";
		var e = "class C { void F() { UnityEngine.MonoBehaviour mb = null; mb.SceneObject = null; } }";
		Test(e, i);
	}

	protected override IEnumerable<ReplacingAstVisotor> GetPipeline(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
	{
		yield return new MemberReferenceReplacer(replacementCollector,resolver);
	}
}
