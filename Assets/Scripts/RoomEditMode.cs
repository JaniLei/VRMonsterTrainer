using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEditMode : MonoBehaviour
{

    public GameObject selection;
    public Material editModeMaterial;

    GameObject selectionMarker;
    Valve.VR.InteractionSystem.Player player;
    Vector3 pointedPos;

	// Use this for initialization
	void Start ()
    {
        player = FindObjectOfType<Valve.VR.InteractionSystem.Player>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        foreach (var hand in player.hands)
        {
            if (hand.GetStandardInteractionButton())
            {
                Vector3 offset = -hand.transform.forward; // new Vector3(0, 0, -1);
                RaycastHit hit;
                bool bHit = Physics.Linecast(hand.transform.position + offset, hand.transform.forward * 100, out hit);

                if (bHit)
                {
                    if (!selection)
                        selection = hit.transform.gameObject;
                    pointedPos = hit.point;
                }
            }
        }

        if (selection)
        {
            if (!selectionMarker)
            {
                CreateSelectionMarker();
            }
            else
            {
                selectionMarker.transform.position = pointedPos;
                MeshRenderer renderer = selectionMarker.GetComponentInChildren<MeshRenderer>();
                if (renderer != null)
                {
                    RaycastHit rayhit;
                    if (Physics.Raycast(renderer.bounds.center, Vector3.down, out rayhit))
                    {
                        float offsetY = rayhit.point.y - renderer.bounds.min.y;
                        selectionMarker.transform.position += new Vector3(0f, offsetY, 0f);
                    }
                }
            }
            //foreach (Transform t in selection.GetComponents<Transform>())
            //{
            //    MeshRenderer renderer = t.GetComponentInChildren<MeshRenderer>();
            //    if (renderer != null)
            //    {
            //        RaycastHit rayhit;
            //        if (Physics.Raycast(renderer.bounds.center, Vector3.down, out rayhit))
            //        {
            //            float offsetY = rayhit.point.y - renderer.bounds.min.y;
            //            t.position += new Vector3(0f, offsetY, 0f);
            //        }
            //    }
            //}
        }
    }

    void CreateSelectionMarker()
    {
        selectionMarker = new GameObject("Selection Marker");
        MeshFilter meshFilter = selectionMarker.AddComponent<MeshFilter>();
        meshFilter.mesh = selection.GetComponent<MeshFilter>().mesh;
        MeshRenderer meshRenderer = selectionMarker.AddComponent<MeshRenderer>();
        meshRenderer.material = editModeMaterial;
    }
}
