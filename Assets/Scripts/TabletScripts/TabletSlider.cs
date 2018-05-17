using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletSlider : MonoBehaviour
{
    public float range = 0.7f;

    Vector3 startPos, fingerPos, nextPos;
    bool hovering;

	void Start()
    {
        startPos = transform.localPosition;
        nextPos = startPos;
	}
	
	void Update()
    {
        if (hovering)
        {
            nextPos.x = fingerPos.x;
            nextPos.x = Mathf.Clamp(nextPos.x, startPos.x, startPos.x + range);
            transform.localPosition = nextPos;
        }
        else
        {
            // return handle to start position
            if (nextPos.x != startPos.x)
            {
                nextPos.x += (startPos.x - nextPos.x) * Time.deltaTime * 5;
                nextPos.x = Mathf.Clamp(nextPos.x, startPos.x, startPos.x + range);
                transform.localPosition = nextPos;
            }
        }

        if (Mathf.Abs(startPos.x) + nextPos.x >= range * 0.9f)
        {
            OnSwiped();
        }
	}

    void OnSwiped()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Finger")
        {
            fingerPos = transform.parent.InverseTransformPoint(other.transform.position);
            hovering = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Finger")
            hovering = false;
    }
}
