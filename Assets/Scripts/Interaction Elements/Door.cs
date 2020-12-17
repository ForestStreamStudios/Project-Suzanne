using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Author: Grant Guenter
public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    [Header ("Next Scene")]
    public string sceneName;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("door collided");
        if (other.gameObject.CompareTag("Player") && KeyDoorManager.GetInstance().keyPickedUp)
        {
            Cursor.lockState = UnityEngine.CursorLockMode.Confined;
            Cursor.visible = true;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

        }
    }
}
