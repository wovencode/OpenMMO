//BY DX4D
#define MULTIPLE_INTERACTIONS
using UnityEngine;
using Mirror;

namespace OpenMMO.Targeting
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
            if (!inUse && Input.GetKeyDown(interactKey))
            {
                inUse = true;
            }
        }
        private void LateUpdate()
        {
            if (inUse)
            {
                inUse = false;
                CmdInteract();
            }
        }

        [Server]
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
        public void CmdInteract() { Interact(); }
    }
}