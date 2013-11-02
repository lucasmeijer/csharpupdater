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
			yield return new Tuple<string, string>(klass + ".rigidbody", "Rigidbody");
			yield return new Tuple<string, string>(klass + ".rigidbody2D", "Rigidbody2D");
			//yield return new Tuple<string, string>(klass + ".rigidbody", "Unity.Runtime.Physics.RigidBody");
			yield return new Tuple<string, string>(klass + ".collider", "Collider");
			yield return new Tuple<string, string>(klass + ".collider2D", "Collider2D");
			yield return new Tuple<string, string>(klass + ".constantForce", "ConstantForce");
			yield return new Tuple<string, string>(klass + ".camera", "Camera");
			yield return new Tuple<string, string>(klass + ".animation", "Animation");
			yield return new Tuple<string, string>(klass + ".renderer", "Renderer");
			yield return new Tuple<string, string>(klass + ".light", "Light");
			yield return new Tuple<string, string>(klass + ".audio", "AudioSource");
			yield return new Tuple<string, string>(klass + ".guiText", "GUIText");
			yield return new Tuple<string, string>(klass + ".particleSystem", "ParticleSystem");
			yield return new Tuple<string, string>(klass + ".particleEmitter", "ParticleEmitter");
			yield return new Tuple<string, string>(klass + ".networkView", "NetworkView");
			yield return new Tuple<string, string>(klass + ".guiTexture", "GUITexture");
		}
	}
}