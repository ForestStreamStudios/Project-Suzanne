using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public Transform goal;
    private NavMeshAgent agent;
    private float speed;

    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        agent.destination = goal.position;
    }

    private void Update()
    {
        agent.destination = goal.position;
    }
}

