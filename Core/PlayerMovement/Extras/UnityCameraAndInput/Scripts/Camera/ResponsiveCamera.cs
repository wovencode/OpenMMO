using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ResponsiveCamera : MonoBehaviour
{
    private Camera tempCamera;
    public Camera TempCamera
    {
        get
        {
            if (tempCamera == null)
                tempCamera = GetComponent<Camera>();
            return tempCamera;
        }
    }

    public float targetWidth;
    public float targetHeight;
    
    private void LateUpdate()
    {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = targetWidth / targetHeight;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = TempCamera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;

            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            TempCamera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = TempCamera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;

            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            TempCamera.rect = rect;
        }
    }
}
