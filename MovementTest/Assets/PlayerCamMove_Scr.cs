using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using TMPro;

public class PlayerCamMove_Scr : MonoBehaviour
{
    public Camera fpsCam;
    float range = 2f;

    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;

    public float sensitivityMouse;

    float mouseX;
    float mouseY;

    float multiplier = 1f;

    float xRotation;
    float yRotation;

    private void Start()
    {
        /*
        //loading save values and if not default values are used instead.
        if (Save_Manager.instance.hasLoaded)
        {
            sensitivityMouse = Save_Manager.instance.activeSave.MainMouseSensitivity;
            MouseSens.value = Save_Manager.instance.activeSave.MainMouseSensitivity;

        }
        else
        {
            Save_Manager.instance.activeSave.MainMouseSensitivity = 1f;
        }

        //calls the load mouse settings funtion
        LoadMouseSettings();
        */

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
