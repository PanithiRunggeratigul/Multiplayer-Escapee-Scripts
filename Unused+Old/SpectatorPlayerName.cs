using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorPlayerName : MonoBehaviour
{
    // can use with player_name in the SpectatorCameraMovement Script
    [SerializeField] SpectatorCameraMovement cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(cam.player_name);
    }
}
