using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;
using NUnit.Framework;

class MemberReferenceReplacer : ReplacingAstVisitor
{
	public MemberReferenceReplacer(ReplacementCollector replacementCollector)
		: base(replacementCollector)
	{
	}

	public override void OnMemberReferenceExpression(MemberReferenceExpression node)
	{
		base.EnterMemberReferenceExpression(node);
		if (node.Entity.FullName != "UnityEngine.Component.gameObject")
			return;

		_replacementCollector.Add(node.LexicalInfo,node.Name.Length,"SceneObject");
	}
}


class MemberReferenceReplacerTests : BooUpdaterTestBase
{
	[Test]
	public void WillReplaceInIdentifierForm()
	{
		var i = "import UnityEngine\nclass L(MonoBehaviour):\n  def Start():\n    Destroy(gameObject)";
		var e = "import UnityEngine\nclass L(MonoBehaviour):\n  def Start():\n    Destroy(SceneObject)";
		Test(e, i);
	}
	
	[Test]
	public void WillReplaceInMemberReferenceForm()
	{
		var i = "import UnityEngine\nclass L:\n  def Start():\n    mb as UnityEngine.MonoBehaviour = null\n    a = mb.gameObject";
		var e = "import UnityEngine\nclass L:\n  def Start():\n    mb as UnityEngine.MonoBehaviour = null\n    a = mb.SceneObject";
		Test(e, i);
	}

	protected override IEnumerable<DepthFirstVisitor> PipeLineForTest(ReplacementCollector collector)
	{
		yield return new MemberReferenceReplacer(collector);
	}
}
