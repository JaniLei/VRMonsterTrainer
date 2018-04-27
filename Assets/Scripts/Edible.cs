using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Valve.VR.InteractionSystem
{
    public class Edible : MonoBehaviour
    {
        public Hand holdingHand;
        
        public enum foodType { meat, vegetable, item}
        public foodType type;
        public bool notEdible;
        [HideInInspector] public bool picked = false;


        private void OnAttachedToHand(Hand hand)
        {
            if (type == foodType.vegetable)
            {
                picked = true;
            }

            holdingHand = hand;
        }

        private void OnDetachedFromHand(Hand hand)
        {
            holdingHand = null;
        }
    }
}
