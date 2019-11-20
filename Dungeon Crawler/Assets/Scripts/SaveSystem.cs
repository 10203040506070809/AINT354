using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Created using Brackeys 'Save & Load System in Unity' https://www.youtube.com/watch?v=XOjd_qU2Ido
/// </summary>
public class SaveSystem
{

    public static void SavePlayer(PlayerHotbar playerHotbar)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.hotbar";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(playerHotbar);

        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    /// <summary>
    /// Saves playerStats to a binary file, player.stats.
    /// </summary>
    /// <param name="playerStats"></param>
    public static void SavePlayer(PlayerStats playerStats)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.stats";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(playerStats);

        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayerStats()
    {
        string path = Application.persistentDataPath + "/player.stats";


        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

            PlayerData data = binaryFormatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in: " + path);
            return null;
        }
    }

    public static PlayerData LoadPlayerHotbar()
    {
        {
            string path = Application.persistentDataPath + "/player.hotbar";


            if (File.Exists(path))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

                if (stream.Length != 0)
                {
                    PlayerData data = binaryFormatter.Deserialize(stream) as PlayerData;
                    
                    stream.Close();

                    return data;
                }
                else
                {
                    stream.Close();
                    return null;
                }
               
            }
            else
            {
                Debug.LogError("Save file not found in: " + path);
                return null;
            }
        }
    }
}
