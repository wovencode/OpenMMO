//BY DX4D
using UnityEngine;
using Mirror;

namespace OpenMMO.Targeting
{
    public class TargetingSystem : NetworkBehaviour
    {
        public const bool AUTOTARGETING = false;

        public const int DEFAULT_MAXIMIZED_TARGET_DISTANCE = 999;
        public const int DEFAULT_AUTOTARGET_FREQUENCY = 60;
        public const int DEFAULT_RANGE_CHECK_FREQUENCY = 60;
        public const int DEFAULT_VITAL_CHECK_FREQUENCY = 60;
        public const int DEFAULT_TARGETING_RANGE = 12;
        public const float RANGE_VERIFICATION_TOLERENCE = 0.99f;

        public static KeyCode[] NEXT_TARGET_KEYS = new KeyCode[3] { KeyCode.Tab, KeyCode.RightBracket, KeyCode.Joystick1Button5 };
        public static KeyCode[] LAST_TARGET_KEYS = new KeyCode[2] { KeyCode.LeftBracket, KeyCode.Joystick1Button4 };
        public static KeyCode[] CANCEL_TARGET_KEYS = new KeyCode[2] { KeyCode.Escape, KeyCode.Joystick1Button1 };

        [Header("TARGET FETCHER")]
        [Tooltip("Fetches nearby targets based upon the Fetcher used.")]
        [SerializeField] internal TargetFetcher fetch;

        [Header("SELECTED TARGET")]
        //[Tooltip("A list of objects that can be targeted.")]
        //[SerializeField] internal List<Targetable> selectableTargets = new List<Targetable>();
        [Tooltip("The currently selected target.")]
        [SerializeField] internal Targetable currentTarget;
        [Tooltip("The previously selected target.")]
        [SerializeField] internal Targetable previousTarget;
        [Tooltip("The distance to the target.")]
        [SerializeField] internal float distanceToTarget = DEFAULT_MAXIMIZED_TARGET_DISTANCE;

        [Header("TARGETING CONDITIONS")]
        [Tooltip("Automatically acquires targets when in range.")]
        [SerializeField] internal bool autoTargeting = AUTOTARGETING;
        [Tooltip("The radius of the target area.")]
        [SerializeField] internal float targetingRange = DEFAULT_TARGETING_RANGE;
        [Tooltip("The layers this object can target.")]
        [SerializeField] internal LayerMask targetableLayers = new LayerMask() { value = 1 }; //Default Layer

        [Header("UPDATE FREQUENCIES")]
        [Tooltip("Attempt to acquire a target every X frames.")]
        [SerializeField][Range(1, 600)] internal uint autoTargetFrequency = DEFAULT_AUTOTARGET_FREQUENCY;
        [Tooltip("Verifies the target is still in range every X frames.")]
        [SerializeField][Range(1, 600)] internal uint verifyRangeFrequency = DEFAULT_RANGE_CHECK_FREQUENCY;
        [Tooltip("Verifies the target is still alive every X frames.")]
        [SerializeField][Range(1, 600)] internal uint verifyVitalsFrequency = DEFAULT_VITAL_CHECK_FREQUENCY;

