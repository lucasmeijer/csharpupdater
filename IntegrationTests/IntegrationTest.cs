using NUnit.Framework;

namespace IntegrationTests
{
	public abstract class IntegrationTest
	{
		delegate void GetTestData(out string input, out string expected);

		[Test]
		public void TestCSharp()
		{
			Test(new CSharpUpdater(), CSharp);
		}

		[Test]
		public void TestBoo()
		{
			Test(new BooUpdater(), Boo);
		}

		private void Test(IScriptUpdater updater, GetTestData getTestData)
		{
			string input;
			string expected;
			getTestData(out input, out expected);

			var output = updater.Update(input);
			Assert.AreEqual(expected, output);
		}

		protected abstract void CSharp(out string i , out string e );
		protected abstract void Boo(out string i, out string e);
	}
}