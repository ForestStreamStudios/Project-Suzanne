using System.Collections;
using System.Collections.Generic;
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
    public int mazeSize;
    public Vector3 size;


    // Start is called before the first frame update
    void Start()
    {
        SetSize();
        BakeNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSize()
    {
        center = mazeCenter.position;
       // size = new Vector3(50 * mazeSize, 50, 50 * mazeSize);
    }

    void BakeNavMesh()
    {
        navMeshData = new NavMeshData();
        navMeshInstance = NavMesh.AddNavMeshData(navMeshData);

        NavMeshSourceTag.Collect(ref sources);
        var defaultBuildSettings = NavMesh.GetSettingsByID(0);
        var bounds = new Bounds(center, size);
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
