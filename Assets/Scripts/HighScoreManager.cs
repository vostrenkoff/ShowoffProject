using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class HighScoreEntry
{
    public string playerName;
    public int score;
    // Add other fields as per your requirements
}

public class HighScoreManager : MonoBehaviour
{
    public int maxHighScores = 10; // Maximum number of high scores to store
    public string highScoreFileName = "highscores.json"; // Name of the JSON file

    private string filePath; // File path for the JSON file

    private List<HighScoreEntry> highScores; // List to store high scores

    private static HighScoreManager instance;

    public static HighScoreManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            filePath = Path.Combine(Application.persistentDataPath, highScoreFileName);
            LoadHighScores();
        }
        Debug.Log(filePath);
    }

    public void SaveHighScore(string playerName, int score)
    {
        // Create a new entry
        HighScoreEntry newEntry = new HighScoreEntry();
        newEntry.playerName = playerName;
        newEntry.score = score;

        // Add the new entry to the list
        highScores.Add(newEntry);

        // Sort the list based on the score (descending order)
        highScores.Sort((a, b) => b.score.CompareTo(a.score));

        // Keep only the top high scores
        if (highScores.Count > maxHighScores)
        {
            highScores = highScores.GetRange(0, maxHighScores);
        }

        // Save the updated high scores to a JSON file
        string highScoresJson = JsonUtility.ToJson(highScores);
        System.IO.File.WriteAllText(Application.dataPath + "/highscores.json", JsonUtility.ToJson(highScoresJson, true));
        //File.WriteAllText(Application.dataPath + "/highScoresJson.json");
    }
    public void testAddHighscore()
    {
        SaveHighScore("temik", 54);
    }
    public void LoadHighScores()
    {
        if (File.Exists(filePath))
        {
            string highScoresJson = File.ReadAllText(filePath);
            highScores = JsonUtility.FromJson<List<HighScoreEntry>>(highScoresJson);
        }
        else
        {
            highScores = new List<HighScoreEntry>();
        }
    }

    public List<HighScoreEntry> GetHighScores()
    {
        return highScores;
    }
}

