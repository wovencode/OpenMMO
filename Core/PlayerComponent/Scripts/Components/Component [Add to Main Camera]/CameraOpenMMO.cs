
using UnityEngine;
using OpenMMO;

namespace OpenMMO {

	// ===================================================================================
	// CameraOpenMMO
	// ===================================================================================
	[DisallowMultipleComponent]
	public class CameraOpenMMO : MonoBehaviour
	{
		
		[Header("Camera HotKey")]
		public KeyCode hotKey = KeyCode.LeftControl;
		
		public Transform target; 
	
		public float targetHeight = 2.5f; 
		public float distance = 10.0f;
		public float offsetFromWall = 0.1f;

		public float maxDistance = 30; 
		public float minDistance = 3f; 

		public float xSpeed = 200.0f; 
		public float ySpeed = 200.0f; 

		public int yMinLimit = -80; 
		public int yMaxLimit = 80; 

		public int zoomRate = 200; 

		public float rotationDampening = 3.0f; 
		public float zoomDampening = 5.0f; 
	
		public LayerMask collisionLayers = -1;

		private float xDeg = 0.0f; 
		private float yDeg = 0.0f; 
		private float currentDistance; 
		private float desiredDistance; 
		private float correctedDistance; 
		
		// -------------------------------------------------------------------------------
		// Start
		// -------------------------------------------------------------------------------
		void Start() 
		{ 
			Vector3 angles = transform.eulerAngles; 
			xDeg = angles.x; 
			yDeg = angles.y; 

			currentDistance = distance; 
			desiredDistance = distance; 
			correctedDistance = distance; 

			// Make the rigid body not change rotation 
			if (GetComponent<Rigidbody>()) 
				GetComponent<Rigidbody>().freezeRotation = true; 
		} 
		
		// -------------------------------------------------------------------------------
		// LateUpdate
		// -------------------------------------------------------------------------------
		void LateUpdate() 
		{ 
			Vector3 vTargetOffset;
		
		  	if (!target)
            {
            
                GameObject player = PlayerComponent.localPlayer;
                
                if (player)
                	target = player.transform;
                else
                	return;
            }
            
			// If either mouse buttons are down, let the mouse govern camera position 
			// OR: if the input key is pressed and not currently any other input active
			if (Input.GetMouseButton(1) || (Input.GetKey(hotKey) && !Tools.AnyInputFocused) ) 
			{ 
				xDeg += Input.GetAxis ("Mouse X") * xSpeed * 0.02f; 
				yDeg -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f; 
			} 
			// otherwise, ease behind the target if any of the directional keys are pressed 
			else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) 
			{ 
				float targetRotationAngle = target.eulerAngles.y; 
				float currentRotationAngle = transform.eulerAngles.y; 
				xDeg = Mathf.LerpAngle (currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime); 
			} 

			yDeg = Tools.ClampAngleBetweenMinAndMax(yDeg, yMinLimit, yMaxLimit); 

			// set camera rotation 
			Quaternion rotation = Quaternion.Euler (yDeg, xDeg, 0); 

			// calculate the desired distance 
			desiredDistance -= Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs (desiredDistance); 
			desiredDistance = Mathf.Clamp (desiredDistance, minDistance, maxDistance); 
			correctedDistance = desiredDistance; 

			// calculate desired camera position
			vTargetOffset = new Vector3 (0, -targetHeight, 0);
			Vector3 position = target.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset); 

			// check for collision using the true target's desired registration point as set by user using height 
			RaycastHit collisionHit; 
			Vector3 trueTargetPosition = new Vector3 (target.position.x, target.position.y + targetHeight, target.position.z); 

			// if there was a collision, correct the camera position and calculate the corrected distance 
			bool isCorrected = false; 
			if (Physics.Linecast (trueTargetPosition, position, out collisionHit, collisionLayers.value)) 
			{ 
				// calculate the distance from the original estimated position to the collision location,
				// subtracting out a safety "offset" distance from the object we hit.  The offset will help
				// keep the camera from being right on top of the surface we hit, which usually shows up as
				// the surface geometry getting partially clipped by the camera's front clipping plane.
				correctedDistance = Vector3.Distance (trueTargetPosition, collisionHit.point) - offsetFromWall; 
				isCorrected = true;
			}

