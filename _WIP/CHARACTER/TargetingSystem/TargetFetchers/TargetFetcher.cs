//BY DX4D
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetFetcher : ScriptableObject
{
    ///FETCH NEARBY TARGETS
    public abstract Transform[] Targets(Transform location, float radius);
    /*{
        //Targetable nearestTarget = null;
        //distanceToTarget = 999f;
        //GameObject selectedObject = EventSystem.current.currentSelectedGameObject; //Get Currently Selected Game Object
        Collider[] hits = null;

        if (Physics.CheckSphere(location, radius))
        {
            hits = Physics.OverlapSphere(location, radius);

            //if (hits != null && hits.Length > 0) { }
        }

#if UNITY_EDITOR && DEBUG
        System.Text.StringBuilder log = new System.Text.StringBuilder("TARGETS ACQUIRED");
        foreach (Collider hit in hits)
        {
            Debug.Log((hit != null) ? "/n" + hit.name.ToUpper() : "/n-----");
        }
#endif
        return hits;
    }*/
}
