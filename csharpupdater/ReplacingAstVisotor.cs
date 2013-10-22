﻿using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

internal class ReplacingAstVisotor : DepthFirstAstVisitor
{
	private CSharpAstResolver _resolver;
	protected ReplacementCollector _replacementCollector;

	public ReplacingAstVisotor(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
	{
		_replacementCollector = replacementCollector;
		_resolver = resolver;
	}

	protected bool IsMemberReferenceTo(AstNode astNode, string expectedMember)
	{
		var member = ResolveMember(astNode);
		if (member == null)
			return false;

		return member.FullName == expectedMember;
	}

	protected bool IsTypeReferenceTo(AstNode astNode, string expectedType)
	{
		var resolveResult = _resolver.Resolve(astNode);
		var typeResolveResult = resolveResult as TypeResolveResult;
		if (typeResolveResult == null)
			return false;
		return typeResolveResult.Type.FullName == expectedType;
	}

	private IMember ResolveMember(AstNode astNode)
	{
		var resolveResult = _resolver.Resolve(astNode);
		var memberResolveResult = resolveResult as MemberResolveResult;
		return memberResolveResult!=null ? memberResolveResult.Member : null;
	}
}