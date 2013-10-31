using System;
using System.Collections.Generic;

namespace ScriptUpdating
{
	public static class MemberReferenceReplaceKnowledge
	{
		public static IEnumerable<Tuple<string, string>> Get()
		{
			//yield return new Tuple<string, string>("UnityEngine.Component.gameObject", "SceneObject");
			yield return new Tuple<string, string>("UnityEngine.ParticleSystem.Particle.velocity","speed");
			yield break;
		}
	}
}