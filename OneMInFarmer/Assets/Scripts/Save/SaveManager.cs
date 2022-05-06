using UnityEngine;

[System.Serializable]
public static class SaveManager
{
    public static void Save(string key, object dataToSave)
    {
        if (key == "" || key == null) return;

        string dataJson = JsonUtility.ToJson(dataToSave);
        PlayerPrefs.SetString(key, dataJson);
    }

    public static string Load(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string objectJson = PlayerPrefs.GetString(key);
            PlayerPrefs.DeleteKey(key);
            return objectJson;
        }
        else
        {
            return "";
        }
    }
}
