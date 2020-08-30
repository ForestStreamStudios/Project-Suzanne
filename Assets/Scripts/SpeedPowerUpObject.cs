using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUpObject : PickupObject
{

    public float speedMultiplier = 2f;
    public float speedUpDuration = 5f;

    protected override void PickUpEffect(GameObject player) 
    {
        Debug.Log("Speed up effect triggered.");

        player.GetComponent<PlayerMovement>().speed *= speedMultiplier;    
    }
}
