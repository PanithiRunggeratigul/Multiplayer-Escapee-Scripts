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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    IEnumerator HitObj(Collider objecthit)
    {
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

        yield return new WaitForSeconds(timeduration);

        if (objecthit.gameObject.GetComponent<PlayerController>() != null || objecthit.gameObject.GetComponent<EnemyController>() != null)
        {
            if (objecthit.gameObject.GetComponent<PlayerController>() != null)
            {
                if (objecthit != null)
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
            StartCoroutine(HitObj(objecthit));
        }
    }
}
