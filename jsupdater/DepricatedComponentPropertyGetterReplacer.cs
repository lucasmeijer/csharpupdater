using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using NUnit.Framework;

class DepricatedComponentPropertyGetterReplacer : ReplacingAstVisitor
{
	public DepricatedComponentPropertyGetterReplacer(ReplacementCollector replacementCollector) : base(replacementCollector)
	{
	}

	IEnumerable<Tuple<string, string>> PropertiesToReplace()
	{
		yield return new Tuple<string, string>("rigidbody", "Unity.Runtime.Physics.RigidBody");
		yield return new Tuple<string, string>("collider", "Unity.Runtime.Physics.Collider");
		yield return new Tuple<string, string>("constantForce", "Unity.Runtime.Physics.ConstantForce");
		yield return new Tuple<string, string>("camera", "Unity.Runtime.Rendering.Camera");
		yield return new Tuple<string, string>("animation", "Unity.Runtime.DepricatedAnimation.Animation");
	}

	public override void OnMemberReferenceExpression(MemberReferenceExpression node)
	{
		base.OnMemberReferenceExpression(node);
		var externalProperty = node.Entity as ExternalProperty;
		if (externalProperty == null)
			return;

		if (externalProperty.DeclaringType.FullName != "UnityEngine.Component")
			return;

		var match = PropertiesToReplace().SingleOrDefault(p => p.Item1 == externalProperty.Name);
		if (match == null)
			return;

		var length = node.Name.Length;
	
		_replacementCollector.Add(node.LexicalInfo,length, "GetComponent.<"+match.Item2+">()");
	}
}


class DepricatedComponentPropertyGetterReplacerTests : BooUpdaterTestBase
{
	[Test]
	public void WillReplaceInIdentifierForm()
	{
		var i = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    rigidbody.mass = 10f";
		var e = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    GetComponent.<Unity.Runtime.Physics.RigidBody>().mass = 10f";
		Test(e, i);
	}

	[Test]
	public void WillReplaceWithSelfPrefix()
	{
		var i = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    self.rigidbody.mass = 10f";
		var e = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    self.GetComponent.<Unity.Runtime.Physics.RigidBody>().mass = 10f";
		Test(e, i);
	}


	protected override IEnumerable<DepthFirstVisitor> PipeLineForTest(ReplacementCollector collector)
	{
		yield return new DepricatedComponentPropertyGetterReplacer(collector);
	}
}