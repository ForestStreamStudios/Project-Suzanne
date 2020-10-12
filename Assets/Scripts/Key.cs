using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : PickupObject
{
    protected override void PickUpEffect(GameObject obj)
    {
        KeyDoorManager.instance.keyPickedUp = true;
        Debug.Log("key picked up");
    }
}
