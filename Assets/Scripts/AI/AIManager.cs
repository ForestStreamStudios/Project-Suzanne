using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    public float waitTimeDefault = 5;
    public string aiObjectName; //IMPORTANT!! This name will be used to search ai object and set speed.
    public float targetSmallRadiusDefault = 10.0f, targetBigRadiusDefault= 30.0f, radiusDecayRate = 2.0f; //Target distance to origin is between targetBigRadius + targetSmallRadius and targetBigRadius - targetSmallRadius. Decay rate makes the ring close on the player over time(TODO). 
    public float navigationDistance = 5.0f;   //Square of the distance to assume ai has reached its target.
    public float aiEyeHeight = 2.0f;           //Adds height to the position.

    private float time = 0.0f;
    private bool started = false;
    private GameObject playerObject;
    private  GameObject aiObject;
    private float targetSmallRadius , targetBigRadius;
    private enum aiStates {Chase, RoamPlayer};   //Add any new states here.
    private aiStates currentState = aiStates.RoamPlayer;
    void Start()
    {
        targetBigRadius = targetBigRadiusDefault;
        targetSmallRadius = targetSmallRadiusDefault;
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
            CheckPerception();
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
            {   //Arrived at target:
                time += Time.deltaTime;
                if (time >= waitTimeDefault)
                {
                    MoveTarget();
                    time = 0.0f;
                    waitTimeDefault = Mathf.Abs(Random.value) % 5.0f + 2.0f;
                    //Could add search animation here.
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

    private void CheckPerception()
    {
        RaycastHit hit;
        int layermask = 1 << 3; //Set up to ignore IgnoreRaycast layer. Change the shifting if layers change!!
        layermask = ~layermask;
        Ray ray = new Ray(playerObject.transform.position, new Vector3(aiObject.transform.position.x,aiObject.transform.position.y + aiEyeHeight, aiObject.transform.position.z) - playerObject.transform.position);
        Debug.DrawRay(playerObject.transform.position, new Vector3(aiObject.transform.position.x, aiObject.transform.position.y + aiEyeHeight, aiObject.transform.position.z) - playerObject.transform.position, Color.green, 1.0f);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            //Debug.Log(hit.collider.gameObject);
            if(hit.collider.gameObject == aiObject)
            {
                SetStateToChase();
            }
            else
            {
                SetStateToRoamPlayer();
            }
        }
        else
        {
            //No hit. Not possible, but just in case...
            SetStateToChase();
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

    
    public void SetStateToChase()
    {
        if(currentState != aiStates.Chase)
        {
            currentState = aiStates.Chase;
            MoveTarget();
        }
        else
        {
            MoveTarget();
        }
    }

    public void SetStateToRoamPlayer()
    {
        if(currentState != aiStates.RoamPlayer)
        {
            //A search logic could be implemented here...
            currentState = aiStates.RoamPlayer;

        }
        
    }
}
