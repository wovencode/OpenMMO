using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenMMO;

namespace OpenMMO
{

	// ===================================================================================
	// BoxWireframe
	// ===================================================================================
	[DisallowMultipleComponent]
	[RequireComponent(typeof(BoxCollider))]
	public class BoxWireframe : MonoBehaviour
	{
		
		public Color gizmoColor 	= new Color(0, 1, 1, 0.25f);
		public Color gizmoWireColor = new Color(1, 1, 1, 0.8f);
		
		// -------------------------------------------------------------------------------
		// OnDrawGizmos
		// -------------------------------------------------------------------------------
		void OnDrawGizmos()
		{
			BoxCollider collider = GetComponent<BoxCollider>();

			Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
			Gizmos.color = gizmoColor;
			Gizmos.DrawCube(collider.center, collider.size);
			Gizmos.color = gizmoWireColor;
			Gizmos.DrawWireCube(collider.center, collider.size);
			Gizmos.matrix = Matrix4x4.identity;
		}
	
	}

}

// =======================================================================================