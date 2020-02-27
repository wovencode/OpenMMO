using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputComponent : MonoBehaviour
{
    public static Vector2 GetPointerPosition(int id)
    {
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0 && id < Input.touchCount)
                return Input.touches[id].position;
            else
                return Vector2.zero;
        }
        else
            return new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
}
