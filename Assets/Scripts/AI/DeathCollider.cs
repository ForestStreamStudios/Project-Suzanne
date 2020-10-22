using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Author: Grant Guenter
public class DeathCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            Cursor.lockState = UnityEngine.CursorLockMode.Confined;
            Cursor.visible = true;
            SceneManager.LoadScene("Death Screen",LoadSceneMode.Single);

        }
    }
}

