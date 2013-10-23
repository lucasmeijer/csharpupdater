using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp.Resolver;
using NUnit.Framework;

namespace csharpupdater
{
	[TestFixture]
	class DepricatedComponentGetterReplacerTests : CSharpUpdaterTestsBase
	{
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

		protected override IEnumerable<ReplacingAstVisotor> GetPipeline(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
		{
			yield return new DepricatedComponentPropertyGetterReplacer(replacementCollector, resolver);
		}
	}
}