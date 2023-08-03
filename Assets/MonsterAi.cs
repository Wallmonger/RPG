using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAi : MonoBehaviour
{
    // Input type range
    [Range(0.5f, 50)]
    public float detectDistance = 3;
    int destinationIndex = 0;
    public float runSpeed = 2;
    public Transform[] points;
    public Transform player;
    private NavMeshAgent agent;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        if (agent != null )
        {
            agent.destination = points[destinationIndex].position;
        }
    }

    private void Update()
    {
        Walk();
        SearchPlayer();
        SetMobSize();
    }

    public void SetMobSize()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectDistance + 2)
        {
            iTween.ScaleTo(gameObject, Vector3.one, 0.5f);
        }
    }

    public void SearchPlayer()
    {
        // Vector3.Distance returns a float about the distance between two objects
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectDistance)
        {
            // Player is detected
            agent.destination = player.position;
            agent.speed = runSpeed;
        }
        else
        {
            agent.destination = points[destinationIndex].position;
            agent.speed = 1.5f;
        }
    }

    public void Walk()
    {
        // Agent.remainingDistance returns the distance left between the position of the gameObject, and its destination (defined in Start function)
        float dist = agent.remainingDistance;
        if (dist <= 0.05f)
        {
            destinationIndex++;
            if (destinationIndex > points.Length - 1)
            {
                destinationIndex = 0;
            }

            agent.destination = points[destinationIndex].position;
        }
    }

    // Draw debug object when an object is selected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectDistance);  
    }
}
