using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Boo.Lang.Compiler.Ast;
using NUnit.Framework;

[TestFixture]
class DocumentTests
{
	[Test]
	public void Tab1()
	{
		Test("\tA", 1, 9, 1);
	}
	[Test]
	public void Tab2()
	{
		Test("...\tA", 1, 9, 4);
	}
	[Test]
	public void Tab3()
	{
		Test(".......\tA", 1, 9, 8);
	}
	[Test]
	public void Tab4()
	{
		Test("........\tA", 1, 9, 8);
	}
	[Test]
	public void Tab5()
	{
		Test("........\tA", 1, 17, 9);
	}

	private static void Test(string input, int line, int column, int expected)
	{
		var doc = new Document(input);
		var offset = doc.LexicalInfoToOffset(new LexicalInfo("bla", line, column));
		Assert.AreEqual(expected, offset);
	}
}
