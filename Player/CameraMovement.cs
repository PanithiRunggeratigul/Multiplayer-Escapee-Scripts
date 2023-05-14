using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float sensitivity;

    [SerializeField] Slider slider;
    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;
    [SerializeField] Transform sprite;

    float xRotation;
    float yRotation;
    float multiplier = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        if (sensitivity <= 50)
        {
            sensitivity = 50;
            PlayerPrefs.SetFloat("currentSensitivity", sensitivity);
        }

        sensitivity = PlayerPrefs.GetFloat("currentSensitivity");
        slider.value = sensitivity;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("currentSensitivity", sensitivity);    // set the sensitivity to PlayerPrefs

        // mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * multiplier * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier * sensitivity;

        // these are the wa unity handle mouse input with 3D rotation
        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // limit x rotation to not goes over player

        // rotate cam, orientation, and player model 
        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        sprite.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    // set sensitivity from the settings
    public void ChangeSensitivity(float newMouseSens)
    {
        sensitivity = newMouseSens;
        PlayerPrefs.SetFloat("currentSensitivity", sensitivity);
    }
}
