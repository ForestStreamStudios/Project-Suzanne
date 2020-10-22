using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : PickupObject
{
    private bool pickedup = false;
    protected override void PickUpEffect(GameObject obj)
    {
        
        KeyDoorManager.instance.keyPickedUp = true;
        AudioSource sound = gameObject.GetComponent<AudioSource>();
        if (!pickedup)
        {
            sound.Play();
            MeshRenderer render = gameObject.GetComponentInChildren<MeshRenderer>();
            render.enabled = false;
            Debug.Log("key picked up");
        }
        
    }
}
