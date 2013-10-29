using System;
using NUnit.Framework;

namespace IntegrationTests
{
	[TestFixture]
	[Ignore("only used for debugging")]
	internal class DebugHelper
	{
		[Test]
		public void Test()
		{
			var input = System.IO.File.ReadAllText("C:\\Users\\Public\\Documents\\Unity Projects\\4-0_AngryBots\\Assets\\Scripts\\Misc\\StaticFollower.js");
			var booupdater = new JavascriptUpdater();
			Console.WriteLine(booupdater.UpdateSmall(input));
		}
	}
}
