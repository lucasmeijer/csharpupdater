﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptUpdating
{
	public class PreprocessorDefines
	{
		public static IEnumerable<string> Get()
		{
			return new[] { "UNITY_EDITOR", "ENABLE_2D_PHYSICS", "ENABLE_PHYSICS", "ENABLE_MOVIES", "ENABLE_WWW" };
		}
	}
}
