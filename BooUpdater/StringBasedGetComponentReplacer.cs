using System.Linq;
using Boo.Lang.Compiler.Ast;

namespace BooUpdater
{
	class StringBasedGetComponentReplacer : ReplacingAstVisitor
	{
		public StringBasedGetComponentReplacer(ReplacementCollector replacementCollector, Document document) : base(replacementCollector, document)
		{
		}

		public override void OnMethodInvocationExpression(MethodInvocationExpression node)
		{
			base.OnMethodInvocationExpression(node);

			var nodeTarget = node.Target as MemberReferenceExpression;
			if (nodeTarget == null)
				return;

			var nodeTargetEntity = nodeTarget.Entity;
			if (nodeTargetEntity == null)
				return;

			if (nodeTargetEntity.FullName != "UnityEngine.Component.GetComponent")
				return;

			if (node.Arguments.Count() != 1)
				return;

			var argument = node.Arguments.Single() as StringLiteralExpression;
			if (argument == null)
				return;



			_replacementCollector.Add(nodeTarget.LexicalInfo,"GetComponent".Length + argument.Value.Length+4 ,"GetComponent("+argument.Value+")");
		}
	}
}
