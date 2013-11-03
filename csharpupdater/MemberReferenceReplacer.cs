using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ScriptUpdating;

namespace CSharpUpdater
{
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
			var db = MemberReferenceReplaceKnowledge.Get();

			var match = db.SingleOrDefault(pair => IsMemberReferenceTo(nodeToResolve, pair.Item1));
			if (match == null)
				return;

			_replacementCollector.Add(nodeToReplace,match.Item2);
		}
	}
}