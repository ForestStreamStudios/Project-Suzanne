using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public Transform goal;
    private NavMeshAgent agent;
    private float speed, defaultSpeed;
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        agent.destination = goal.position;
        defaultSpeed = speed;
    }

    private void Update()
    {
        agent.destination = goal.position;
    }


    public void SetAiSpeed(float newSpeed)
    {
        speed = newSpeed;
        agent.speed = speed;
        //Debug.Log("Ai Speeding Up...");
    }

    public float GetAiSpeed()
    {
        return speed;
    }

    public void ResetAiSpeed()
    {
        speed = defaultSpeed;
        agent.speed = speed;
        //Debug.Log("Ai Resetting Speed...");
    }
}



