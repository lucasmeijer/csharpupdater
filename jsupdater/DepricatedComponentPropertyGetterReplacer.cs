using System.Linq;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

class DepricatedComponentPropertyGetterReplacer : ReplacingAstVisitor
{
	public DepricatedComponentPropertyGetterReplacer(ReplacementCollector replacementCollector, Document document) : base(replacementCollector, document)
	{
	}

	public override void OnMemberReferenceExpression(MemberReferenceExpression node)
	{
		base.OnMemberReferenceExpression(node);
		var externalProperty = node.Entity as ExternalProperty;
		if (externalProperty == null)
			return;

		var match = DepricatedComponentPropertyGetterReplacerKnowledge.PropertiesToReplace().SingleOrDefault(p => p.Item1 == externalProperty.FullName);
		if (match == null)
			return;

		var length = node.Name.Length;
	
		_replacementCollector.Add(node.LexicalInfo,length, "GetComponent.<"+match.Item2+">()");
	}
}