using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Throwable))]
    public class Edible : MonoBehaviour
    {
        public Hand holdingHand;
        
        public enum foodType { meat, vegetable, item}
        public foodType type;
        public bool notEdible;

        private void OnAttachedToHand(Hand hand)
        {
            holdingHand = hand;
        }

        private void OnDetachedFromHand(Hand hand)
        {
            holdingHand = null;
        }
    }
}
