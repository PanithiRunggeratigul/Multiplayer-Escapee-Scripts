using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Camera");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
