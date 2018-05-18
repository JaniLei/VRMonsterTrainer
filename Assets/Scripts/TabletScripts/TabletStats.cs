using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TabletStats : MonoBehaviour
{
    public GameObject screen;
    public RectTransform healthBar, hungerBar;
    public GameObject foodWarning, sleepWarning, healthWarning;

    MonsterStats mStats;
    Valve.VR.InteractionSystem.Tablet tablet;
    

	void Start()
    {
        tablet = GetComponent<Valve.VR.InteractionSystem.Tablet>();
        mStats = FindObjectOfType<MonsterStats>();
        if (!mStats)
            Debug.LogWarning("No monster found in scene");
        StartCoroutine("UpdateUIStats");
    }

    IEnumerator UpdateUIStats()
    {
        if (mStats)
        {
            Vector2 healthSize = healthBar.sizeDelta;
            healthSize.x = mStats.health * 10;
            healthBar.sizeDelta = healthSize;
            Vector2 hungerSize = hungerBar.sizeDelta;
            hungerSize.x = (10 - mStats.mStats.hunger) * 10;
            hungerBar.sizeDelta = hungerSize;

            if (mStats.mStats.hunger >= 7 && !foodWarning.activeInHierarchy)
            {
                foodWarning.SetActive(true);
                tablet.PlayWarningSound();
            }
            else
                foodWarning.SetActive(false);

            if (mStats.mStats.fatigue >= 7 && !sleepWarning.activeInHierarchy)
            {
                sleepWarning.SetActive(true);
                tablet.PlayWarningSound();
            }
            else
                sleepWarning.SetActive(false);

            if (mStats.health <= 3 && !healthWarning.activeInHierarchy)
            {
                healthWarning.SetActive(true);
                tablet.PlayWarningSound();
            }
            else
                healthWarning.SetActive(false);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine("UpdateUIStats");
    }
}
