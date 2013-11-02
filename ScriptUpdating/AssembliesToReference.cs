using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptUpdating
{
	public class AssembliesToReference
	{
		public static string[] Get()
		{
			return new[]
			       {
				       "C:/Program Files (x86)/Unity/Editor/Data/Managed/UnityEngine.dll"
					   ,"C:/Program Files (x86)/Unity/Editor/Data/Managed/UnityEditor.dll"
			       };
		}
	}
}
