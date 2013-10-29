using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp.Resolver;
using NUnit.Framework;

namespace CSharpUpdater
{
	[TestFixture]
	class PropertyUpperCaserTests : CSharpUpdaterTestsBase
	{
		[Test]
		public void CanUpdatePropertyUsedAsIdentifier()
		{
			var input    = "class C : UnityEngine.MonoBehaviour { void Start() { Debug.Log(name); } }";
			var expected = "class C : UnityEngine.MonoBehaviour { void Start() { Debug.Log(Name); } }";
			Test(expected,input);
		}

		[Test]
		public void CanUpdatePropertyUsedAsMemberReference()
		{
			var input    = "class C { void Start() { UnityEngine.GameObject go = null; string a = go.tag; } }";
			var expected = "class C { void Start() { UnityEngine.GameObject go = null; string a = go.Tag; } }";
			Test(expected, input);
		}

		protected override IEnumerable<ReplacingAstVisitor> PipelineForTest(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
		{
			yield return new PropertyUpperCaser(replacementCollector,resolver);
		}
	}
}