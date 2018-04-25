using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEditMode : MonoBehaviour
{

    public GameObject selection;
    public Material editModeMat;
    public Material overlappingMat;
    public float lineWidth = 0.01f;

    GameObject selectionMarker;
    LineRenderer lineRenderer;
    Transform lineHolder;
    Valve.VR.InteractionSystem.Player player;
    Vector3 pointedPos;
    GameObject pointedObj;
    bool markerOverlapping;
    
	void Start()
    {
        player = FindObjectOfType<Valve.VR.InteractionSystem.Player>();
	}
	
	void Update()
    {
        foreach (var hand in player.hands)
        {
            RaycastHit hit;
            //bool bHit = Physics.Linecast(hand.transform.position, hand.transform.position + hand.transform.forward * 100, out hit);
            bool bHit = Physics.Linecast(player.hmdTransform.position, hand.transform.position, out hit);
            if (bHit)
            {
                pointedPos = hit.point;

                if (hand.GetStandardInteractionButton())
                {
                    if (!selectionMarker)
                    {
                        pointedObj = hit.transform.gameObject;
                        //CreateLineRenderer(hand.transform.position, pointedPos);
                        CreateLineRenderer(player.hmdTransform.position, pointedPos);
                    }
                }
            }
            
            if (hand.GetStandardInteractionButtonUp())
            {
                if (!selection)
                {
                    if (pointedObj)
                    {
                        selection = pointedObj;
                        CreateSelectionMarker();
                    }

                }
                else
                {
                    if (markerOverlapping)
                        CancelSelection();
                    else
                        ConfirmSelection();
                }

                if (lineRenderer)
                    lineRenderer.enabled = false;
            }

            if (selectionMarker)
            {
                float hAxis = Input.GetAxis("Horizontal");
                float vAxis = Input.GetAxis("Vertical");
                if (Mathf.Abs(hAxis) + Mathf.Abs(vAxis) > 0)
                {
                    RotateSelectionMarker(hAxis, vAxis);
                }

                if (hand.controller != null && hand.controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    Vector2 axis = hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
                    RotateSelectionMarker(axis.x, axis.y);
                    //Vector3 rot = new Vector3(axis.x, 0, axis.y);
                    //Quaternion rotation = Quaternion.LookRotation(rot);
                    //selectionMarker.transform.rotation = rotation;
                }
            }
        }

        if (selectionMarker)
        {
            MoveSelectionMarker(pointedPos);

            Renderer markerRenderer = selectionMarker.GetComponent<Renderer>();
            if (markerOverlapping)
            {
                markerRenderer.material = overlappingMat;
            }
            else
            {
                markerRenderer.material = editModeMat;
            }
        }
    }

    void CreateLineRenderer(Vector3 from, Vector3 to)
    {
        if (lineHolder != null)
        {
            Destroy(lineHolder.gameObject);
        }

        GameObject lineObjectParent = new GameObject("LineObjects");
        lineHolder = lineObjectParent.transform;
        lineHolder.SetParent(this.transform);
        
        GameObject newObject = new GameObject("LineRenderer");
        newObject.transform.SetParent(lineHolder);
        
        lineRenderer = newObject.AddComponent<LineRenderer>();

        lineRenderer.enabled = true;
        lineRenderer.receiveShadows = false;
        lineRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        lineRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.material = editModeMat;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
    }

    void CreateSelectionMarker()
    {
        selection.GetComponent<Collider>().enabled = false;
        selectionMarker = new GameObject("Selection Marker");

        selectionMarker.transform.localScale = selection.transform.localScale;
        selectionMarker.transform.rotation = selection.transform.rotation;

        MeshFilter meshFilter = selectionMarker.AddComponent<MeshFilter>();
        meshFilter.mesh = selection.GetComponent<MeshFilter>().mesh;

        MeshRenderer rend = selectionMarker.AddComponent<MeshRenderer>();
        rend.material = editModeMat;
    }

    void MoveSelectionMarker(Vector3 pos)
    {
        selectionMarker.transform.position = pos;
        MeshRenderer renderer = selectionMarker.GetComponentInChildren<MeshRenderer>();
        if (renderer != null)
        {
            // snap to ground
            RaycastHit rayhit;

            if (Physics.Raycast(/*renderer.bounds.center*/selectionMarker.transform.position, Vector3.down, out rayhit))
            {
                float offsetY = rayhit.point.y - renderer.bounds.min.y;
                selectionMarker.transform.position += new Vector3(0f, offsetY, 0f);
            }
            // check overlaps
            Vector3 offset = new Vector3(0, 0.01f, 0);
            Collider[] colls = Physics.OverlapBox(/*selectionMarker.transform.position*/renderer.bounds.center + offset, /*selectionMarker.transform.localScale / 2*/renderer.bounds.size / 2);
            if (colls.Length > 0)
                markerOverlapping = true;
            else
                markerOverlapping = false;
        }
    }

    void RotateSelectionMarker(float hAxis, float vAxis)
    {
        Vector3 rot = new Vector3(hAxis, 0, vAxis);
        //rot = (rot + player.transform.forward/*<-replace with player body rotation*/).normalized;

        Quaternion rotation = Quaternion.LookRotation(rot, selectionMarker.transform.up);
        //selectionMarker.transform.rotation = /*rotation*/Quaternion.RotateTowards(selectionMarker.transform.rotation, rotation, 1);
        selectionMarker.transform.rotation = rotation;
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
}
