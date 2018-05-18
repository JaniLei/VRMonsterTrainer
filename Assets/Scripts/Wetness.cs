using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wetness : MonoBehaviour
{

    public MeshRenderer Wet;
    public float GlossyValue = 2.1f;
    public float DefaultValue = 0.25f;

    public bool isWet;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isWet)
        {
            Wet.material.SetFloat("_Glossiness", GlossyValue);
        }
        else
        {
            Wet.material.SetFloat("_Glossiness", GlossyValue);
        }

    }
}
