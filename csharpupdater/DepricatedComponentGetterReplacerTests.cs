using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp.Resolver;
using NUnit.Framework;

namespace csharpupdater
{
	[TestFixture]
	class DepricatedComponentGetterReplacerTests : CSharpUpdaterTestsBase
	{
		[Test]
		public void WillReplaceInIdentifierForm()
		{
			var i = "class C : UnityEngine.MonoBehaviour { void Start() { rigidbody.mass = 10f; } }";
			var e = "class C : UnityEngine.MonoBehaviour { void Start() { GetComponent<Unity.Runtime.Physics.RigidBody>().mass = 10f; } }";
			Test(e,i);
		}

		[Test]
		public void WillReplacePropertyInMemberReferenceForm()
		{
			var i = "using UnityEngine; class C { void Start() { MonoBehaviour b; float a = b.rigidbody.mass; } }";
			var e ="using UnityEngine; class C { void Start() { MonoBehaviour b; float a = b.GetComponent<Unity.Runtime.Physics.RigidBody>().mass; } }";
		
			Test(e,i);
		}

		[Test]
		public void WillNotModifySomethingCalledMonoBehaviourButThatIsNotOurMonoBehaviour()
		{
			//notice MonoBehaviour is unresolvable to UnityEngine.MonoBehaviour, because there's no using statement.
			AssertIsNotModified("class C : MonoBehaviour { void Start()	{ rigidbody.mass = 10f;	} }");
		}

		[Test]
		public void WillNotModifyLocalVariableCalledRigidBody()
		{
			AssertIsNotModified("class C : UnityEngine.MonoBehaviour { void Start() { int rigidbody = 0; rigidbody = 1; } }");
		}

		[Test]
		public void WillNotModifyLocalVariableCalledRigidBodyOfTypeRigidbody()
		{
			AssertIsNotModified("using UnityEngine; class C : MonoBehaviour { void Start() { Rigidbody rigidbody = null; rigidbody.mass = 10f; } }");
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


		protected override IEnumerable<ReplacingAstVisitor> PipelineForTest(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
		{
			yield return new DepricatedComponentPropertyGetterReplacer(replacementCollector, resolver);
		}
	}
}