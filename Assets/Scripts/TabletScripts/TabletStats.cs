using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TabletStats : MonoBehaviour
{
    public RectTransform healthBar, hungerBar;

    MonsterStats mStats;
    float timer;
    
	void Start()
    {
        mStats = FindObjectOfType<MonsterStats>();
        if (mStats)
            Debug.LogWarning("No monster found in scene");
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            UpdateUIStats();
            timer = 0;
        }
    }

    void UpdateUIStats()
    {
        if (mStats)
        {
            Vector2 healthSize = healthBar.sizeDelta;
            healthSize.x = mStats.health * 10;
            healthBar.sizeDelta = healthSize;
            Vector2 hungerSize = hungerBar.sizeDelta;
            hungerSize.x = mStats.mStats.hunger * 10;
            hungerBar.sizeDelta = hungerSize;
        }
    }
}
