using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TabletSlider : MonoBehaviour
{
    public float range = 0.7f;
    public RectTransform image;
    public bool swiped;

    Vector3 startPos, fingerPos, nextPos, imageStart;
    bool hovering;

	void Start()
    {
        startPos = transform.localPosition;
        nextPos = startPos;
        imageStart = image.position;
	}
	
	void Update()
    {
        if (hovering)
        {
            nextPos.x = fingerPos.x;
            nextPos.x = Mathf.Clamp(nextPos.x, startPos.x, startPos.x + range);
            transform.localPosition = nextPos;
            Vector3 imagePos = image.position;
            imagePos.x = imageStart.x + nextPos.x * 10;
            imagePos.x += 3.5f;
            image.position = imagePos;
        }
        else
        {
            // return handle to start position
            if (nextPos.x != startPos.x)
            {
                nextPos.x += (startPos.x - nextPos.x) * Time.deltaTime * 5;
                nextPos.x = Mathf.Clamp(nextPos.x, startPos.x, startPos.x + range);
                transform.localPosition = nextPos;
                Vector3 imagePos = image.position;
                imagePos.x = imageStart.x + nextPos.x * 10;
                imagePos.x += 3.5f;
                image.position = imagePos;
            }
        }

        swiped = (Mathf.Abs(startPos.x) + nextPos.x >= range * 0.95f);
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
