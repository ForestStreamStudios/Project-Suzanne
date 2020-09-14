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
        
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
    }

    private void Update()
    {
            agent.destination = goal.position;
    }
}

