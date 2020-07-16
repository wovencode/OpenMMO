using UnityEngine;

namespace OpenMMO
{
    public class Orbit : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float orbitSpeed = 5.0f;
        [SerializeField] Vector3 orbitOffset = new Vector3(0f, 5f, 6f);

        Vector3 offset;

        private void Start()
        {
            offset = new Vector3(target.position.x + orbitOffset.x,
                target.position.y + orbitOffset.y,
                target.position.z + orbitOffset.z);
        }

        private void LateUpdate()
        {
            if (!target)
            {
                if (!PlayerAccount.localPlayer) { return; }
                else { target = PlayerAccount.localPlayer.transform; }
            }

            //if (!Input.GetKey(KeyCode.Mouse1)) return;

            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * orbitSpeed, Vector3.up) * offset;
            transform.position = target.position + offset;
            transform.LookAt(target.position);
        }
    }
}