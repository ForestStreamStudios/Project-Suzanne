using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KeyDoorManager : ScriptableSingleton<KeyDoorManager>
{
    public bool keyPickedUp;
    // Start is called before the first frame update
    void Start()
    {
        keyPickedUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
