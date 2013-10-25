using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DepricatedComponentPropertyGetterReplacerKnowledge
{
	public static IEnumerable<Tuple<string, string>> PropertiesToReplace()
	{
		yield return new Tuple<string, string>("UnityEngine.Component.rigidbody","Unity.Runtime.Physics.RigidBody");
		yield return new Tuple<string, string>("UnityEngine.Component.collider", "Unity.Runtime.Physics.Collider");
		yield return new Tuple<string, string>("UnityEngine.Component.constantForce", "Unity.Runtime.Physics.ConstantForce");
		yield return new Tuple<string, string>("UnityEngine.Component.camera", "Unity.Runtime.Rendering.Camera");
		yield return new Tuple<string, string>("UnityEngine.Component.animation", "Unity.Runtime.DepricatedAnimation.Animation");
	}
}