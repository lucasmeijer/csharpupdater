using System;
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

	[TestFixture]
	public class InMemberReferenceExpression : IntegrationTest
	{
		protected override void CSharp(out string i, out string e)
		{
			i = "class C : UnityEngine.MonoBehaviour { void Start() { string a = transform.name; } }";
			e = "class C : UnityEngine.MonoBehaviour { void Start() { string a = Transform.Name; } }";
		}

		protected override void Boo(out string i, out string e)
		{
			i = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    a as string = self.transform.name";
			e = "class C(UnityEngine.MonoBehaviour):\n  def Start():\n    a as string = self.Transform.Name";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = "function S() { var a = this.transform.name; }";
			e = "function S() { var a = this.Transform.Name; }";
		}
	}

	[TestFixture]
	public class OnMethodInvocationResult: IntegrationTest
	{
		protected override void CSharp(out string i, out string e)
		{
			i = "using UnityEngine; class C { void Start() { string a = GameObject.Find(\"a\").transform.name; } }";
			e = "using UnityEngine; class C { void Start() { string a = GameObject.Find(\"a\").Transform.Name; } }";
		}

		protected override void Boo(out string i, out string e)
		{
			i = "class C:\n  def S():\n    q as string = UnityEngine.GameObject.Find(\"a\").transform.name";
			e = "class C:\n  def S():\n    q as string = UnityEngine.GameObject.Find(\"a\").Transform.Name";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = "var a = GameObject.Find(\"a\").transform.name;";
			e = "var a = GameObject.Find(\"a\").Transform.Name;";
		}
	}

	[TestFixture]
	public class OnMethodInvocationResultAfterVariableOfUnknownType : IntegrationTest
	{
		protected override void CSharp(out string i, out string e)
		{
			i = "using UnityEngine; class C { void Start() { NoIdeaWhatThisIs b; string a = GameObject.Find(\"a\").transform.name; } }";
			e = "using UnityEngine; class C { void Start() { NoIdeaWhatThisIs b; string a = GameObject.Find(\"a\").Transform.Name; } }";
		}

		protected override void Boo(out string i, out string e)
		{
			i = "class C:\n  def S():\n    bla as NoIdeaWahtThisIs\n    q as string = UnityEngine.GameObject.Find(\"a\").transform.name";
			e = "class C:\n  def S():\n    bla as NoIdeaWahtThisIs\n    q as string = UnityEngine.GameObject.Find(\"a\").Transform.Name";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = "var c:NoIdeaWhatThisIs;\nvar a = GameObject.Find(\"a\").transform.name;";
			e = "var c:NoIdeaWhatThisIs;\nvar a = GameObject.Find(\"a\").Transform.Name;";
		}
	}
}
