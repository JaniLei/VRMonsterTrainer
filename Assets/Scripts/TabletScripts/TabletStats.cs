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
    
	void Start()
    {
        mStats = FindObjectOfType<MonsterStats>();
        if (!mStats)
            Debug.LogWarning("No monster found in scene");
        StartCoroutine("UpdateUIStats");
    }

    //void Update()
    //{
    //    if (screen.activeInHierarchy && 
    //        GetComponent<Valve.VR.InteractionSystem.Tablet>().screenStatus == Valve.VR.InteractionSystem.Tablet.ScreenStatus.Stats)
    //    {
    //        timer += Time.deltaTime;
    //        if (timer >= 1)
    //        {
    //            UpdateUIStats();
    //            timer = 0;
    //        }
    //    }
    //}

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

            if (mStats.mStats.hunger >= 7)
                foodWarning.SetActive(true);
            else
                foodWarning.SetActive(false);

            if (mStats.mStats.fatigue >= 7)
                sleepWarning.SetActive(true);
            else
                sleepWarning.SetActive(false);

            if (mStats.health <= 3)
                healthWarning.SetActive(true);
            else
                healthWarning.SetActive(false);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine("UpdateUIStats");
    }
}
