using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreUI : MonoBehaviour
{
    public Text highScoreText;
    public HighScoreManager highScoreManager;

    private void Start()
    {
        // Update the high score text on start
        UpdateHighScoreText();
    }

    public void UpdateHighScoreText()
    {
        // Load the high scores
        List<HighScoreEntry> highScores = highScoreManager.GetHighScores();

        // Clear the previous text
        highScoreText.text = string.Empty;

        // Iterate through the high scores and update the text
        for (int i = 0; i < highScores.Count; i++)
        {
            HighScoreEntry entry = highScores[i];
            highScoreText.text += $"{i + 1}. {entry.playerName} - {entry.score}\n";
        }
    }
}