        [Header("KEY ASSIGNMENT")]
        //NEXT TARGET
        [Tooltip("Select the closest target.\ndefault: Tab - R Button - Right Bracket")]
        [SerializeField] internal KeyCode[] nextTargetKey = NEXT_TARGET_KEYS;
        //PREVIOUS TARGET
        [Tooltip("Select the previous target.\ndefault: L Button - Left Bracket")]
        [SerializeField] internal KeyCode[] previousTargetKey = LAST_TARGET_KEYS;
        //CANCEL
        [Tooltip("Cancel the active target.\ndefault: Escape - Cancel")]
        [SerializeField] internal KeyCode[] cancelTargetKey = CANCEL_TARGET_KEYS;

#if _CLIENT
        //VALIDATE
#if UNITY_EDITOR
        [Tooltip("Reset this component to default values?")]
        internal bool resetValues = true;
        private void OnValidate()
        {
            if (fetch == null) { fetch = Resources.Load<TargetFetcher>("Player/Targeting/RaycastTargeting"); }
            if (resetValues)
            {
                resetValues = false; //Resetting Values...

                //KEYS
                nextTargetKey = NEXT_TARGET_KEYS; //R Button
                previousTargetKey = LAST_TARGET_KEYS; //L Button
                cancelTargetKey = CANCEL_TARGET_KEYS; //ESC or B button
                //LAYERS
                targetableLayers = new LayerMask() { value = 1 }; //Default Layer
                //DISTANCES
                distanceToTarget = DEFAULT_MAXIMIZED_TARGET_DISTANCE;
                targetingRange = DEFAULT_TARGETING_RANGE;
                //UPDATE FREQUENCIES
                autoTargetFrequency = DEFAULT_AUTOTARGET_FREQUENCY;
                verifyRangeFrequency = DEFAULT_RANGE_CHECK_FREQUENCY;
                verifyVitalsFrequency = DEFAULT_VITAL_CHECK_FREQUENCY;
            }
        }
#endif

        //ANY KEY IS PRESSED
        bool AnyKeyPressed(KeyCode[] keys)
        {
            foreach (KeyCode key in keys)
            {
                if (Input.GetKeyDown(key)) return true;
            }

            return false;
        }

        //CANCEL TARGET
        public void CancelTarget()
        {
            previousTarget = currentTarget; //SET PREVIOUS TARGET
            currentTarget = null; //REMOVE CURRENT TARGET
            distanceToTarget = DEFAULT_MAXIMIZED_TARGET_DISTANCE; //RESET TARGET DISTANCE
        }
        //FIND LAST TARGET
        public void FindLastTarget(Vector3 location, float range, out Targetable nextTarget)
        {
            nextTarget = previousTarget;
            previousTarget = currentTarget;
        }
        //FIND NEXT TARGET
        public void FindNextTarget(Transform location, float range, out Targetable nextTarget)
        {
            Transform[] targets = fetch.Targets(location, range);

            /*
        #region DEBUG
#if UNITY_EDITOR && DEBUG
            if (targets != null && targets.Length > 0)
            {
                System.Text.StringBuilder log = new System.Text.StringBuilder("<color=green>TARGETS FOUND</color>");

                foreach (Transform t in targets)
                {
                    log.Append("\n" + t.name.ToString());
                }

                Debug.Log(log.ToString());
            }
            else
            {
                Debug.Log("<color=red>NO TARGETS FOUND</color>");
            }
#endif
        #endregion
            */

            Targetable targ = GetNearestTarget(targets);

            if (targ != null)
            {
                previousTarget = currentTarget; //SET PREVIOUS TARGET
                nextTarget = targ;
            }
            else
            {
                nextTarget = currentTarget;
            }
        }
        //GET NEAREST TARGET
        private Targetable GetNearestTarget(Transform[] targetList)
        {
            Targetable nearestTarget = null;

            if (targetList != null && targetList.Length > 0)
            {
                foreach (Transform hit in targetList)
                {
                    Targetable targ = hit.GetComponent<Targetable>();
                    //if (targ == null) targ = hit.transform.GetComponentInChildren<Targetable>();

                    if (targ != null)
                    {
                        float distance = Vector3.Distance(transform.position, targ.position);
                        if (distance <= distanceToTarget)
                        {
                            distanceToTarget = distance;
                            nearestTarget = targ;
                        }
                    }
                }
            }

            return nearestTarget;
        }


        //EVENTS
        bool findNextTarget = false;
        bool findLastTarget = false;
        bool autoTarget = false;
        bool verifyRange = false;
        bool verifyVitals = false;

