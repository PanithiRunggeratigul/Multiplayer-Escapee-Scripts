using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] PhotonView PV;

    private void Start()
    {
        if (PV.IsMine)
        {
            InvokeRepeating("CreateController", 10, 60);
        }
    }

    private void Update()
    {
   
    }

    void CreateController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "EnemyAI"), transform.position, Quaternion.identity);
    }
}
