using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class EditModeButton : MonoBehaviour
    {
        public RoomEditMode editMode;
        public Text screenText;

        bool buttonPressed;
        //Vector3 startPos;

        void Start()
        {
            //startPos = transform.localPosition;
            screenText.text = ("Edit mode off.");
        }

        private void OnHandHoverBegin(Hand hand)
        {
            editMode.enabled = !editMode.enabled;
            buttonPressed = !buttonPressed;

            if (buttonPressed)
            {
                //transform.localPosition += new Vector3(0, 0, 0.01f);
                GetComponent<SpriteRenderer>().color = Color.cyan;
                screenText.text = ("Edit mode on.");
            }
            else
            {
                //transform.localPosition = startPos;
                GetComponent<SpriteRenderer>().color = Color.white;
                screenText.text = ("Edit mode off.");
            }
        }
    }
}
