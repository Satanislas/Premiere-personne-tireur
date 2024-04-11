using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraStuff : MonoBehaviour
{
    public Camera mainCamera;

    public Camera secondCamera;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            mainCamera.enabled = false;
            secondCamera.enabled = true;
        }
        else
        {
            mainCamera.enabled = true;
            secondCamera.enabled = false;
        }
    }
}
