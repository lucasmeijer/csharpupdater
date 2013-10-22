using ICSharpCode.NRefactory.CSharp;

internal class GameObjectReferenceCollector : DepthFirstAstVisitor
{
	private readonly ReplacementCollector _replacementCollector;

	public GameObjectReferenceCollector(ReplacementCollector replacementCollector)
	{
		_replacementCollector = replacementCollector;
	}

	public override void VisitSimpleType(SimpleType simpleType)
	{
		base.VisitSimpleType(simpleType);
		if (simpleType.Identifier.ToString() == "GameObject")
			Mark(simpleType);
	}

	public override void VisitTypeParameterDeclaration(TypeParameterDeclaration typeParameterDeclaration)
	{
		Mark(typeParameterDeclaration);
	}

	public override void VisitTypeReferenceExpression(TypeReferenceExpression typeReferenceExpression)
	{
		Mark(typeReferenceExpression);
	}

	private void Mark(AstNode astNode)
	{
		_replacementCollector.Add(astNode, "UnityEngine.Core.SceneObject");
	}
	
}