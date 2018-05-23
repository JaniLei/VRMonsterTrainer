using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EventManager : MonoBehaviour
{
    private static EventManager _instance;
    public static EventManager instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogWarning("Failed to get event manager instance. Add an event manager to the scene.");
            }
            return _instance;
        }
    }

    public delegate void StateEventHandler();
    public event StateEventHandler Pointing, Fetching, MonsterDeath, Victory;
    public GameObject targetObj;
    public bool monsterDead;
    public bool victory;


    void Awake ()
    {
        if (_instance == null)
        {
            // if not, set instance to this
            _instance = this;
        }
        // if instance already exists and it's not this
        else if (_instance != this)
        {
            // destroy this, there can only be one instance
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (victory)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }
    }
	
	public void OnPointing()
    {
        Pointing();
    }

    public void OnFetching()
    {
        Fetching();
    }

    public void OnMonsterDeath()
    {
        Debug.Log("Monster died");
        MonsterDeath();
        monsterDead = true;
    }

    public void OnVictory()
    {
        Victory();
        victory = true;
    }

    public void RestartGame()
    {
        if (SaveSystem.instance)
            SaveSystem.instance.DeleteSave();
        SceneManager.LoadScene("cave", LoadSceneMode.Single);
    }
}
