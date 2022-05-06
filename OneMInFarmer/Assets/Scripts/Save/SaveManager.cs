using UnityEngine;

public static class SaveManager
{
    public static void Save(string key, object dataToSave)
    {
        if (key == "" || key == null) return;

        string dataJson = JsonUtility.ToJson(dataToSave);
        PlayerPrefs.SetString(key, dataJson);
    }

    public static object Load(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string objectJson = PlayerPrefs.GetString(key);
            object obj = JsonUtility.FromJson<object>(objectJson);
            PlayerPrefs.DeleteKey(key);
            return obj;
        }
        else
        {
            return null;
        }
    }
}
