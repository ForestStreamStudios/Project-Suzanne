using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PostprocessModels : AssetPostprocessor
{


    Dictionary<string, List<Mesh>> meshLookup;

    void OnPostprocessModel(GameObject root)
    {
        Debug.Log("Finished Importing Models");
        if (meshLookup == null) meshLookup = new Dictionary<string, List<Mesh>>();
        else meshLookup.Clear();

        Apply(root.transform);
    }

    //removes the _001 etc after instance names
    string CleanMeshName(string name)
    {
        int end = name.LastIndexOf('.');
        if (end == -1) return name;

        return name.Remove(end, name.Length - end);
    }

    void Apply(Transform transform)
    {
        MeshFilter mF = transform.GetComponent<MeshFilter>();
        if (mF)
        {
            Mesh mesh = mF.sharedMesh;
            string name = CleanMeshName(mesh.name);

            List<Mesh> relatives;
            if (!meshLookup.TryGetValue(name, out relatives))
            {
                mesh.name = name;
                relatives = new List<Mesh>();
                relatives.Add(mesh);
                meshLookup.Add(name, relatives);
            }
            else
            {
                //Debug.LogFormat("mesh {0} has {1} relatives", name, relatives.Count);
                for (int i = 0; i < relatives.Count; i++)
                {
                    Mesh relMesh = relatives[i];
                    if (CompareForMeshEquality(relMesh, mesh))
                    {
                        //Debug.LogFormat("mesh {0} is instance of mesh {1}; replacing", mesh.name, relMesh.name);
                        mF.sharedMesh = relMesh;

                        MeshCollider mC = transform.GetComponent<MeshCollider>();
                        if (mC)
                        {
                            mC.sharedMesh = relMesh;
                        }

                        GameObject.DestroyImmediate(mesh); //ensures the duplicate is completely removed
                    }
                }
            }
        }

        for (int i = 0; i < transform.childCount; i++) Apply(transform.GetChild(i));
    }

    const float maxBoundsError = 0.001f; //trying to evade floating point shenanigans
    bool CompareForMeshEquality(Mesh a, Mesh b)
    {

        //check if we're actually comparing a mesh object against itself.
        //since we delete 'b' if equal, we actually want to return false here or we'll delete 'a' as well
        if (a == b) return false;

        //check submeshes and vertex counts; this should early-out most non-duplicates
        if (a.subMeshCount != b.subMeshCount || a.vertexCount != b.vertexCount) return false;

        //check bounds centers
        if (Vector3.SqrMagnitude(a.bounds.center - b.bounds.center) > maxBoundsError) return false;
        //check bounds sizes
        if (Vector3.SqrMagnitude(a.bounds.size - b.bounds.size) > maxBoundsError) return false;

        //if not exited as false by now, they're very probably identical
        return true;
    }

}

