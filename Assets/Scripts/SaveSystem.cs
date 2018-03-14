using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    private static SaveSystem _instance;
    public static SaveSystem instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogWarning("Failed to get save system instance. Add a save system to the scene.");
            }
            return _instance;
        }
    }


    void Awake()
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

        //Save();
        if (!Load())
            Save();
    }

    void OnApplicationQuit()
    {
        Save();
    }
    
	public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveData.dat");

        SaveData data = new SaveData();
        var player = FindObjectOfType<Valve.VR.InteractionSystem.Player>();
        data.playerX = player.transform.position.x;
        data.playerY = player.transform.position.y;
        data.playerZ = player.transform.position.z;
        //data.monsterPos = monster.transform;
        //data.stats = monster.stats;
        //data.timePlayed += Time.time;

        bf.Serialize(file, data);
        file.Close();
    }

    public bool Load()
    {
        string fileName = Application.persistentDataPath + "/saveData.dat";
        if (File.Exists(fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            var player = FindObjectOfType<Valve.VR.InteractionSystem.Player>();
            player.transform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
            //var monster = FindObjectOfType<Monster>();
            //monster.transform = data.monsterPos;
            //monster.stats = data.stats;
            return true;
        }
        else
        {
            Debug.LogError("Cannot load from file \"" + fileName + "\", as it does not exist.");
            return false;
        }
    }
}

[System.Serializable]
public class SaveData
{
    public float playerX, playerY, playerZ;
    //public Vector3 playerPos;
    //public Transform monsterPos;
    //public MonsterStats stats;
    //other object transforms?
    //public float timePlayed;
}
