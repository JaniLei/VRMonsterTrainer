using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class DetectObject : MonoBehaviour
    {

        public Monster monster;

        void OnTriggerStay(Collider col)
        {

            Edible edible = col.GetComponent<Edible>();
            if (edible && edible.holdingHand == null)
            {
                monster.EatObject(col.gameObject);
            }
            
        }
    }
}