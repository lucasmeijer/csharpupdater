using System;
using NUnit.Framework;
using ScriptUpdating;

namespace IntegrationTests
{
	[TestFixture]
	[Ignore("only used for debugging")]
	internal class DebugHelper
	{
		[Test]
		public void Test()
		{
			var booupdater = new JavascriptUpdater.JavascriptUpdater();
			Console.WriteLine(booupdater.UpdateSmall(new[] {SourceFile.For("C:\\Users\\Public\\Documents\\Unity Projects\\4-0_AngryBots\\Assets\\Scripts\\AI\\MechAttackMoveController.js")}));
		}
	}
}
