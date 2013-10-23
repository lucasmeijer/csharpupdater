using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp.Resolver;
using NUnit.Framework;

namespace csharpupdater
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
			var output = updater.Update(input, GetPipeline);
			Assert.AreEqual(
				expected, output);
		}

		protected abstract IEnumerable<ReplacingAstVisotor> GetPipeline(ReplacementCollector replacementCollector, CSharpAstResolver resolver);
	}
}