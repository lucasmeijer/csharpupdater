using System.Linq;
using Boo.Lang.Compiler.Ast;
using ScriptUpdating;

namespace BooUpdater
{
	class MemberReferenceReplacer : ReplacingAstVisitor
	{
		public MemberReferenceReplacer(ReplacementCollector replacementCollector)
			: base(replacementCollector)
		{
		}

		public override void OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			base.EnterMemberReferenceExpression(node);
			if (node.Entity == null)
				return;

			var match = MemberReferenceReplaceKnowledge.Get().SingleOrDefault(pair => node.Entity.FullName == pair.Item1);
			if (match == null)
				return;

			_replacementCollector.Add(node.LexicalInfo,node.Name.Length,match.Item2);
		}
	}
}