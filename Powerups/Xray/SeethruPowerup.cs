using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeethruPowerup : PickupObject
{
    protected override void PickUpEffect(GameObject player)
    {
        throw new System.NotImplementedException();
        XrayWalls.isEffectActive = true;
    }
}
