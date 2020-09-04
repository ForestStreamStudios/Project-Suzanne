using UnityEngine;

public class SpeedPowerUpObject : PickupObject
{

    [Range(1.0f, 3.0f)]
    public float speedMultiplier = 2f;

    [Range(2.0f, 15.0f)]
    public float speedUpDuration = 5f;

    [Range(2.0f, 15.0f)]
    public float respawnDuration = 10f;

    private float timeOfTrigger;
    private bool isActive;
    private bool isDespawned;

    private PlayerMovement playerMovement;

    private void Despawn() 
    {
        base.objectRenderer.enabled = false;
        base.objectCollider.enabled = false;
        isDespawned = true;
    }

    private void Respawn()
    {
        base.objectRenderer.enabled = true;
        base.objectCollider.enabled = true;
        isDespawned = false;
        Debug.Log("Speed up PowerUp respawned.");
    }

    private void Activate(GameObject player) 
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.speed *= speedMultiplier;
        isActive = true;
    }

    private void DeActivate()
    {
        playerMovement.speed /= speedMultiplier;
        isActive = false;
        Debug.Log("Speed up effect ended.");
    }

    protected override void PickUpEffect(GameObject player) 
    {
        Debug.Log("Speed up effect triggered.");
        Activate(player);
        Despawn();
        timeOfTrigger = Time.time;
    }

    private void Update()
    {
        if (isActive | isDespawned) {

            float timeSinceTrigger = Time.time - timeOfTrigger;

            if (isActive & timeSinceTrigger > speedUpDuration)
            {
                DeActivate();
            }

            if (isDespawned & timeSinceTrigger > respawnDuration)
            {
                Respawn();
            }
        }
    }
}
