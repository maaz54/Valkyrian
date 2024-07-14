using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gameplay
{
    public class SaveLoadController : MonoBehaviour
    {
        public void SaveData(LevelData levelData, string fileName)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            string jsonData = JsonUtility.ToJson(levelData);
            File.WriteAllText(filePath, jsonData);
            Debug.Log("Data saved to: " + filePath);
        }

        public LevelData LoadData(string fileName)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                LevelData data = JsonUtility.FromJson<LevelData>(jsonData);
                Debug.Log("Data loaded from: " + filePath);
                return data;
            }
            else
            {
                Debug.LogError("File not found: " + filePath);
                return null;
            }
        }


        [System.Serializable]
        public class LevelData
        {
            public int Turn = 0;
            public int Score = 0;
            public int TotalTargetPair;
            public List<CardData> Cards = new List<CardData>();
        }

        [System.Serializable]
        public class CardData
        {
            public int Index;
            public bool IsRevealed;
        }
    }
}