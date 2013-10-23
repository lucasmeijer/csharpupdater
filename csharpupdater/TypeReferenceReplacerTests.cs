using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp.Resolver;
using NUnit.Framework;

namespace csharpupdater
{
	[TestFixture]
	class TypeReferenceReplacerTests : CSharpUpdaterTestsBase
	{
		[Test]
		public void Simple1()
		{
			var input =    "using UnityEngine; class C { GameObject go; //this is a nice one }";
			var expected = "using UnityEngine; class C { Unity.Runtime.Core.SceneObject go; //this is a nice one }";

			Test(expected, input);
		}

		[Test]
		public void MultipleReferences()
		{
			var input    = "using UnityEngine; class C { GameObject go; GameObject go2; }";
			var expected = "using UnityEngine; class C { Unity.Runtime.Core.SceneObject go; Unity.Runtime.Core.SceneObject go2; }";
		
			Test(expected, input);
		}

		[Test]
		public void AsMethodArgument()
		{
			var input = "using UnityEngine; class C { void Kill(GameObject go) {} }";
			var expected ="using UnityEngine; class C { void Kill(Unity.Runtime.Core.SceneObject go) {} }";

			Test(expected, input);
		}

		[Test]
		public void AsGenericConstraint()
		{
			var input    = "class Lucas<T> where T : UnityEngine.GameObject {}";
			var expected = "class Lucas<T> where T : Unity.Runtime.Core.SceneObject {}";

			Test(expected, input);
		}

		[Test]
		public void ArrayOfGameObject()
		{
			var input    = "class Lucas { UnityEngine.GameObject[] a; }";
			var expected = "class Lucas { Unity.Runtime.Core.SceneObject[] a; }";

			Test(expected, input);
		}

		[Test]
		public void AsGenericListArgument()
		{
			var input    = "using UnityEngine; class C { List<GameObject> mylist; }";
			var expected = "using UnityEngine; class C { List<Unity.Runtime.Core.SceneObject> mylist; }";

			Test(expected, input);
		}

		protected override IEnumerable<ReplacingAstVisotor> GetPipeline(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
		{
			yield return new TypeReferenceReplacer(replacementCollector,resolver);
		}
	}
}