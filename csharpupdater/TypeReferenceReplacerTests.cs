using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp.Resolver;
using NUnit.Framework;

namespace csharpupdater
{
	[TestFixture]
	class TypeReferenceReplacerTests : CSharpUpdaterTestsBase
	{
		[Test]
		public void AsFieldType()
		{
			var i = "using UnityEngine; class C { GameObject go; //this is a nice one }";
			var e = "using UnityEngine; class C { Unity.Runtime.Core.SceneObject go; //this is a nice one }";
			Test(e, i);
		}

		[Test]
		public void MultipleReferences()
		{
			var i = "using UnityEngine; class C { GameObject go; GameObject go2; }";
			var e = "using UnityEngine; class C { Unity.Runtime.Core.SceneObject go; Unity.Runtime.Core.SceneObject go2; }";
			Test(e, i);
		}

		[Test]
		public void AsMethodArgument()
		{
			var i = "using UnityEngine; class C { void Kill(GameObject go) {} }";
			var e = "using UnityEngine; class C { void Kill(Unity.Runtime.Core.SceneObject go) {} }";
			Test(e, i);
		}

		[Test]
		public void AsGenericConstraint()
		{
			var i = "class C<T> where T : UnityEngine.GameObject {}";
			var e = "class C<T> where T : Unity.Runtime.Core.SceneObject {}";
			Test(e, i);
		}

		[Test]
		public void AsFieldOfArrayOf()
		{
			var i = "class C { UnityEngine.GameObject[] a; }";
			var e = "class C { Unity.Runtime.Core.SceneObject[] a; }";
			Test(e, i);
		}

		[Test]
		public void AsGenericListArgument()
		{
			var i = "using UnityEngine; class C { List<GameObject> mylist; }";
			var e = "using UnityEngine; class C { List<Unity.Runtime.Core.SceneObject> mylist; }";
			Test(e, i);
		}

		protected override IEnumerable<ReplacingAstVisotor> GetPipeline(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
		{
			yield return new TypeReferenceReplacer(replacementCollector,resolver);
		}
	}
}