using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action OnGamePause;
    public event Action OnGameResume;

    // Start is called before the first frame update
    void Start()
    {
        OnResume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Call this method if a pause button or key is pressed
    public void OnPause()
    {
        Cursor.lockState = CursorLockMode.None;
        if (OnGamePause != null)
            OnGamePause();
    }

    public void OnResume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (OnGameResume != null)
            OnGameResume();
    }
}
