using NUnit.Framework;

namespace IntegrationTests.PropertyUpperCaserTests
{
	[TestFixture]
    public class InsideOfMonoBehaviour : IntegrationTest
    {
		protected override void CSharp(out string i, out string e)
		{
			i = "class C : UnityEngine.MonoBehaviour { void Start() { Debug.Log(name); } }";
			e = "class C : UnityEngine.MonoBehaviour { void Start() { Debug.Log(Name); } }";
		}

		protected override void Boo(out string i, out string e)
		{
			i = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    name = name";
			e = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    Name = Name";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = "function S() { name = name; }";
			e = "function S() { Name = Name; }";
		}
    }

	[TestFixture]
	public class OutsideOfMonoBehaviour : IntegrationTest
	{
		protected override void CSharp(out string i, out string e)
		{
			i = "class C { void S() { UnityEngine.MonoBehaviour mb = null; Debug.Log(mb.tag); } }";
			e = "class C { void S() { UnityEngine.MonoBehaviour mb = null; Debug.Log(mb.Tag); } }";
		}

		protected override void Boo(out string i, out string e)
		{
			i = "class C:\n  def S():\n    mb as UnityEngine.MonoBehaviour\n    Debug.Log(mb.tag)";
			e = "class C:\n  def S():\n    mb as UnityEngine.MonoBehaviour\n    Debug.Log(mb.Tag)";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = "function S() { var mb:UnityEngine.MonoBehaviour; Debug.Log(mb.tag); }";
			e = "function S() { var mb:UnityEngine.MonoBehaviour; Debug.Log(mb.Tag); }";
		}
	}
}
