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
            if (monster)
            {
                Edible edible = col.GetComponent<Edible>();
            if (!edible)
                edible = col.GetComponentInChildren<Edible>();
            if (!edible)
                edible = col.GetComponentInParent<Edible>();

                if (edible && edible.holdingHand == null)
                {
                    monster.EatObject(col.gameObject);
                }
                else if (edible)
                {
                    monster.SniffObject(col.gameObject);
                }
            }
            else
            {
                Destroy(this);
            }

            
        }
    }
}