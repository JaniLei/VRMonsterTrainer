using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TabletSlider : MonoBehaviour
{
    public enum ConfirmingType
    {
        ConfirmQuit,
        ConfirmDeathRestart,
        ConfirmVictoryRestart
    }
    public ConfirmingType confirmingType;
    public float range = 0.7f;
    public RectTransform image;
    public bool swiped;
    public Text description;

    Vector3 startPos, fingerPos, nextPos, imageStart;
    bool hovering;
    bool quit;

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

        if (swiped)
        {
            if (confirmingType == ConfirmingType.ConfirmQuit)
            {
                if (!quit)
                {
                    Debug.Log("Quit game.");
                    Application.Quit();
                    quit = true;
                }
            }
            else if (confirmingType == ConfirmingType.ConfirmDeathRestart)
            {
                EventManager.instance.RestartGame();
            }
        }
	}

    public void UpdateDescription()
    {
        if (confirmingType == ConfirmingType.ConfirmQuit)
            description.text = "Quitting game.\nSwipe to confirm.";
        else if (confirmingType == ConfirmingType.ConfirmDeathRestart)
            description.text = "Monster dead.\nSwipe to restart.";
        else if (confirmingType == ConfirmingType.ConfirmVictoryRestart)
            description.text = "Victory! The monster fully evolved.\nSwipe to restart or 'esc' to Quit";
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
