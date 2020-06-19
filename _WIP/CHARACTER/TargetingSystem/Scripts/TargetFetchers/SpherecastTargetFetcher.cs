//BY DX4D
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OpenMMO/Character/Targeting/Spherecast Targeting")]
public class SpherecastTargetFetcher : TargetFetcher
{
    ///FETCH NEARBY TARGETS
    public override Transform[] Targets(Transform location, float radius)
    {
        List<Transform> transforms = new List<Transform>();

        if (Physics.CheckSphere(location.position, radius))
        {
            Collider[] hits = Physics.OverlapSphere(location.position, radius);

            if (hits != null && hits.Length > 0)
            {
                foreach (Collider hit in hits)
                {
                    transforms.Add(hit.transform);
                }
            }
        }

        return transforms.ToArray();
    }
}
