using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using UnityEngine;
using System.Xml;
/// <summary>
/// Created using Brackeys 'Save & Load System in Unity' https://www.youtube.com/watch?v=XOjd_qU2Ido
/// </summary>
public class SaveSystem
{
    /// <summary>
    /// Saves the player hotbar.
    /// </summary>
    /// <param name="playerHotbar"></param>
    public static void SavePlayer(PlayerHotbar playerHotbar)
    {
        string path = Application.persistentDataPath;

        FileStream file = File.Create(path + "/player.hotbar");

        PlayerHotbarData data = new PlayerHotbarData();

        data.hotbarItems = playerHotbar.m_hotBarItems;

        DataContractSerializer bf = new DataContractSerializer(data.GetType());
        MemoryStream streamer = new MemoryStream();

        bf.WriteObject(streamer, data);

        streamer.Seek(0, SeekOrigin.Begin);

        file.Write(streamer.GetBuffer(), 0, streamer.GetBuffer().Length);

        file.Close();
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
    /// <summary>
    /// Loads the players stats.
    /// </summary>
    /// <returns></returns>
    public static PlayerData LoadPlayerStats()
    {
        string path = Application.persistentDataPath + "/player.stats";
        Debug.Log(path);

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
            SavePlayer(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>());
            Debug.LogError("Save file not found in: " + path);
            return null;
        }
    }
    /// <summary>
    /// Loads the player hotbar.
    /// </summary>
    /// <returns></returns>
    public static PlayerHotbar LoadPlayerHotbar()
    {
        string path = Application.persistentDataPath + "/player.hotbar";
        PlayerHotbarData data = new PlayerHotbarData();
        if (File.Exists(path))
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            DataContractSerializer ser = new DataContractSerializer(typeof(PlayerHotbarData));

            PlayerHotbar phb = (PlayerHotbar)ser.ReadObject(reader, true);
            reader.Close();
            fs.Close();
            Debug.Log("Debug: " + phb.m_hotBarItems);
            return phb;
        }
        else
        {
            return null;
        }
      
    }

   /// <summary>
   /// A class storing the players hotbar.
   /// </summary>
    [DataContract(Namespace ="")] 
    public class PlayerHotbarData
    {
        [DataMember]
        public GameObject[] hotbarItems;

    }
}
