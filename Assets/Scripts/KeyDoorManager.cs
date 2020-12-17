using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Author: Grant Guenter
public class KeyDoorManager : Singleton
{
    public bool keyPickedUp;
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

public class Singleton:MonoBehaviour
{
    private static Singleton instance;
    protected Singleton()
    {
    }
    public static Singleton GetInstance()
    {
        if(instance == null)
        {
            instance = new Singleton();
        }
        return instance;
    }
}
