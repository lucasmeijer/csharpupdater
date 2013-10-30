using System.Collections.Generic;

namespace ScriptUpdating
{
	public interface IScriptUpdater
	{
		IEnumerable<SourceFile> Update(IEnumerable<SourceFile> input);
		IEnumerable<SourceFile> UpdateSmall(IEnumerable<SourceFile> input);
	}
}