			// For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance 
			currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp (currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance; 

			// keep within legal limits
			currentDistance = Mathf.Clamp (currentDistance, minDistance, maxDistance); 

			// recalculate position based on the new currentDistance 
			position = target.position - (rotation * Vector3.forward * currentDistance + vTargetOffset); 
		
			transform.rotation = rotation; 
			transform.position = position; 
		} 
	
	/*
	[DisallowMultipleComponent]
	public class CameraOpenMMO : FollowCamera
	{

		[Header("Camera Key")]
		public KeyCode hotKey = KeyCode.LeftControl;

		[Header("Controls")]
		public bool updateRotation = true;
		public bool updateRotationX = false;
		public bool updateRotationY = false;
		public bool updateZoom = true;

		[Header("X Rotation")]
		public bool limitXRotation;
		[Range(-360, 360)]
		public float minXRotation = 0;
		[Range(-360, 360)]
		public float maxXRotation = 0;
		public bool smoothRotateX;
		public float rotateXSmoothing = 10.0f;

		[Header("Y Rotation")]
		public bool limitYRotation;
		[Range(-360, 360)]
		public float minYRotation = 0;
		[Range(-360, 360)]
		public float maxYRotation = 0;
		public bool smoothRotateY;
		public float rotateYSmoothing = 10.0f;

		[Header("General Rotation Settings")]
		public float startXRotation;
		public float startYRotation;
		public float rotationSpeed = 5;

		[Header("Zoom")]
		public bool limitZoomDistance;
		public float minZoomDistance;
		public float maxZoomDistance;
		public bool smoothZoom;
		public float zoomSmoothing = 10.0f;

		[Header("General Zoom Settings")]
		public float startZoomDistance;
		public float zoomSpeed = 5;

		private float xVelocity;
		private float yVelocity;
		private float zoomVelocity;
		
		// -------------------------------------------------------------------------------
		// Start
		// -------------------------------------------------------------------------------
		protected virtual void Start()
		{
			xRotation = startXRotation;
			yRotation = startYRotation;
			zoomDistance = startZoomDistance;
		}
		
		// -------------------------------------------------------------------------------
		// Update
		// -------------------------------------------------------------------------------
		protected virtual void Update()
        {
            if (!target)
            {
                GameObject player = PlayerComponent.localPlayer;
                if (player) target = player.transform;
            }

            float deltaTime = Time.deltaTime;
		
			if (Input.GetKey(hotKey))
				useTargetYRotation = false;
			else
				useTargetYRotation = true;
		
			// X rotation
			if (updateRotation || updateRotationX)
				xVelocity += InputManager.GetAxis("Mouse Y", false) * rotationSpeed;
		
			if (Input.GetKey(hotKey))
				xRotation -= xVelocity;
		
			if (limitXRotation)
				xRotation = ClampAngleBetweenMinAndMax(xRotation, minXRotation, maxXRotation);
			else
				xRotation = ClampAngleBetweenMinAndMax(xRotation, -360, 360);

			// Y rotation
			if (updateRotation || updateRotationY)
				yVelocity += InputManager.GetAxis("Mouse X", false) * rotationSpeed;
		
			if (Input.GetKey(hotKey) && !Tools.AnyInputFocused)
				yRotation += yVelocity;
		
			if (limitYRotation)
				yRotation = ClampAngleBetweenMinAndMax(yRotation, minYRotation, maxYRotation);
			else
				yRotation = ClampAngleBetweenMinAndMax(yRotation, -360, 360);

			// Zoom
			if (updateZoom)
				zoomVelocity += InputManager.GetAxis("Mouse ScrollWheel", false) * zoomSpeed;
		
			if (Input.GetKey(hotKey))
				zoomDistance += zoomVelocity;
		
			if (limitZoomDistance)
				zoomDistance = Mathf.Clamp(zoomDistance, minZoomDistance, maxZoomDistance);

			// X rotation smooth
			if (smoothRotateX)
				xVelocity = Mathf.Lerp(xVelocity, 0, deltaTime * rotateXSmoothing);
			else
				xVelocity = 0f;

			// Y rotation smooth
			if (smoothRotateY)
				yVelocity = Mathf.Lerp(yVelocity, 0, deltaTime * rotateYSmoothing);
			else
				yVelocity = 0f;

			// Zoom smooth
			if (smoothZoom)
				zoomVelocity = Mathf.Lerp(zoomVelocity, 0, deltaTime * zoomSmoothing);
			else
				zoomVelocity = 0f;
		}
		
		
		*/
		// -------------------------------------------------------------------------------
		
	}
	
	// ===================================================================================

}