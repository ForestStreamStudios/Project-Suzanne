using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

//Author: Grant Guenter
public class UIManager : MonoBehaviour
{

    public Button resetButton;
    public string levelZero;
    public string levelOne;
    // Start is called before the first frame update
    void Start()
    {
        resetButton.onClick.AddListener(ResetLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResetLevel()
    {
        Cursor.visible = false;
        SceneManager.LoadScene(levelZero, LoadSceneMode.Single);
    }

    
}
