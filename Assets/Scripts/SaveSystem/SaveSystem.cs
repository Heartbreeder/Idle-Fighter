using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string[] ProfileNames
    {
        get
        {
            if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");
            }
            string path = Application.persistentDataPath + "/Saves/";
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] info = dir.GetFiles("*.pro");
            string[] profileNames = new string[info.Length];
            for (int i = 0; i < info.Length; i++)
            {
                profileNames[i] = info[i].Name.Substring(0, info[i].Name.Length - 4);
                //Debug.Log(profileNames[i]);
            }
            return profileNames;
            //Debug.Log("Load Done!");
        }

        set { }
    }

    #region Player Profile saves

    public static void SavePlayerData(PlayerData data, string fileName = "playerData")
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");
        }
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Saves/" + fileName + ".pro";

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerDataContainer container = new PlayerDataContainer(data);

        formatter.Serialize(stream, container);
        stream.Close();

        Debug.Log("Save System: Player Profile saved.");
    }

    public static PlayerDataContainer LoadPlayerData(string fileName = "playerData")
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");
        }
        string path = Application.persistentDataPath + "/Saves/" + fileName + ".pro";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerDataContainer data = formatter.Deserialize(stream) as PlayerDataContainer;

            stream.Close();

            //PlayerData dat = new PlayerData(data);

            Debug.Log("Save System: Player profile loaded.");

            return data;
        }
        else
        {
            Debug.Log("Save System: Last played profile is Null or Empty; No Player Profile loaded.");
            //Debug.Log("Profile file not found.");
            return null;
        }
    }

    public static bool DeletePlayerData(string fileName)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");
        }
        string path = Application.persistentDataPath + "/Saves/" + fileName + ".pro";
        if (File.Exists(path))
        {
            File.Delete(path);
            return true;
        }
        return false;
    }

    public static void LoadAllPlayerNames()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");
        }
        string path = Application.persistentDataPath + "/Saves/";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.pro");
        string[] profileNames = new string[info.Length];
        for (int i = 0; i < info.Length; i++)
        {
            profileNames[i] = info[i].Name.Substring(0, info[i].Name.Length - 4);
            //Debug.Log(profileNames[i]);
        }
        ProfileNames = profileNames;
        //Debug.Log("Load Done!");
    }

    #endregion

    /*
    #region Options saves
     
    public static void SaveOptions(OptionsData data)
    {
        string fileName = "Options.opt";
        if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");
        }

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Saves/" + fileName;

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        FileStream stream = new FileStream(path, FileMode.Create);

        OptionsDataContainer container = new OptionsDataContainer(data);

        formatter.Serialize(stream, container);
        stream.Close();
    }

    public static OptionsDataContainer LoadOptions()
    {
        string fileName = "Options.opt";
        if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");
        }

        string path = Application.persistentDataPath + "/Saves/" + fileName;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            OptionsDataContainer data = formatter.Deserialize(stream) as OptionsDataContainer;

            stream.Close();
            if (data == null) Debug.Log("data is null");
            
            //OptionsData dat = new OptionsData(data);
            //if (dat == null) Debug.Log("dat is null");
            
            return data;
        }
        else
        {
            Debug.Log("Options file not found.");
            return null;
        }
    }
    #endregion

    */

}
