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
			e = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    GetComponent[of Unity.Runtime.Physics.RigidBody]().Mass = 10f";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = "function S() { rigidbody.mass = 10f; }";
			e = "function S() { GetComponent(Unity.Runtime.Physics.RigidBody).Mass = 10f; }";
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
			e = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    self.GetComponent[of Unity.Runtime.Physics.RigidBody]().Mass = 10f";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = "function S() { this.rigidbody.mass = 10f; }";
			e = "function S() { this.GetComponent(Unity.Runtime.Physics.RigidBody).Mass = 10f; }";
		}
	}

	class CanResolveThroughGenericList : IntegrationTest
	{
		protected override void CSharp(out string i, out string e)
		{
			i = "using UnityEngine; using System.Collections.Generic; class C { void S() { var meshFilters = new List<MeshFilter>(); foreach(var meshFilter in meshFilters) { var a = meshFilter.renderer.sharedMaterials; } } }";
			e = "using UnityEngine; using System.Collections.Generic; class C { void S() { var meshFilters = new List<MeshFilter>(); foreach(var meshFilter in meshFilters) { var a = meshFilter.GetComponent<Renderer>().SharedMaterials; } } }";
		}

		protected override void Boo(out string i, out string e)
		{
			i = "class C:\n def S():\n  a as System.Collections.Generic.List[of UnityEngine.MeshFilter] = null\n  b = a[0].renderer";
			e = "class C:\n def S():\n  a as System.Collections.Generic.List[of UnityEngine.MeshFilter] = null\n  b = a[0].GetComponent[of Renderer]()";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = "function S() { var a:System.Collections.Generic.List.<MeshFilter> = null; for (var b in a) { var c = b.renderer; } }";
			e = "function S() { var a:System.Collections.Generic.List.<MeshFilter> = null; for (var b in a) { var c = b.GetComponent(Renderer); } }";
		}
	}

	class BinaryOperatorThatCanExpandWorks : IntegrationTest
	{
		protected override void CSharp(out string i, out string e)
		{
			i = "class C : UnityEngine.MonoBehaviour { void S() { camera.depthTextureMode |= DepthTextureMode.DepthNormals; } }";
			e = "class C : UnityEngine.MonoBehaviour { void S() { GetComponent<Camera>().DepthTextureMode |= DepthTextureMode.DepthNormals; } }";
		}

		protected override void Boo(out string i, out string e)
		{
			i = "class C(UnityEngine.MonoBehaviour):\n def S():\n  camera.depthTextureMode |= DepthTextureMode.DepthNormals\n";
			e = "class C(UnityEngine.MonoBehaviour):\n def S():\n  GetComponent[of Camera]().DepthTextureMode |= DepthTextureMode.DepthNormals\n";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = "camera.depthTextureMode |= DepthTextureMode.DepthNormals;";
			e = "GetComponent(Camera).DepthTextureMode |= DepthTextureMode.DepthNormals;";
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
