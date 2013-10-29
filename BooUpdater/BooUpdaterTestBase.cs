using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;
using NUnit.Framework;
using ScriptUpdating;

namespace BooUpdater
{
	internal abstract class BooUpdaterTestBase
	{
		protected void Test(string expected, string input)
		{
			var updater = new BooUpdater();
			var output = updater.Update(new []{new SourceFile() {Contents = input, FileName = "bla.cs"}}, PipeLineForTest);
			Assert.AreEqual(expected, output);
		}

		protected abstract IEnumerable<DepthFirstVisitor> PipeLineForTest(ReplacementCollector collector, Document document);
	}
}