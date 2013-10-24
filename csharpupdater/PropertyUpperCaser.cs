using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

internal class PropertyUpperCaser : ReplacingAstVisitor
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