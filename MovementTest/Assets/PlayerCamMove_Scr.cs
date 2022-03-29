using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using TMPro;

public class PlayerCamMove_Scr : MonoBehaviour
{
    [Header("Mouse sensitivity")]

    //old mouse sensitivity so you can set indivisual values to the mouse. Switch this out with the sensitivityMouse
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] float sensitivityMouse = 1; //DO NOT HARD SET THIS VALUE - I AM USING IT FOR EXPERIEMENTS

    [SerializeField] float multiplier = 1f;

    [Header("Mouse required objects")]
    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;

    float mouseX;
    float mouseY;

    float xRotation;
    float yRotation;

    private void Start()
    {
        //locks the cursor and hides it so you don't see it while playing and click out side
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //because this will run if the game is paused but this if statement stops that
        if (Time.timeScale == 1)
        {
            //gets the mouse input
            mouseX = Input.GetAxisRaw("Mouse X");
            mouseY = Input.GetAxisRaw("Mouse Y");

            //final mouse rotation
            yRotation += mouseX * sensitivityMouse * multiplier;
            xRotation -= mouseY * sensitivityMouse * multiplier;

            //clamps the rotaion for Up and Down so you can't flip your screen
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //rotates the camera and o=the orientation
            cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
