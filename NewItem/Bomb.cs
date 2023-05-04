using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Bomb : MonoBehaviour
{
    private Rigidbody rb;

    private bool targetHit;

    float explosionRadius = 6;

    float timeduration;
    GameObject explosion;
    GameObject particle;

    int particleID;
    int explosionID;

    [SerializeField] PhotonView PV;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        timeduration = 5f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetHit)
        {
            return;
        }
        else
        {
            targetHit = true;
        }

        rb.isKinematic = true;

        //transform.SetParent(collision.transform);

        Explode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    IEnumerator StopPlayer(Collider player)
    {
        player.gameObject.GetComponent<PlayerController>().enabled = false;

        yield return new WaitForSeconds(timeduration);

        if (player != null)
        {
            player.gameObject.GetComponent<PlayerController>().enabled = true;

            Destroy(gameObject);
        }
    }

    IEnumerator StopEnemy(Collider enemy)
    {
        enemy.gameObject.GetComponent<EnemyController>().enabled = false;

        yield return new WaitForSeconds(timeduration);

        enemy.gameObject.GetComponent<EnemyController>().enabled = true;

        Destroy(gameObject);
    }

    IEnumerator NotHit()
    {
        yield return new WaitForSeconds(timeduration);
        
        Destroy(gameObject);
    }

    IEnumerator DestroyParticles()
    {
        yield return new WaitForSeconds(timeduration);

        if (particle.GetComponent<PhotonView>().IsMine)
        {
            PhotonNetwork.Destroy(particle);
        }
        if (explosion.GetComponent<PhotonView>().IsMine)
        {
            PhotonNetwork.Destroy(explosion);
        }
    }

    void Explode()
    {
        if (PV.IsMine)
        {
            explosion = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ExplosionParticle"), transform.position, Quaternion.identity);
            particle = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Particle"), transform.position, Quaternion.identity);

            StartCoroutine(DestroyParticles());
        }

        Component[] meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (Component mesh in meshes)
        {
            mesh.GetComponent<MeshRenderer>().enabled = false;
        }

        GetComponent<SphereCollider>().enabled = false;

        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider objecthit in objectsInRange) {
            if (objecthit.gameObject.GetComponent<PlayerController>() != null || objecthit.gameObject.GetComponent<EnemyController>() != null)
            {
                if (objecthit.gameObject.GetComponent<PlayerController>() != null)
                {
                    StartCoroutine(StopPlayer(objecthit));
                }
                if (objecthit.gameObject.GetComponent<EnemyController>() != null)
                {
                    StartCoroutine(StopEnemy(objecthit));
                }
            }
            else if (objecthit.gameObject.GetComponent<PlayerController>() == null && objecthit.gameObject.GetComponent<EnemyController>() == null)
            {
                StartCoroutine(NotHit());
            }
        }
    }

    [PunRPC]
    void Remove_Item(GameObject particle)
    {
        PhotonNetwork.Destroy(particle);
    }
}
