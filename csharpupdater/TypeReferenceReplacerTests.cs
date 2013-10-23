using NUnit.Framework;

namespace csharpupdater
{
	[TestFixture]
	class TypeReferenceReplacerTests : DepricatedComponentGetterReplacerTests
	{
		[Test]
		public void Simple1()
		{
			var input =
				@"using UnityEngine;
class Lucas
{
	GameObject go; //this is a nice one
}";

			var expected =
				@"using UnityEngine;
class Lucas
{
	Unity.Runtime.Core.SceneObject go; //this is a nice one
}";

			Test(expected, input);
		}

		[Test]
		public void MultipleReferences()
		{
			var input =
				@"using UnityEngine;
class Lucas
{
	GameObject go; //this is a nice one
	GameObject go2; //this is a nice one too
}";

			var expected =
				@"using UnityEngine;
class Lucas
{
	Unity.Runtime.Core.SceneObject go; //this is a nice one
	Unity.Runtime.Core.SceneObject go2; //this is a nice one too
}";

			Test(expected, input);
		}

		[Test]
		public void AsMethodArgument()
		{
			var input =
				@"using UnityEngine;
class Lucas
{
	void Kill(GameObject go) {}
}";

			var expected =
				@"using UnityEngine;
class Lucas
{
	void Kill(Unity.Runtime.Core.SceneObject go) {}
}";

			Test(expected, input);
		}

		[Test]
		public void AsGenericConstraint()
		{
			var input = "class Lucas<T> where T : UnityEngine.GameObject {}";
			var expected = "class Lucas<T> where T : Unity.Runtime.Core.SceneObject {}";

			Test(expected, input);
		}

		[Test]
		public void ArrayOfGameObject()
		{
			var input = "class Lucas { UnityEngine.GameObject[] a; }";
			var expected = "class Lucas { Unity.Runtime.Core.SceneObject[] a; }";

			Test(expected, input);
		}

		[Test]
		public void AsGenericListArgument()
		{
			var input =
				@"using UnityEngine;
class Lucas
{
	List<GameObject> mylist;
}";

			var expected =
				@"using UnityEngine;
class Lucas
{
	List<Unity.Runtime.Core.SceneObject> mylist;
}";

			Test(expected, input);
		}

	}
}