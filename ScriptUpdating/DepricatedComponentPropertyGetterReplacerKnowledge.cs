using System;
using System.Collections.Generic;

namespace ScriptUpdating
{
	public class DepricatedComponentPropertyGetterReplacerKnowledge
	{
		public static IEnumerable<Tuple<string, string>> PropertiesToReplace()
		{
			foreach (var tuple in DataFor("UnityEngine.Component")) 
				yield return tuple;
			foreach (var tuple in DataFor("UnityEngine.GameObject"))
				yield return tuple;
		}

		private static IEnumerable<Tuple<string, string>> DataFor(string klass)
		{
			//yield return new Tuple<string, string>(klass + ".rigidbody", "Rigidbody");
			yield return new Tuple<string, string>(klass + ".rigidbody", "Unity.Runtime.Physics.RigidBody");
			yield return new Tuple<string, string>(klass + ".collider", "Collider");
			yield return new Tuple<string, string>(klass + ".constantForce", "ConstantForce");
			yield return new Tuple<string, string>(klass + ".camera", "Camera");
			yield return new Tuple<string, string>(klass + ".animation", "Animation");
			yield return new Tuple<string, string>(klass + ".renderer", "Renderer");
			yield return new Tuple<string, string>(klass + ".light", "Light");
		}
	}
}