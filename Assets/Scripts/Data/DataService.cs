using System.IO;
using UnityEngine;

public class DataService : MonoBehaviour {
    
    //==============================================================================================//
    //==============================================================================================//

    public SaveData Data => _saveData ??= LoadData();

    string _saveFilePath;
    SaveData _saveData;

    //==============================================================================================//
    //==============================================================================================//
    
    public static DataService Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _saveFilePath = Application.persistentDataPath + "/data.json";
    }

    //==============================================================================================//
    //==============================================================================================//
    
    public void Save() {
        string jsonText = JsonUtility.ToJson(Data);
        File.WriteAllText(_saveFilePath, jsonText);
    }

    SaveData LoadData() {
        if (File.Exists(_saveFilePath)) {
            string jsonText = File.ReadAllText(_saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(jsonText);
            if (data != null) return data;
        }
        return new SaveData();
    }

    //==============================================================================================//
    //==============================================================================================//

    void OnApplicationPause(bool pause) {
        if (pause) Save();
    }

    void OnApplicationQuit() {
        Save();
    }
    
    //==============================================================================================//
    //==============================================================================================//
}
