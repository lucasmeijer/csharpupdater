using System.Linq;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using ScriptUpdating;

namespace BooUpdater
{
	class PropertyUpperCaser : ReplacingAstVisitor
	{
		private readonly bool _onlyTransform;

		public PropertyUpperCaser(ReplacementCollector replacementCollector, bool onlyTransform=false) : base(replacementCollector)
		{
			_onlyTransform = onlyTransform;
		}

		public override void OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			base.OnMemberReferenceExpression(node);

			var externalProperty = node.Entity as ExternalProperty;
			if (externalProperty == null)
				return;
			var propertyInfo = externalProperty.PropertyInfo;
			var declaringType = propertyInfo.DeclaringType;
			var assembly = declaringType.Assembly;
			var assemblyName = assembly.GetName();
			if (_onlyTransform)
			{
				if (externalProperty.FullName != "UnityEngine.Component.transform" &&
				    externalProperty.FullName != "UnityEngine.GameObject.transform")
					return;
			}
		

			if (DepricatedComponentPropertyGetterReplacerKnowledge.PropertiesToReplace().Any(p => p.Item1 == externalProperty.FullName))
				return;

			if (assemblyName.Name == "UnityEngine")
				_replacementCollector.Add(node.LexicalInfo,node.Name.Length,UpperCaseFirstChar(node.Name));
		}

		private string UpperCaseFirstChar(string name)
		{
			return char.ToUpper(name[0]) + name.Substring(1);
		}
	}
}