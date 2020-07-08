//by Fhiz
using System;
using UnityEngine;
using UnityEngine.AI;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// This partial section of the EntityMovementComponent is exposed to public and used to check various movement related states.
	/// </summary>
	public partial class EntityMovementComponent
	{
	
		/// <summary>
		/// Checks if the entity is currently moving
		/// </summary>
		public bool GetIsMoving
    	{
    		get
    		{
    			return agent.velocity != Vector3.zero;
    		}
		}

	}

}