using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DestroyerGame : MonoBehaviour
{
    //UI
    public Image getreadyBar;
    public Image timeleftBar;
    public GameObject listenandsay;
    public GameObject PlayScreen;
    public GameObject GetReadyScreen;
    public GameObject SpinnerPref;
    public bool goPlayTimer = false;
    public bool won = false;
    public bool lost = false;
    public bool spinned = false;


    //score
    public int calculatedScore = 0;
    public GameObject score;
    public GameObject totalscore;
    private void Start()
    {
        HealthManager();
        totalscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");


    }
    void Update()
    {
        
        //calculatedScore = Mathf.RoundToInt(playerpref.transform.position.x * 10);
        //score.GetComponent<TMPro.TextMeshProUGUI>().text = "" + calculatedScore;
        //UI
        if (getreadyBar.fillAmount > 0)
        {
            getreadyBar.fillAmount -= 0.002f;
        }
        else if (!won)
        {
            GetReadyScreen.SetActive(false);
            //PlayScreen.SetActive(true);
            StartCoroutine(timeout());
        }
        if (timeleftBar.fillAmount <= 0 && !won && !lost)
        {
            lost = true;
            StartCoroutine(loseScreenTimeout());
        }
        if (timeleftBar.fillAmount > 0 && goPlayTimer)
        {
            if (!won)
                timeleftBar.fillAmount -= 0.00028f;
            else
                timeleftBar.fillAmount = 1;
        }

        if (won && !spinned)
        {
            spinned = true;
            won = true;
            StartCoroutine(winScreenTimeout());
        }
    }






    public void NextRandomRound()
    {
        SpinnerPref.GetComponent<SpinnerController>().startSpinning();
    }
    private IEnumerator timeout()
    {
        goPlayTimer = true;
        yield return new WaitForSeconds(3f);
        if (won)
        {
            yield break;
        }
        yield break;
    }
    private IEnumerator winScreenTimeout()
    {

        PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("highscore") + 100);
        totalscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
        won = true;
        yield return new WaitForSeconds(2f);
        NextRandomRound();

        yield break;
    }
    private IEnumerator loseScreenTimeout()
    {
        ReduceHealth();
        UpdateHighscore(0);
        listenandsay.GetComponent<TMPro.TextMeshProUGUI>().text = "Better luck next time.";
        //tooquiet.SetActive(false);


        yield return new WaitForSeconds(2f);
        NextRandomRound();
        yield break;
    }
    void ReduceHealth()
    {
        PlayerPrefs.SetInt("livesCount", PlayerPrefs.GetInt("livesCount") - 1);
        //HealthManager();
        if (PlayerPrefs.GetInt("livesCount") <= 0)
        {
            GameOver();
        }
    }
    void UpdateHighscore(int scoreAdded)
    {
        score.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore") + scoreAdded;
    }
    void GameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    void HealthManager()
    {
        GameObject live1 = GameObject.Find("Life1");
        GameObject live2 = GameObject.Find("Life2");
        GameObject live3 = GameObject.Find("Life3");

        if (PlayerPrefs.GetInt("livesCount") == 3)
        {
            if (live1 != null) live1.SetActive(true);
            if (live2 != null) live2.SetActive(true);
            if (live3 != null) live3.SetActive(true);
        }
        if (PlayerPrefs.GetInt("livesCount") == 2)
        {
            if (live1 != null) live1.SetActive(true);
            if (live2 != null) live2.SetActive(true);
            if (live3 != null) live3.SetActive(false);
        }
        if (PlayerPrefs.GetInt("livesCount") == 1)
        {
            if (live1 != null) live1.SetActive(true);
            if (live2 != null) live2.SetActive(false);
            if (live3 != null) live3.SetActive(false);
        }
        if (PlayerPrefs.GetInt("livesCount") <= 0)
        {
            if (live1 != null) live1.SetActive(false);
            if (live2 != null) live2.SetActive(false);
            if (live3 != null) live3.SetActive(false);
        }
    }

}
