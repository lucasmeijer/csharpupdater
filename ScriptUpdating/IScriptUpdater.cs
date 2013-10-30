using System.Collections.Generic;

namespace ScriptUpdating
{
	public interface IScriptUpdater
	{
		IEnumerable<SourceFile> Update(SourceFile[] input);
		IEnumerable<SourceFile> UpdateSmall(SourceFile[] input);
	}
}