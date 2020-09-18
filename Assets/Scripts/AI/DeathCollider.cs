using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Debug.Log("collision detected");
        if (other.gameObject.CompareTag("Respawn"))
        {
            Cursor.lockState = UnityEngine.CursorLockMode.Confined;
            RemoveMaze();
            SceneManager.LoadScene("Death Screen");
        }
    }
    private void RemoveMaze()
    {
        GameObject.FindGameObjectWithTag("MazeGenerator").GetComponent<MazeGenerator>().DestroyMaze();
    }
}

