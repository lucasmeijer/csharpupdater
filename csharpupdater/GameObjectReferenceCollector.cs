using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;

internal class GameObjectReferenceCollector : ReplacingAstVisotor
{
	public GameObjectReferenceCollector(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
		: base(replacementCollector, resolver)
	{
	}

	public override void VisitSimpleType(SimpleType simpleType)
	{
		base.VisitSimpleType(simpleType);
		if (IsTypeReferenceTo(simpleType, "UnityEngine.GameObject"))
			Replace(simpleType);
	}

	public override void VisitTypeParameterDeclaration(TypeParameterDeclaration typeParameterDeclaration)
	{
		if (IsTypeReferenceTo(typeParameterDeclaration,"UnityEngine.GameObject"))
			Replace(typeParameterDeclaration);
	}

	public override void VisitMemberType(MemberType memberType)
	{
		base.VisitMemberType(memberType);
		if (IsTypeReferenceTo(memberType, "UnityEngine.GameObject"))
			Replace(memberType);
	}


	public override void VisitTypeReferenceExpression(TypeReferenceExpression typeReferenceExpression)
	{
		Replace(typeReferenceExpression);
	}

	private void Replace(AstNode astNode)
	{
		_replacementCollector.Add(astNode, "Unity.Runtime.Core.SceneObject");
	}
	
}