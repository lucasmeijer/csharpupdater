using NUnit.Framework;

namespace IntegrationTests.MemberReferenceReplacerTests
{
	[TestFixture]
	class WillReplaceInIdentifierForm : IntegrationTest
	{
		protected override void CSharp(out string i, out string e)
		{
			i = "using UnityEngine; class L : MonoBehaviour { void Start() { Destroy(gameObject); } } ";
			e = "using UnityEngine; class L : MonoBehaviour { void Start() { Destroy(SceneObject); } } ";
		}

		protected override void Boo(out string i, out string e)
		{
			i = "import UnityEngine\nclass L(MonoBehaviour):\n  def S():\n    Destroy(gameObject)";
			e = "import UnityEngine\nclass L(MonoBehaviour):\n  def S():\n    Destroy(SceneObject)";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = "function S() { Destroy(gameObject); }";
			e = "function S() { Destroy(SceneObject); }";
		}
	}

	[TestFixture]
	class WillReplaceInMemberReferenceForm : IntegrationTest
	{
		protected override void CSharp(out string i, out string e)
		{
			i = "class C { void F() { UnityEngine.MonoBehaviour mb = null; mb.gameObject = null; } }";
			e = "class C { void F() { UnityEngine.MonoBehaviour mb = null; mb.SceneObject = null; } }";
		}

		protected override void Boo(out string i, out string e)
		{
			i = "class C:\n  def F():\n    mb as UnityEngine.MonoBehaviour = null\n    mb.gameObject = null";
			e = "class C:\n  def F():\n    mb as UnityEngine.MonoBehaviour = null\n    mb.SceneObject = null";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = "function F(){ var mb:UnityEngine.MonoBehaviour = null; mb.gameObject = null; }";
			e = "function F(){ var mb:UnityEngine.MonoBehaviour = null; mb.SceneObject = null; }";
		}
	}
}
