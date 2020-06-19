//BY DX4D
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OpenMMO/Character/Targeting/Raycast Targeting")]
public class RaycastTargetFetcher : TargetFetcher
{
    ///FETCH NEARBY TARGETS
    public override Transform[] Targets(Transform location, float distance)
    {
        List<Transform> transforms = new List<Transform>();

        if (Physics.Raycast(location.position + Vector3.up, location.forward, distance))
        {
            RaycastHit[] hits = Physics.RaycastAll(location.position + Vector3.up, location.forward, distance);

            if (hits != null && hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    transforms.Add(hit.transform);
                }
            }
        }

        return transforms.ToArray();
    }
}
