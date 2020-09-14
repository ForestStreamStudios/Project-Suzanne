using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

public class NavMeshBaker : MonoBehaviour
{
    /*
     * All objects that are included in navmesh must have NavMeshSourceTag component attached to them. 
     * All objects must have Read Access allowed under the Model section of the Asset.
     * 
     * */
    
    private NavMeshData navMeshData;
    private List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
    NavMeshDataInstance navMeshInstance;

    public Transform mazeCenter;
    Vector3 center;
    public float mazeSize;
    public float cellSize;
    public Vector3 size;
    bool started = false;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!started)
        {
            Thread.Sleep(100);
            started = true;
            SetSize();
            BakeNavMesh();
        }
    }

    public void SetSize()
    {   //size = new Vector3((cellSize * mazeSize), 50, (cellSize * mazeSize));
        //Vector3 size2 = size;
        center = mazeCenter.position; //(size2/2) - new Vector3(cellSize/2, 0,cellSize/2);
        Debug.Log("Center:" + center.ToString());
        Debug.Log("Cellsize:"+cellSize+" MazeSize:"+mazeSize+ " Size:" + center.ToString());
    }

    void BakeNavMesh()
    {
        navMeshData = new NavMeshData(0);
        navMeshInstance = NavMesh.AddNavMeshData(navMeshData);

        NavMeshSourceTag.Collect(ref sources);
        var defaultBuildSettings = NavMesh.GetSettingsByID(0);
        var bounds = new Bounds(center, size);
        Debug.Log("Sources: " + sources.Count);
        NavMeshBuilder.UpdateNavMeshData(navMeshData,defaultBuildSettings,sources,bounds);
    }
    void OnDrawGizmosSelected()
    {
        center = mazeCenter.position;
        if (navMeshData)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(navMeshData.sourceBounds.center, navMeshData.sourceBounds.size);
        }

        Gizmos.color = Color.yellow;
        var bounds = new Bounds(center, size);
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, size);
    }
}
