using UnityEngine;

//Author Speterius
//Additional code by Grant Guenter
public abstract class PickupObject : MonoBehaviour
{

    protected abstract void PickUpEffect(GameObject player);

    protected Collider objectCollider;
    protected Renderer objectRenderer;
    protected bool pickedup = false;
    public bool soundEffect;
    protected AudioSource sound;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();
        if(soundEffect)
        {
            sound = gameObject.GetComponent<AudioSource>();
        }
        // Set the collider to Trigger
        if (objectCollider != null)
        {
            objectCollider.isTrigger = true;
            Debug.Log("Object Collider Trigger is turned on.");
        }
        else 
        {
            Debug.Log("PickupObject Collider not found.");
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PickUpEffect(collider.gameObject);
            pickedup = true;
        }
        
    }
}
