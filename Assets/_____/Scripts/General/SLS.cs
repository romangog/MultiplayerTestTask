using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

public class SLS
{
    public static int GetInt(string key, int defaultVal = 0) => PlayerPrefs.GetInt(key, defaultVal);

    public static List<int> GetIntCollection(string key, List<int> defaultValues = null)
    {
        string str = PlayerPrefs.GetString(key);
        if (str == "")
            return defaultValues;
        else
            return str.Split('&').Select(t => int.Parse(t)).ToList();

    }

    public static List<bool> GetBoolCollection(string key, List<bool> defaultValues = null)
    {
        string str = PlayerPrefs.GetString(key);
        if (str == "")
            return defaultValues;
        else
            return str.Split('&').Select(t => int.Parse(t) == 1).ToList();
    }

    public static void SetInt(string key, int value) => PlayerPrefs.SetInt(key, value);

    public static void SetIntCollection(string key, List<int> values)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < values.Count; i++)
        {
            sb.Append(values[i].ToString());
            if (i != values.Count - 1)
                sb.Append('&');
        }

        PlayerPrefs.SetString(key, sb.ToString());
    }

    public static void SetBoolCollection(string key, List<bool> values)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < values.Count; i++)
        {
            sb.Append(values[i] ? '1' : '0');
            if (i != values.Count - 1)
                sb.Append('&');
        }

        PlayerPrefs.SetString(key, sb.ToString());
    }

    public static void SetObject(object obj, string key)
    {
        File.WriteAllText(Application.persistentDataPath + "/" + key + ".json", JsonUtility.ToJson(obj));
    }

    public static T GetObject<T>(string key, T defaultObject) where T : class
    {
        string path = Application.persistentDataPath + "/" + key + ".json";
        if (File.Exists(path)) return JsonUtility.FromJson<T>(File.ReadAllText(path));
        else
            return defaultObject;
    }

    public static void SetObjectsCollection<T>(List<T> objectsCollection, string key)
    {
        for (int i = 0; i < objectsCollection.Count; i++)
        {
            SetObject(objectsCollection[i], key + i);
        }
    }

    public static List<T> GetObjectsCollection<T>(string key, int length, T defaultElement) where T : class
    {
        List<T> objectsCollection = new List<T>();
        for (int i = 0; i < length; i++)
        {
            objectsCollection.Add(GetObject<T>(key + i, defaultElement));
        }
        return objectsCollection;
    }


    public class Keys
    {
        public class Progress
        {
            public const string LEVEL_CURRENT_INT = "LEVEL_CURRENT";
            public const string MONEY_INT = "MONEY";
        }
    }

    public class Snapshot
    {
        private int _currentLevel;
        public int CurrentLevel
        {
            get => _currentLevel;
            set
            {
                SLS.SetInt(SLS.Keys.Progress.LEVEL_CURRENT_INT, value);
                _currentLevel = value;
            }
        }

        private int _money;
        public int Money
        {
            get => _money;
            set
            {
                SLS.SetInt(SLS.Keys.Progress.MONEY_INT, value);
                _money = value;
            }
        }



        public Snapshot(GameModSettings gmodSettings)
        {
            _currentLevel = SLS.GetInt(SLS.Keys.Progress.LEVEL_CURRENT_INT);
            _money = SLS.GetInt(SLS.Keys.Progress.MONEY_INT);

            if (gmodSettings.ClearSaves)
            {
                PlayerPrefs.DeleteAll();
            }
        }
    }
}
