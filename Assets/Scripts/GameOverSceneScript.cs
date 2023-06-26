using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneScript : MonoBehaviour
{
    public GameObject currentScore;
    private void Start()
    {
        UpdateHighscore(0);
    }
    public void ReturnToMainScreen()
    {
        SceneManager.LoadScene("StartGameScreen");
    }
    void UpdateHighscore(int scoreAdded)
    {
        currentScore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
    }
}
