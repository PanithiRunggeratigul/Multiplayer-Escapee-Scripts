using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class ItemBomb : MonoBehaviour, IUsable
{
    GameObject currbomb;

    float shootForce = 50;
    float upwardForce = 3;

    Transform attackPoint;
    Camera cam;

    PhotonView PV;



    public void UseItem()
    {
        attackPoint = GameObject.Find("attackpoint").transform;
        cam = GameObject.Find("CameraHolder").GetComponentInChildren<Camera>();
        PV = transform.parent.parent.GetComponentInParent<PhotonView>();

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(70);
        }

        Vector3 direction = targetPoint - attackPoint.position;

        if (PV.IsMine)
        {
            currbomb = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "bomb"), attackPoint.position, Quaternion.identity);
        }
        currbomb.transform.forward = direction.normalized;

        currbomb.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);
        currbomb.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);
    }
}
