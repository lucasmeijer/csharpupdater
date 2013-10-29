using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;

namespace CSharpUpdater
{
	internal class DepricatedComponentPropertyGetterReplacer : ReplacingAstVisitor
	{
		public DepricatedComponentPropertyGetterReplacer(ReplacementCollector replacementCollector, CSharpAstResolver resolver) : base(replacementCollector, resolver)
		{
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
			var match = DepricatedComponentPropertyGetterReplacerKnowledge.PropertiesToReplace().SingleOrDefault(p => IsMemberReferenceTo(thingThatShouldResolveToProperty, p.Item1));
			if (match == null)
				return;
		
			_replacementCollector.Add(expressionToReplace, "GetComponent<"+match.Item2+">()");
		}
	}
}