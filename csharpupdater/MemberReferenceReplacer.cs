using System.Collections.Generic;
using csharpupdater;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using NUnit.Framework;

class MemberReferenceReplacer : ReplacingAstVisitor
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