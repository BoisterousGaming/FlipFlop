using UnityEngine;

public static class SaveLoadManager
{
    public static void SaveGame(int score, GridData gridData)
    {
        var data = new SaveData
        {
            score = score,
            gridData = gridData
        };

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Constants.SAVE_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("Game saved: " + json);
    }

    public static SaveData LoadGame()
    {
        if (!PlayerPrefs.HasKey(Constants.SAVE_KEY))
        {
            Debug.Log("No save found, starting fresh.");
            return null;
        }

        string json = PlayerPrefs.GetString(Constants.SAVE_KEY);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        return data;
    }

    public static void ClearSave()
    {
        PlayerPrefs.DeleteKey(Constants.SAVE_KEY);
        PlayerPrefs.Save();

        Debug.Log("Save data cleared.");
    }
}
