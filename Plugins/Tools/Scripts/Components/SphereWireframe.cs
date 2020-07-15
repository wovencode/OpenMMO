using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenMMO;

namespace OpenMMO
{

	// ===================================================================================
	// SphereWireframe
	// ===================================================================================
	[DisallowMultipleComponent]
	[RequireComponent(typeof(SphereCollider))]
	public class SphereWireframe : MonoBehaviour
	{
		
		public Color gizmoColor 	= new Color(0, 1, 1, 0.25f);
		public Color gizmoWireColor = new Color(1, 1, 1, 0.8f);
		
		// -------------------------------------------------------------------------------
		// OnDrawGizmos
		// -------------------------------------------------------------------------------
		void OnDrawGizmos()
		{
			SphereCollider collider = GetComponent<SphereCollider>();

			Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
			Gizmos.color = gizmoColor;
			Gizmos.DrawSphere(collider.center, collider.radius);
			Gizmos.color = gizmoWireColor;
			Gizmos.DrawWireSphere(collider.center, collider.radius);
			Gizmos.matrix = Matrix4x4.identity;
		}
	
	}

}

// =======================================================================================