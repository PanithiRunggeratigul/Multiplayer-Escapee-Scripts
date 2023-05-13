using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

interface ICapturable
{
    public void Captured();
}

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;

    float mindist = Mathf.Infinity;
    Transform playerMin = null;

    public LayerMask Ground, Player;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, Ground))
        {
            walkPointSet = true;
        }
    }
    private void ChasePlayer(Transform player)
    {
        agent.SetDestination(player.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, Player);
        foreach (Collider hitCollider in hitColliders)
        {
            hitCollider.gameObject.GetComponentInChildren<ICapturable>()?.Captured();
        }

        if (!playerInAttackRange && !playerInSightRange)
        {
            Patrolling();
        }
        if (!playerInAttackRange && playerInSightRange)
        {
            Collider[] playersInRange = Physics.OverlapSphere(transform.position, sightRange, Player);

            foreach (Collider player in playersInRange)
            {
                float dist = Vector3.Distance(player.transform.position, transform.position);
                if (dist < mindist)
                {
                    playerMin = player.transform;
                    mindist = dist;
                }
            }
            if (playerMin != null)
            {
                ChasePlayer(playerMin);
            }
            else if (playerMin == null)
            {
                mindist = Mathf.Infinity;
            }
        }
    }
}
