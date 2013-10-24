using Boo.Lang.Compiler.Ast;

internal abstract class ReplacingAstVisitor : DepthFirstVisitor
{
	protected readonly ReplacementCollector _replacementCollector;

	protected ReplacingAstVisitor(ReplacementCollector replacementCollector)
	{
		_replacementCollector = replacementCollector;
	}
}