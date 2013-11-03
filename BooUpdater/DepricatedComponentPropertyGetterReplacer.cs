using System.Linq;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using ScriptUpdating;

namespace BooUpdater
{
	public class DepricatedComponentPropertyGetterReplacer : ReplacingAstVisitor
	{
		public DepricatedComponentPropertyGetterReplacer(ReplacementCollector replacementCollector) : base(replacementCollector)
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
			
			_replacementCollector.Add(node.LexicalInfo,length, ReplacementStringFor(match.Item2));
		}

		protected virtual string ReplacementStringFor(string type)
		{
			return "GetComponent[of "+type+"]()";
		}
	}
}