using NUnit.Framework;

namespace csharpupdater
{
	internal class CSharpUpdaterTestsBase
	{
		protected static void AssertIsNotModified(string input)
		{
			Test(input, input);
		}

		protected static void Test(string expected, string input)
		{
			var updater = new CSharpUpdater();
			var output = updater.Update(input);
			Assert.AreEqual(
				expected, output);
		}
	}
}