        uint frameCount = 0;
        //AWAKE
        private void Awake() { frameCount = 0; }
        //UPDATE
        private void Update()
        {
            //if (framecount % tickDelay != 0) return;
            if (!gameObject.activeInHierarchy) return;

            //FRAMECOUNTER
            if (frameCount >= 1200) frameCount = 0;
            frameCount++;

            //NEXT TARGET
            if (!findNextTarget)
            {
                if (AnyKeyPressed(nextTargetKey))
                {
                    findNextTarget = true;
                }
            }
            //PREVIOUS TARGET
            if (!findLastTarget)
            {
                if (AnyKeyPressed(previousTargetKey))
                {
                    findLastTarget = true;
                }
            }
            //AUTOTARGET
            if (!autoTarget && autoTargeting && (frameCount % autoTargetFrequency == 0))
            {
                autoTarget = true;
            }
            //VERIFY RANGE
            if (!verifyRange && (frameCount % verifyRangeFrequency == 0))
            {
                verifyRange = true;
            }
            //CHECK VITALS
            if (!verifyVitals && (frameCount % verifyVitalsFrequency == 0))
            {
                verifyVitals = true;
            }
        }
        
        //LATE UPDATE
        private void LateUpdate()
        {
            if (!gameObject.activeInHierarchy) return;

            //AUTOTARGET
            if (autoTarget)
            {
                autoTarget = false;
                findNextTarget = true;
                //FindNextTarget(transform, targetingRange, out currentTarget);
                return;
            }

            if (findNextTarget) //SELECT NEXT TARGET
            {
                findNextTarget = false;
                Targetable nextTarget;
                FindNextTarget(transform, targetingRange, out nextTarget);

                if (currentTarget != nextTarget && distanceToTarget <= targetingRange) currentTarget = nextTarget;

                return;
            }
            else if (findLastTarget) //SELECT LAST TARGET
            {
                findLastTarget = false;
                Targetable lastTarget;
                FindLastTarget(transform.position, targetingRange, out lastTarget);

                if (currentTarget != lastTarget && distanceToTarget <= targetingRange) currentTarget = lastTarget;
                return;
            }
            else if (currentTarget != null)
            {
                //VERIFY RANGE
                if (verifyRange)
                {
                    verifyRange = false;
                    if (Vector3.Distance(currentTarget.position, transform.position) > targetingRange * RANGE_VERIFICATION_TOLERENCE)
                    {
                        CancelTarget();
                        return;
                    }
                }
                //VERIFY VITALS
                if (verifyVitals)
                {
                    verifyVitals = false;
                    if (!currentTarget.IsAlive)
                    {
                        CancelTarget();
                        return;
                    }
                }
            }
        }
#endif
    }
}

        /*
        //SELECT NEAREST TARGET
        private Targetable SelectNearestTarget(Vector3 location, float radius)
        {
            Targetable nearestTarget = null;
            distanceToTarget = 999f;
            //GameObject selectedObject = EventSystem.current.currentSelectedGameObject; //Get Currently Selected Game Object

            if (Physics.CheckSphere(location, radius))
            {
                Collider[] hits = Physics.OverlapSphere(location, radius);

                if (hits != null && hits.Length > 0)
                {
                    foreach (Collider hit in hits)
                    {
                        Targetable targ = hit.GetComponent<Targetable>();
                        //if (targ == null) targ = hit.transform.parent.GetComponentInChildren<Targetable>();

                        if (targ != null)
                        {
                            float distance = Vector3.Distance(location, targ.position);
                            if (distance < distanceToTarget)
                            {
                                distanceToTarget = distance;
                                nearestTarget = targ;
                            }
                        }
                    }
                }
            }

#if UNITY_EDITOR
#if DEBUG
            Debug.Log("TARGET " + ((nearestTarget != null) ? nearestTarget.name.ToUpper() + " ACQUIRED" : "NOT ACQUIRED"));
#endif
#endif
            if (nearestTarget == null) distanceToTarget = 999f;
            return nearestTarget;
        }
        */

