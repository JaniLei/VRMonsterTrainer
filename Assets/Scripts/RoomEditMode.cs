using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEditMode : MonoBehaviour
{

    public GameObject selection;
    public Material editModeMat;
    public Material overlappingMat;

    GameObject selectionMarker;
    MeshRenderer markerRenderer;
    Valve.VR.InteractionSystem.Player player;
    Vector3 pointedPos;
    bool markerOverlapping;
    
	void Start()
    {
        player = FindObjectOfType<Valve.VR.InteractionSystem.Player>();
	}
	
	void Update()
    {
        foreach (var hand in player.hands)
        {
            if (hand.GetStandardInteractionButtonDown())
            {
                if (!selection)
                {
                    Vector3 offset = -hand.transform.forward; // new Vector3(0, 0, -1);
                    RaycastHit hit;
                    //bool bHit = Physics.Linecast(hand.transform.position, hand.transform.position + -hand.transform.up * 100, out hit);
                    bool bHit = Physics.Linecast(player.hmdTransform.position, hand.transform.position, out hit);


                    if (bHit)
                    {
                        selection = hit.transform.gameObject;
                        pointedPos = hit.point;
                    }
                }
                else
                {
                    if (markerOverlapping)
                        CancelSelection();
                    else
                        ConfirmSelection();
                }

                if (selection)
                {
                    if (hand.controller != null && hand.controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
                    {
                        Vector2 axis = hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
                        Vector3 rot = new Vector3(axis.x, 0, axis.y);
                        Quaternion rotation = Quaternion.LookRotation(rot);
                        selectionMarker.transform.rotation = rotation;
                    }
                }
            }

            if (selection)
            {
                Vector3 offset = -hand.transform.forward; // new Vector3(0, 0, -1);
                RaycastHit hit;
                //bool bHit = Physics.Linecast(hand.transform.position, hand.transform.position + -hand.transform.up * 100, out hit);
                bool bHit = Physics.Linecast(player.hmdTransform.position, hand.transform.position, out hit);

                if (bHit)
                {
                    pointedPos = hit.point;
                }
            }

            if (selectionMarker)
            {
                float hAxis = Input.GetAxis("Horizontal");
                float vAxis = Input.GetAxis("Vertical");
                if (Mathf.Abs(hAxis) + Mathf.Abs(vAxis) > 0)
                {
                    Vector3 rot = new Vector3(hAxis, 0, vAxis);
                    //rot = (rot + player.transform.forward/*<-replace with player body rotation*/).normalized;

                    Quaternion rotation = Quaternion.LookRotation(rot, selectionMarker.transform.up);
                    //selectionMarker.transform.rotation = /*rotation*/Quaternion.RotateTowards(selectionMarker.transform.rotation, rotation, 1);
                    selectionMarker.transform.rotation = rotation;

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

                    if (Physics.Raycast(/*renderer.bounds.center*/selectionMarker.transform.position, Vector3.down, out rayhit))
                    {
                        float offsetY = rayhit.point.y - renderer.bounds.min.y;
                        selectionMarker.transform.position += new Vector3(0f, offsetY, 0f);
                    }
                    Vector3 offset = new Vector3(0, 0.01f, 0);
                    Collider[] colls = Physics.OverlapBox(/*selectionMarker.transform.position*/renderer.bounds.center + offset, /*selectionMarker.transform.localScale / 2*/renderer.bounds.size / 2);
                    if (colls.Length > 0)
                        markerOverlapping = true;
                    else
                        markerOverlapping = false;
                }
            }

            if (markerOverlapping)
            {
                markerRenderer.material = overlappingMat;
            }
            else
            {
                markerRenderer.material = editModeMat;
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
        selection.GetComponent<Collider>().enabled = false;
        selectionMarker = new GameObject("Selection Marker");

        selectionMarker.transform.localScale = selection.transform.localScale;
        selectionMarker.transform.rotation = selection.transform.rotation;

        MeshFilter meshFilter = selectionMarker.AddComponent<MeshFilter>();
        meshFilter.mesh = selection.GetComponent<MeshFilter>().mesh;

        markerRenderer = selectionMarker.AddComponent<MeshRenderer>();
        markerRenderer.material = editModeMat;
    }

    void CancelSelection()
    {
        Destroy(selectionMarker);
        markerOverlapping = false;
        selection.GetComponent<Collider>().enabled = true;
        selection = null;
    }

    void ConfirmSelection()
    {
        selection.transform.position = selectionMarker.transform.position;
        selection.transform.rotation = selectionMarker.transform.rotation;

        CancelSelection();
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            foreach (var hand in player.hands)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(hand.transform.position, hand.transform.position + -hand.transform.up);
            }

            if (selectionMarker)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(selectionMarker.transform.position + selectionMarker.GetComponent<MeshFilter>().mesh.bounds.center, 0.1f);
            }
        }
    }
}
