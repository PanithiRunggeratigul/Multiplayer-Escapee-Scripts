using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.AI;

public class Bomb : MonoBehaviour
{
    private Rigidbody rb;

    private bool targetHit;

    float explosionRadius = 6;

    float timeduration;
    GameObject explosion;
    GameObject particle;

    [SerializeField] PhotonView PV;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        timeduration = 5f;
    }

    // stop the bomb movement and explode when the collider touch any other collider
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

        Explode();
    }

    // draw explosion range in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    // using IEnumerator because return WaitForSeconds for the bomb effect duration
    IEnumerator HitObj(Collider objecthit)
    {
        // disable the controller scripts when hit player or enemy
        if (objecthit.gameObject.GetComponent<PlayerController>() != null || objecthit.gameObject.GetComponent<EnemyController>() != null)
        {
            if (objecthit.gameObject.GetComponent<PlayerController>() != null)
            {
                objecthit.gameObject.GetComponent<PlayerController>().enabled = false;
                Animator playerAnimator = objecthit.gameObject.GetComponentInChildren<Animator>();
                playerAnimator.SetFloat("Running", 0);
            }
            if (objecthit.gameObject.GetComponent<EnemyController>() != null)
            {
                objecthit.gameObject.GetComponent<EnemyController>().enabled = false;
                objecthit.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }
        }

        yield return new WaitForSeconds(timeduration);  // wait for timeduration (5 seconds)

        // enable the movement back
        if (objecthit.gameObject.GetComponent<PlayerController>() != null || objecthit.gameObject.GetComponent<EnemyController>() != null)
        {
            if (objecthit.gameObject.GetComponent<PlayerController>() != null)
            {
                if (objecthit != null)  // handle error when there is player quit while getting hit by the bomb
                {
                    objecthit.gameObject.GetComponent<PlayerController>().enabled = true;
                }
            }
            if (objecthit.gameObject.GetComponent<EnemyController>() != null)
            {
                objecthit.gameObject.GetComponent<EnemyController>().enabled = true;
                objecthit.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            }
        }
        Destroy(gameObject);    // destroy the bomb
    }

    // set a time to destroy the particles
    IEnumerator DestroyParticles()
    {
        yield return new WaitForSeconds(timeduration);

        // since the particle and explosion do not have the same PhotonView as the bomb, they must be destroy by this way
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
        if (PV.IsMine)  // to instantiate and destroy once
        {
            explosion = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ExplosionParticle"), transform.position, Quaternion.identity);
            particle = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Particle"), transform.position, Quaternion.identity);

            StartCoroutine(DestroyParticles());
        }

        Component[] meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (Component mesh in meshes)
        {
            mesh.GetComponent<MeshRenderer>().enabled = false;  // hide the bomb before destroy itself after Coroutine end
        }

        GetComponent<SphereCollider>().enabled = false;

        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider objecthit in objectsInRange) {
            StartCoroutine(HitObj(objecthit));  // StartCoroutine to start the 5 seconds timer
        }
    }
}
