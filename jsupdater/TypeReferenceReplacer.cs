﻿using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;
using NUnit.Framework;

class TypeReferenceReplacer : ReplacingAstVisitor
{
	public TypeReferenceReplacer(ReplacementCollector replacementCollector) : base(replacementCollector)
	{
	}

	public override void OnSimpleTypeReference(SimpleTypeReference node)
	{
		base.OnSimpleTypeReference(node);
		if (node.Entity.FullName != "UnityEngine.GameObject")
			return;
		_replacementCollector.Add(node.LexicalInfo, node.OriginalName.Length, "Unity.Runtime.Core.SceneObject");
	}
}


[TestFixture]
internal class TypeReferenceReplacerTests : BooUpdaterTestBase
{
	[Test]
	public void AsFieldType_FullyQualified()
	{
		var i = "class C:\n  go as UnityEngine.GameObject";
		var e = "class C:\n  go as Unity.Runtime.Core.SceneObject";
		Test(e, i);
	}

	[Test]
	public void AsFieldType_NotFullyQualified()
	{
		var i = "import UnityEngine\nclass C:\n  go as GameObject";
		var e = "import UnityEngine\nclass C:\n  go as Unity.Runtime.Core.SceneObject";
		Test(e, i);
	}

	[Test]
	public void AsFieldType_NotFullyQualified_Array()
	{
		var i = "import UnityEngine\nclass C:\n  go as (GameObject)";
		var e = "import UnityEngine\nclass C:\n  go as (Unity.Runtime.Core.SceneObject)";
		Test(e, i);
	}
	
	protected override IEnumerable<DepthFirstVisitor> PipeLineForTest(ReplacementCollector collector)
	{
		yield return new TypeReferenceReplacer(collector);
	}
}