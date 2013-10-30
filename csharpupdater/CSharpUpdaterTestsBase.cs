using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp.Resolver;
using NUnit.Framework;

namespace CSharpUpdater
{
	internal abstract class CSharpUpdaterTestsBase
	{
		protected void AssertIsNotModified(string input)
		{
			Test(input, input);
		}

		protected void Test(string expected, string input)
		{
			var updater = new CSharpUpdater();
			var output = updater.Update(input, PipelineForTest);
			Assert.AreEqual(expected, output.First().Contents);
		}

		protected abstract IEnumerable<ReplacingAstVisitor> PipelineForTest(ReplacementCollector replacementCollector, CSharpAstResolver resolver);
	}
}