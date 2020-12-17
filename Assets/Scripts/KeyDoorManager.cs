using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Author: Grant Guenter
public class KeyDoorManager 
{
    private static KeyDoorManager instance;
    public bool keyPickedUp;
    private KeyDoorManager()
    {
        keyPickedUp = false;
    }

    public static KeyDoorManager GetInstance()
    {
        if (instance == null)
        {
            instance = new KeyDoorManager();
        }
        return instance;
    }
    // Start is called before the first frame update
    public void SetKeyPickedUp(bool state)
    {
        keyPickedUp = state;
    }
    void Start()
    {
        keyPickedUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
