﻿using System.Linq;
using NUnit.Framework;
using ScriptUpdating;

namespace IntegrationTests
{
	public abstract class IntegrationTest
	{
		delegate void GetTestData(out string input, out string expected);

		[Test]
		public void TestCSharp()
		{
			Test(new CSharpUpdater.CSharpUpdater(), CSharp);
		}

		[Test]
		public void TestBoo()
		{
			Test(new BooUpdater.BooUpdater(), Boo);
		}

		[Test]
		public void TestJavascript()
		{
			Test(new JavascriptUpdater.JavascriptUpdater(), Javascript);
		}

		private void Test(IScriptUpdater updater, GetTestData getTestData)
		{
			string input;
			string expected;
			getTestData(out input, out expected);

			var output = updater.Update(new [] { new SourceFile() { Contents = input, FileName = "bla.bs"}});
			Assert.AreEqual(expected, output.First().Contents);
		}

		protected abstract void CSharp(out string i , out string e );
		protected abstract void Boo(out string i, out string e);
		protected abstract void Javascript(out string i, out string e);
	}

	public abstract class MultiFileIntegrationTest
	{
		delegate void GetTestData(out SourceFile[] input, out string expected);

		[Test]
		public void TestCSharp()
		{
			Test(new CSharpUpdater.CSharpUpdater(), CSharp);
		}

		[Test]
		public void TestBoo()
		{
			Test(new BooUpdater.BooUpdater(), Boo);
		}

		[Test]
		public void TestJavascript()
		{
			Test(new JavascriptUpdater.JavascriptUpdater(), Javascript);
		}

		private void Test(IScriptUpdater updater, GetTestData getTestData)
		{
			SourceFile[] input;
			string expected;
			getTestData(out input, out expected);

			var output = updater.Update(input);
			Assert.AreEqual(expected, output.First().Contents);
		}

		protected abstract void CSharp(out SourceFile[] i, out string e);
		protected abstract void Boo(out SourceFile[] i, out string e);
		protected abstract void Javascript(out SourceFile[] i, out string e);
	}
}