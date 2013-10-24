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
	public PropertyUpperCaser(ReplacementCollector replacementCollector) : base(replacementCollector)
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


[TestFixture]
internal class PropertyUpperCaserTests : BooUpdaterTestBase
{
	[Test]
	public void PropertyAssignmentInMonoBehaviour()
	{
		var i = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    name = name";
		var e = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    Name = Name";
		Test(e,i);
	}

	[Test]
	public void PropertyAssignmentOutsideMonoBehaviour()
	{
		var i = "class C:\n  def Start():\n    mb as UnityEngine.MonoBehaviour\n    print mb.transform.name";
		var e = "class C:\n  def Start():\n    mb as UnityEngine.MonoBehaviour\n    print mb.Transform.Name";
		Test(e, i);
	}

	protected override IEnumerable<DepthFirstVisitor> PipeLineForTest(ReplacementCollector collector)
	{
		yield return new PropertyUpperCaser(collector);
	}
}