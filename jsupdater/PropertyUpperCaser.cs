using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using NUnit.Framework;

class PropertyUpperCaser : ReplacingAstVisitor
{
	public PropertyUpperCaser(ReplacementCollector replacementCollector, Document document) : base(replacementCollector, document)
	{
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
		if (assemblyName.Name == "UnityEngine")
			_replacementCollector.Add(node.LexicalInfo,node.Name.Length,UpperCaseFirstChar(node.Name));
	}

	private string UpperCaseFirstChar(string name)
	{
		return char.ToUpper(name[0]) + name.Substring(1);
	}
}