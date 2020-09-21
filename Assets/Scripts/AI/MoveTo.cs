using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public Transform goal;
    private NavMeshAgent agent;
    private float speed;
    bool disable = true;

    void Start()
    {
        gameObject.AddComponent<NavMeshAgent>();
        agent = GetComponent<NavMeshAgent>();
        agent.radius = 0.5f;
        if (!agent.isOnNavMesh)
        {
            NavMeshHit startPos;
            NavMesh.SamplePosition(gameObject.transform.position, out startPos, 300, NavMesh.AllAreas);
            agent.Warp(startPos.position);
        }
        speed = agent.speed;
    }

    private void Update()
    {
        if (!agent.isOnNavMesh)
        {
            NavMeshHit startPos;
            NavMesh.SamplePosition(gameObject.transform.position, out startPos, 300, NavMesh.AllAreas);
            agent.Warp(startPos.position);
        }
        agent.destination = goal.position; 
    }
}

