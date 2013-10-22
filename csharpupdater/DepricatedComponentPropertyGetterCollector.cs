using System;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

internal class DepricatedComponentPropertyGetterCollector : DepthFirstAstVisitor
{
	private readonly CSharpAstResolver _resolver;
	private readonly ReplacementCollector _replacementCollector;
	private string _deprecatedProperty = "UnityEngine.Component.rigidbody";
	private string _replacement = "GetComponent<Unity.Runtime.Physics.RigidBody>()";

	public DepricatedComponentPropertyGetterCollector(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
	{
		_replacementCollector = replacementCollector;
		_resolver = resolver;
	}

	public override void VisitIdentifierExpression(IdentifierExpression identifierExpression)
	{
		base.VisitIdentifierExpression(identifierExpression);
		ReplacePropertyReferenceIfRequired(identifierExpression,identifierExpression);
	}

	public override void VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression)
	{
		base.VisitMemberReferenceExpression(memberReferenceExpression);
		ReplacePropertyReferenceIfRequired(memberReferenceExpression,memberReferenceExpression.MemberNameToken);
	}

	private void ReplacePropertyReferenceIfRequired(AstNode thingThatShouldResolveToProperty, AstNode expressionToReplace)
	{
		if (!IsMemberReferenceTo(thingThatShouldResolveToProperty, _deprecatedProperty))
			return;

		_replacementCollector.Add(expressionToReplace, _replacement);
	}

	private bool IsMemberReferenceTo(AstNode astNode, string expectedMember)
	{
		var member = ResolveMember(astNode);
		if (member == null)
			return false;

		return member.FullName == expectedMember;
	}

	private IMember ResolveMember(AstNode astNode)
	{
		var resolveResult = _resolver.Resolve(astNode);
		var memberResolveResult = resolveResult as MemberResolveResult;
		return memberResolveResult!=null ? memberResolveResult.Member : null;
	}
}