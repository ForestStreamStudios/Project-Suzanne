using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    public float updateTime = 5;
    public string aiObjectName; //IMPORTANT!! This name will be used to search ai object and set speed.
    public float targetSmallRadius = 10.0f, targetBigRadius= 30.0f; //Target distance to origin is between targetBigRadius + targetSmallRadius and targetBigRadius - targetSmallRadius
    public float navigationDistance = 100.0f;   //Square of the distance to assume ai has reached its target. Current value is high, because the cube is really tall.

    private float time = 0.0f;
    private bool started = false;
    private GameObject playerObject;
    private static GameObject aiObject;
    private enum aiStates {Chase, RoamPlayer};   //Add any new states here.
    private aiStates currentState = aiStates.RoamPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (playerObject == null)
            playerObject = GameObject.Find("Player");
        if (playerObject == null)
        {
            Debug.LogError("Ai Couldn't Find the Player! Searching by tags...");
            playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject == null)
                Debug.LogError("Ai Couldn't Find Player! Check AI Manager Script.");
        }
        if (aiObjectName != null && aiObjectName.Length > 0)
        {
            aiObject = GameObject.Find(aiObjectName);
        }
        else
        {
            Debug.LogError("Please set aiObjectName for " + this.gameObject.name);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (aiObject != null)
        {
            if (!started)
            {
                if (NavMesh.SamplePosition(Vector3.zero, out NavMeshHit hit, 1000.0f, NavMesh.AllAreas))
                {
                    started = true;
                    Vector3 newPos = GetRandomPointAroundTarget(playerObject.transform.position);
                    newPos.y = aiObject.GetComponent<Collider>().bounds.size.y / 2; //Set Y to half the bound size. This assumes the origin is at the centre. May be replaced by a constant once Suzanne is ready.
                    //Old code, breaks navigation
                    //aiObject.transform.position = newPos;
                    //New version uses NavMeshAgent
                    aiObject.GetComponent<NavMeshAgent>().Warp(newPos);
                    MoveTarget();
                }
            }
            else if ((aiObject.transform.position - transform.position).sqrMagnitude < navigationDistance)
            {
                time += Time.deltaTime;
                if (time >= updateTime)
                {
                    MoveTarget();
                    time = 0.0f;
                }
            }
            
        }
        
    }

    private void MoveTarget()
    {
        Vector3 targetPosition = Vector3.zero;
        if(currentState == aiStates.Chase)
            targetPosition = playerObject.transform.position;
        else if(currentState == aiStates.RoamPlayer)
        {
            targetPosition = GetRandomPointAroundTarget(playerObject.transform.position);
        }

        if(targetPosition != Vector3.zero)
        {
            transform.position = targetPosition;
        }
    }

    private Vector3 GetRandomPointAroundTarget(Vector3 origin)//Random point is within a torus, centred around the target and projected onto the NavMesh
    {
        NavMeshHit hit; //For NavMesh Projection
        float randomAngle = Random.value * 6.28f; // use radians, saves converting degrees to radians
        float cX = Mathf.Sin(randomAngle);
        float cZ = Mathf.Cos(randomAngle);
        Vector3 ringPos = new Vector3(cX, 0, cZ);
        ringPos *= targetBigRadius;
        Vector3 sPos = Random.insideUnitSphere * targetSmallRadius;
        if (NavMesh.SamplePosition((ringPos + sPos + origin), out hit, targetBigRadius, NavMesh.AllAreas))
            return hit.position;    //If hit found
        else
            return Vector3.zero;
    }

 
}
