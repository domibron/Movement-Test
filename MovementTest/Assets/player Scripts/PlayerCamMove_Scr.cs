using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using TMPro;

public class PlayerCamMove_Scr : MonoBehaviour
{
    [SerializeField] WallRun_Scr wallRun;

    [Header("Mouse sensitivity")]

    //old mouse sensitivity so you can set indivisual values to the mouse. Switch this out with the sensitivityMouse
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] float sensitivityMouse = 1; //DO NOT HARD SET THIS VALUE - I AM USING IT FOR EXPERIEMENTS

    [SerializeField] float multiplier = 0.01f;

    [Header("Mouse required objects")]
    //[SerializeField] Transform cam;
    Camera cam;
    [SerializeField] Transform orientation;

    float mouseX;
    float mouseY;

    float xRotation;
    float yRotation;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        //locks the cursor and hides it so you don't see it while playing and click out side
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //gets the mouse input
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        //final mouse rotation
        yRotation += mouseX * sensitivityMouse * multiplier;
        xRotation -= mouseY * sensitivityMouse * multiplier;

        //clamps the rotaion for Up and Down so you can't flip your screen
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotates the camera and the orientation
        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, wallRun.tilt);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
