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
    public GameObject GameOverScreen;
    public GameObject SpinnerPref;
    public GameObject gameoverscore;
    public GameObject gameovermessage;
    public bool goPlayTimer = false;
    public bool won = false;
    public bool lost = false;
    public bool spinned = false;


    //score
    private int calculatedScore = 0;
    public GameObject score;
    public GameObject totalscore;

    private float duration = 25f;
    private float currentDuration; // The current duration elapsed
    private float initialFillAmount=1; // The initial fillAmount of timeleftBar

    private float getReadyDuration = 5f;
    private float currentGetReadyDuration; // The current duration elapsed for get ready bar
    private float initialGetReadyFillAmount=1; // The initial fillAmount of getreadyBar

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
            float deltaAmount = (initialGetReadyFillAmount / getReadyDuration) * Time.deltaTime;
            getreadyBar.fillAmount -= deltaAmount;
            getreadyBar.fillAmount = Mathf.Clamp(getreadyBar.fillAmount, 0f, initialGetReadyFillAmount);
            currentGetReadyDuration += Time.deltaTime;

            if (currentGetReadyDuration >= getReadyDuration)
            {
                getreadyBar.fillAmount = 0;
                GetReadyScreen.SetActive(false);
                StartCoroutine(timeout());
            }
        }
        else if (!won)
        {
            GetReadyScreen.SetActive(false);
            StartCoroutine(timeout());
        }
        if (timeleftBar.fillAmount <= 0 && !won && !lost)
        {
            lost = true;
            StartCoroutine(gameoverScreenTimeout(false));
        }
        if (timeleftBar.fillAmount > 0 && goPlayTimer)
        {
            if (!won)
            {
                float deltaAmount = (initialFillAmount / duration) * Time.deltaTime;
                timeleftBar.fillAmount -= deltaAmount;
                timeleftBar.fillAmount = Mathf.Clamp(timeleftBar.fillAmount, 0f, initialFillAmount);
                currentDuration += Time.deltaTime;

                if (currentDuration >= duration)
                {
                    timeleftBar.fillAmount = 0;
                    if (timeleftBar.fillAmount <= 0 && !won && !lost)
                    {
                        lost = true;
                        StartCoroutine(gameoverScreenTimeout(false));
                    }
                }
            }
            else
            {
                timeleftBar.fillAmount = 1;
            }
        }

        if (won && !spinned)
        {
            spinned = true;
            won = true;
            StartCoroutine(gameoverScreenTimeout(true));
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
    /*private IEnumerator winScreenTimeout()
    {

        PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("highscore") + 100);
        totalscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
        won = true;
        yield return new WaitForSeconds(2f);
        NextRandomRound();

        yield break;
    }*/
    /*private IEnumerator loseScreenTimeout()
    {
        ReduceHealth();
        UpdateHighscore(0);
        yield return new WaitForSeconds(2f);
        NextRandomRound();
        yield break;
    }*/
    private IEnumerator gameoverScreenTimeout(bool _won)
    {
        GameOverScreen.SetActive(true);
        HealthManager();
        if (_won)
        {
            won = true;
            PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("highscore") + calculatedScore);
            totalscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
            gameovermessage.GetComponent<TMPro.TextMeshProUGUI>().text = "Level completed +" + calculatedScore;
        }
        else
        {
            gameovermessage.GetComponent<TMPro.TextMeshProUGUI>().text = "Better luck next time";

            UpdateHighscore(0);
        }
        gameoverscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
        UpdateHighscore(0);
        yield return new WaitForSeconds(4f);
        if (!_won)
            ReduceHealth();
        else
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
        GameObject[] live1 = GameObject.FindGameObjectsWithTag("Life1");
        GameObject[] live2 = GameObject.FindGameObjectsWithTag("Life2");
        GameObject[] live3 = GameObject.FindGameObjectsWithTag("Life3");

        if (PlayerPrefs.GetInt("livesCount") == 3)
        {
            foreach (GameObject go in live1) { go.SetActive(true); }
            foreach (GameObject go in live2) { go.SetActive(true); }
            foreach (GameObject go in live3) { go.SetActive(true); }
        }
        if (PlayerPrefs.GetInt("livesCount") == 2)
        {
            foreach (GameObject go in live1) { go.SetActive(true); }
            foreach (GameObject go in live2) { go.SetActive(true); }
            foreach (GameObject go in live3) { go.SetActive(false); }
        }
        if (PlayerPrefs.GetInt("livesCount") == 1)
        {
            foreach (GameObject go in live1) { go.SetActive(true); }
            foreach (GameObject go in live2) { go.SetActive(false); }
            foreach (GameObject go in live3) { go.SetActive(false); }
        }
        if (PlayerPrefs.GetInt("livesCount") <= 0)
        {

            foreach (GameObject go in live1) { go.SetActive(false); }
            foreach (GameObject go in live2) { go.SetActive(false); }
            foreach (GameObject go in live3) { go.SetActive(false); }
        }
    }

}
