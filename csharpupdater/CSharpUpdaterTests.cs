using NUnit.Framework;

namespace csharpupdater
{
	[TestFixture]
	class CSharpUpdaterTests
	{
		[Test]
		public void Simple1()
		{
			var input = 
				@"class Lucas
{
	GameObject go; //this is a nice one
}";

			var expected =
				@"class Lucas
{
	UnityEngine.Core.SceneObject go; //this is a nice one
}";

			Test(expected, input);
		}

		[Test]
		public void MultipleReferences()
		{
			var input = 
				@"class Lucas
{
	GameObject go; //this is a nice one
	GameObject go2; //this is a nice one too
}";

			var expected =
				@"class Lucas
{
	UnityEngine.Core.SceneObject go; //this is a nice one
	UnityEngine.Core.SceneObject go2; //this is a nice one too
}";

			Test(expected, input);
		}

		[Test]
		public void AsMethodArgument()
		{
			var input =
				@"class Lucas
{
	void Kill(GameObject go) {}
}";

			var expected =
				@"class Lucas
{
	void Kill(UnityEngine.Core.SceneObject go) {}
}";

			Test(expected, input);
		}

		[Test]
		public void AsGenericListArgument()
		{
			var input =
				@"class Lucas
{
	List<GameObject> mylist;
}";

			var expected =
				@"class Lucas
{
	List<UnityEngine.Core.SceneObject> mylist;
}";

			Test(expected, input);
		}

		[Test]
		public void GetRigidBodyToGetComponent()
		{
			var input =
@"class Lucas : UnityEngine.MonoBehaviour
{
	void Start()
	{
		rigidbody.mass = 10f;
	}
}";

			var expected =
@"class Lucas : UnityEngine.MonoBehaviour
{
	void Start()
	{
		GetComponent<Unity.Runtime.Physics.RigidBody>().mass = 10f;
	}
}";

			Test(expected, input);
		}

		[Test]
		public void LooksLikeRigidBodyGetterButIsnt2()
		{
			//notice MonoBehaviour is unresolvable to UnityEngine.MonoBehaviour, because there's no using statement.
			var input =
@"class Lucas : MonoBehaviour
{
	void Start()
	{
		rigidbody.mass = 10f;
	}
}";
			AssertIsNotModified(input);
		}

		[Test]
		public void LooksLikeRigidBodyGetterButIsnt1()
		{
			var input =
@"class Lucas : MonoBehaviour
{
	void Start()
	{
		Rigidbody rigidBody = null;
		rigidbody.mass = 10f;
	}
}";
			AssertIsNotModified(input);
		}

		[Test]
		[Ignore("NRefactory incorrectly resolves MonoBehaviour to UnityEngine.MonoBehaviour, as the resolver apparently cannot deal with the using alias")]
		public void LooksLikeRigidBodyGetterButIsnt3()
		{
			var input =
@"
using UnityEngine;
using MonoBehavour=SomethingElse; //note how we're redirecting this type, so rigidBody should not resolve.
class Lucas : MonoBehaviour
{
	void Start()
	{
		rigidbody.mass = 10f;
	}
}";
			AssertIsNotModified(input);
		}


		[Test]
		public void rigidBodyFromOutside()
		{
			var input =
@"
using UnityEngine;
class Lucas
{
	void Start()
	{
		MonoBehaviour b;
		float a = b.rigidbody.mass;
	}
}";
			var expect =
@"
using UnityEngine;
class Lucas
{
	void Start()
	{
		MonoBehaviour b;
		float a = b.GetComponent<Unity.Runtime.Physics.RigidBody>().mass;
	}
}";

			Test(expect,input);
		}


		private static void AssertIsNotModified(string input)
		{
			Test(input, input);
		}


		private static void Test(string expected, string input)
		{
			var updater = new CSharpUpdater();
			var output = updater.Update(input);
			Assert.AreEqual(
				expected, output);
		}
	}
}