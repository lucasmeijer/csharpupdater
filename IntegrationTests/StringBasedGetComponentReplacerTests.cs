using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IntegrationTests.StringBasedGetComponentReplacerTests
{
	[Ignore("not implemented yet")]
	internal class StringBasedGetComponentReplacerTests : IntegrationTest
	{
		protected override void CSharp(out string i, out string e)
		{
			i = "using UnityEngine; class L : MonoBehaviour { void Start() { GetComponent(\"Light\"); } } ";
			e = "using UnityEngine; class L : MonoBehaviour { void Start() { GetComponent(typeof(UnityEngine.Light)); } } ";
		}

		protected override void Boo(out string i, out string e)
		{
			i = "import UnityEngine\nclass L(MonoBehaviour):\n  def S():\n    GetComponent(\"Light\"  )";
			e = "import UnityEngine\nclass L(MonoBehaviour):\n  def S():\n    GetComponent(Light)";
		}

		protected override void Javascript(out string i, out string e)
		{
			throw new NotImplementedException();
		}
	}
}