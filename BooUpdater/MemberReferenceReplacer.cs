using Boo.Lang.Compiler.Ast;

namespace BooUpdater
{
	class MemberReferenceReplacer : ReplacingAstVisitor
	{
		public MemberReferenceReplacer(ReplacementCollector replacementCollector, Document document)
			: base(replacementCollector, document)
		{
		}

		public override void OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			base.EnterMemberReferenceExpression(node);
			if (node.Entity == null)
				return;

			if (node.Entity.FullName != "UnityEngine.Component.gameObject")
				return;

			_replacementCollector.Add(node.LexicalInfo,node.Name.Length,"SceneObject");
		}
	}
}