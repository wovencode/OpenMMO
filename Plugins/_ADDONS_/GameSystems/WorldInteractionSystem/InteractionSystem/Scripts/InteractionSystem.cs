//BY DX4D
#define MULTIPLE_INTERACTIONS
using UnityEngine;
using Mirror;

namespace OpenMMO
{
    [RequireComponent(typeof(TargetingSystem))]
    public class InteractionSystem : NetworkBehaviour
    {
        [SerializeField] TargetingSystem targeting;
        [SerializeField] KeyCode interactKey = KeyCode.Return;
        [SerializeField] bool inUse = false;

        Targetable target { get { return targeting.currentTarget; } }

        private void OnValidate()
        {
            if (!targeting) targeting = GetComponent<TargetingSystem>();
        }
        private void Update()
        {
            if (!isLocalPlayer) return;
            if (!inUse && Input.GetKeyDown(interactKey))
            {
                inUse = true;
            }
        }
        private void LateUpdate()
        {
            if (!isLocalPlayer) return;

            if (!hasAuthority)
            {
                Debug.Log(gameObject.name.ToUpper() + " HAS NO AUTHORITY");
            }
            // isClient
            if (inUse)
            {
                inUse = false;
                CmdInteract();
                Interact();
            }
        }

        //[Server]
        public virtual void Interact()
        {
            if (target != null)
            {
#if !MULTIPLE_INTERACTIONS
                Interaction interaction = target.GetComponent<Interaction>();
                interaction.Interact(this);
#else
                Interaction[] interactions = target.GetComponents<Interaction>();

                foreach (Interaction interaction in interactions)
                {
                    interaction.Interact(this);
                }
#endif
            }
        }
        
        [Command]
        public void CmdInteract()
        {
            PerformServerInteraction();
        }

        [Server]
        private void PerformServerInteraction() { Interact(); }
    }
}