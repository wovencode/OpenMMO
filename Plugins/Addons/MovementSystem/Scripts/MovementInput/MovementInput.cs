//by Fhiz, DX4D
using UnityEngine;

namespace OpenMMO
{
    /// <summary>
    /// Holds data related to the current movement input state of a controlled character.
    /// </summary>
    public partial struct MovementInput
    {
        //ORIENTATION
        public Vector3 position;
        public Quaternion rotation;

        //MOVE
        public float verticalInput;
        public float horizontalInput;

        //STRAFE
        public bool strafeLeft;
        public bool strafeRight;

        //RUN
        public bool running;

        //SNEAK
        public bool sneaking;

        //CONSTRUCTOR
        /// <summary>Struct Constructor. Passes all properties to the struct and initializes them.</summary>
    	public MovementInput(Vector3 _position, Quaternion _rotation, MovementProfile _data)// float _verticalInput, float _horizontalInput, bool _running, bool _sneaking, bool _strafeLeft, bool _strafeRight)
        {
            //ORIENTATION
            position = _position;
            rotation = _rotation;
            
            //MOVE
            verticalInput = _data.verticalMovement;
            horizontalInput = _data.horizontalMovement;

            //STRAFE
            strafeLeft = _data.strafeLeft;
            strafeRight = _data.strafeRight;

            //RUN
            running = _data.running;

            //SNEAK
            sneaking = _data.sneaking;
        }

    }
}