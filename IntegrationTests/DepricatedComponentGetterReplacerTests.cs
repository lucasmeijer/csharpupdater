using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IntegrationTests.DepricatedComponentGetterReplacerTests
{
	class WillReplaceInIdentifierForm : IntegrationTest
	{
		protected override void CSharp(out string i, out string e)
		{
			i = "class C : UnityEngine.MonoBehaviour { void Start() { rigidbody.mass = 10f; } }";
			e = "class C : UnityEngine.MonoBehaviour { void Start() { GetComponent<Unity.Runtime.Physics.RigidBody>().Mass = 10f; } }";
		}

		protected override void Boo(out string i, out string e)
		{
			i = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    rigidbody.mass = 10f";
			e = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    GetComponent.<Unity.Runtime.Physics.RigidBody>().Mass = 10f";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = "function S() { rigidbody.mass = 10f; }";
			e = "function S() { GetComponent.<Unity.Runtime.Physics.RigidBody>().Mass = 10f; }";
		}
	}

	class WillReplaceWithSelfPrefix : IntegrationTest
	{
		protected override void CSharp(out string i, out string e)
		{
			i = "class C : UnityEngine.MonoBehaviour { void Start() { this.rigidbody.mass = 10f; } }";
			e = "class C : UnityEngine.MonoBehaviour { void Start() { this.GetComponent<Unity.Runtime.Physics.RigidBody>().Mass = 10f; } }";
		}

		protected override void Boo(out string i, out string e)
		{
			i = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    self.rigidbody.mass = 10f";
			e = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    self.GetComponent.<Unity.Runtime.Physics.RigidBody>().Mass = 10f";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = "function S() { this.rigidbody.mass = 10f; }";
			e = "function S() { this.GetComponent.<Unity.Runtime.Physics.RigidBody>().Mass = 10f; }";
		}
	}

	class WillNotModifySomethingCalledMonoBehaviourButThatIsNotOurMonoBehaviour : IntegrationTest
	{
		//note there is no using UnityEngine;
		protected override void CSharp(out string i, out string e)
		{
			i = e = "class C : MonoBehaviour { void Start()	{ rigidbody.mass = 10f;	} }";
		}

		protected override void Boo(out string i, out string e)
		{
			i = e = "class C(MonoBehaviour):\n  def S():\n    rigidbody.mass = 10f";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = e = "class C : MonoBehaviour { function S() { rigidbody.mass = 10f; } }";
		}
	}

	internal class WillNotModifyLocalVariableCalledRigidBody : IntegrationTest
	{
		protected override void CSharp(out string i, out string e)
		{
			i = e = "class C : UnityEngine.MonoBehaviour { void Start() { int rigidbody = 0; rigidbody = 1; } }";
		}

		protected override void Boo(out string i, out string e)
		{
			i = e = "class C(UnityEngine.MonoBehaviour):\n  def S():\n    rigidbody as int = 0\n    rigidbody = 1; } }";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = e = "function S() { var rigidbody:int = 0; rigidbody = 1; } }";
		}
	}

}
