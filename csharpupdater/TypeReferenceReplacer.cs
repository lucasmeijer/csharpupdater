using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;

internal class TypeReferenceReplacer : ReplacingAstVisitor
{
	private static readonly string _oldFullTypeName = "UnityEngine.GameObject";
	private static readonly string _newFullTypeName = "Unity.Runtime.Core.SceneObject";

	public TypeReferenceReplacer(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
		: base(replacementCollector, resolver)
	{
	}

	public override void VisitSimpleType(SimpleType simpleType)
	{
		base.VisitSimpleType(simpleType);
		ReplaceIfTypeReferencesMatches(simpleType);
	}

	public override void VisitTypeParameterDeclaration(TypeParameterDeclaration typeParameterDeclaration)
	{
		base.VisitTypeParameterDeclaration(typeParameterDeclaration);		
		ReplaceIfTypeReferencesMatches(typeParameterDeclaration);
	}

	public override void VisitMemberType(MemberType memberType)
	{
		base.VisitMemberType(memberType);
		ReplaceIfTypeReferencesMatches(memberType);
	}


	private void ReplaceIfTypeReferencesMatches(AstNode simpleType)
	{
		if (!IsTypeReferenceTo(simpleType, _oldFullTypeName)) 
			return;

		_replacementCollector.Add(simpleType, _newFullTypeName);
	}
}