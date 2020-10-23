using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Grant Guenter
public class Key : PickupObject
{
    protected override void PickUpEffect(GameObject obj)
    {
        
        KeyDoorManager.instance.keyPickedUp = true;
        
        if (!pickedup)
        {
            sound.Play();
            MeshRenderer render = gameObject.GetComponentInChildren<MeshRenderer>();
            render.enabled = false;
            Debug.Log("key picked up");
        }
        
    }
}
