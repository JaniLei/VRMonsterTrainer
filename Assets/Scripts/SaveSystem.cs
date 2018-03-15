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
                Debug.LogWarning("Failed to get save system instance. Add a save system to the scene.");

            return _instance;
        }
    }

    float _timePlayed;
    public float timePlayed
    {
        get { return _timePlayed + Time.time; }
    }

    void Awake()
    {
        // singleton instance check
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        
        if (!Load())
            Save();
    }

    void OnApplicationQuit()
    {
        Save();
        Debug.Log("Time played: " + timePlayed + "s");
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveData.dat");
        SaveData data = new SaveData();

        var player = FindObjectOfType<Valve.VR.InteractionSystem.Player>();
        data.playerPos = new Position(player.transform.position.x, player.transform.position.y, player.transform.position.z);

        var objs = FindObjectsOfType<Valve.VR.InteractionSystem.Interactable>();
        data.objPositions = new Position[objs.Length];
        data.objRotations = new Position[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            data.objPositions[i] = new Position(objs[i].transform.position.x, objs[i].transform.position.y, objs[i].transform.position.z);
            data.objRotations[i] = new Position(objs[i].transform.rotation.x, objs[i].transform.rotation.y, objs[i].transform.rotation.z, objs[i].transform.rotation.w);
        }

        var monster = FindObjectOfType<Monster>();
        if (monster)
        {
            //data.monsterPos = monster.transform;
            data.monsterStats = monster.GetComponent<MonsterStats>();
        }

        //data.timePlayed += Time.time;
        data.timePlayed = timePlayed;

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
            player.transform.position = new Vector3(data.playerPos.x, data.playerPos.y, data.playerPos.z);

            var objs = FindObjectsOfType<Valve.VR.InteractionSystem.Interactable>();
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].transform.position = new Vector3(data.objPositions[i].x, data.objPositions[i].y, data.objPositions[i].z);
                objs[i].transform.rotation = new Quaternion(data.objRotations[i].x, data.objRotations[i].y, data.objRotations[i].z, data.objRotations[i].w);
            }

            var monster = FindObjectOfType<Monster>();
            if (monster)
            {
                //monster.transform = data.monsterPos;
                monster.GetComponent<MonsterStats>().mStats = data.monsterStats.mStats;
                monster.GetComponent<MonsterStats>().health = data.monsterStats.health;
            }

            _timePlayed = data.timePlayed;

            return true;
        }
        else
        {
            Debug.LogError("Cannot load from file \"" + fileName + "\", as it does not exist.");
            return false;
        }
    }
}

/* used for serializing Vector3 and Quaternion */
[System.Serializable]
public struct Position
{
    public float x, y, z, w;
    public Position(float _x, float _y, float _z, float _w = 0)
    {
        x = _x; y = _y; z = _z; w = _w;
    }
}

[System.Serializable]
public class SaveData
{
    public Position playerPos;
    public Position[] objPositions;
    public Position[] objRotations;
    //public Transform monsterPos;
    public MonsterStats monsterStats;
    public float timePlayed;
}
