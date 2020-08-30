using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            PickUpEffect(collider.gameObject);
            Destroy(gameObject);
        }
    }

    protected abstract void PickUpEffect(GameObject player); 

}
