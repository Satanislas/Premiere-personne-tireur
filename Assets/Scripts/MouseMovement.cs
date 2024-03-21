using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    public float topClamp = -90f;
    public float botClamp = 90f;
    
    // Start is called before the first frame update
    void Start()
    {
        //makes the cursor invisible and in the center
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //getting the inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        //rotate around the x axis
        xRotation -= mouseY;
        //clamp rotation
        xRotation = Math.Clamp(xRotation, topClamp, botClamp);
        
        //rotate around the y axis
        yRotation += mouseX;
        
        //apply the rotation
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
