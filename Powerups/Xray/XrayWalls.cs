using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XrayWalls : MonoBehaviour
{

    //Stick this to the prefab that the maze generator uses
    //Note that it must have a MeshRenderer

    public MeshRenderer renderer;
    private List<Material> materials;
    private int matLength;
    public static Boolean isEffectActive;
    public Material xrayMat;
    public GameObject cam;

    public void Start()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
        materials = new List<Material>();
        renderer.GetMaterials(materials);
        matLength = renderer.materials.Length;
    }

    public void Update()
    {
        float distance = Vector3.Distance(cam.transform.position, gameObject.transform.position);
        float opacity = distance / 15;
        if (opacity>0.7f)
        {
            opacity = 0.7f;
        }
        else if(opacity<0.1f)
        {
            opacity = 0.1f;
        }

        Debug.Log(opacity);

        xrayMat.SetFloat("_Opacity", opacity);
        if (isEffectActive)
        {
            gameObject.GetComponent<Renderer>().material = xrayMat;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = materials[0];
        }
    }

}
