using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Roar : MonoBehaviour
{
    //UI
    public Image getreadyBar;
    public Image timeleftBar;
    public GameObject listenandsay;
    public GameObject currentScore;
    public GameObject PlayScreen;
    public GameObject GetReadyScreen;
    public GameObject GameOverScreen;
    public GameObject SpinnerPref;
    public GameObject gameoverscore;
    public GameObject gameovermessage;
    public Animator Flower;
    public bool goPlayTimer = false;
    public bool won = false;
    public bool lost = false;
    public bool spinned = false;
    private int calculatedScore = 200;

    //loudnessControl
    public AudioSource source;
    public float multiplier;
    public AudioLoudnessDetection detector;
    public GameObject volumelevel;
    public GameObject totalvolume;
    public GameObject tooquiet;
    public float loudnessSensibility = 100;
    float loudnessRecord = 0;

    private float duration = 10f;
    private float currentDuration; // The current duration elapsed
    private float initialFillAmount = 1; // The initial fillAmount of timeleftBar

    private float getReadyDuration = 5f;
    private float currentGetReadyDuration; // The current duration elapsed for get ready bar
    private float initialGetReadyFillAmount = 1; // The initial fillAmount of getreadyBar

    private void Start()
    {
        UpdateHighscore(0);
        HealthManager();
    }
    void Update()
    {
        //UI
        if (getreadyBar.fillAmount > 0)
        {
            float deltaAmount = (initialGetReadyFillAmount / getReadyDuration) * Time.deltaTime;
            getreadyBar.fillAmount -= deltaAmount;
            getreadyBar.fillAmount = Mathf.Clamp(getreadyBar.fillAmount, 0f, initialGetReadyFillAmount);
            currentGetReadyDuration += Time.deltaTime;
            Debug.Log("currentGetReadyDuration " + currentGetReadyDuration + "  getReadyDuration " + getReadyDuration);
            if (currentGetReadyDuration >= getReadyDuration)
            {
                Debug.Log("IFFFFFcurrentGetReadyDuration " + currentGetReadyDuration + "  getReadyDuration " + getReadyDuration);
                getreadyBar.fillAmount = 0;
                GetReadyScreen.SetActive(false);
                StartCoroutine(listenTimeout());
            }
        }
        else if (!won && !lost)
        {
            GetReadyScreen.SetActive(false);
            PlayScreen.SetActive(true);

            StartCoroutine(listenTimeout());
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


        //loudness
        float loudness = (detector.GetLoudnessFromMicrophone() * multiplier) / 81;
        if(goPlayTimer)
        {
            totalvolume.SetActive(true);
            volumelevel.SetActive(true);
        }
        if (loudness > loudnessRecord && goPlayTimer && !won && !lost)
        {
            loudnessRecord = loudness;
        }
        if (timeleftBar.fillAmount <= 0 && !won && !lost)
        {
            lost= true;
            StartCoroutine(gameoverScreenTimeout(false));
        }
        if (loudnessRecord >= 0 && !won && !lost)
        {
            loudnessRecord -= 0.0008f;
        }
        if (loudnessRecord > 0.5 && loudnessRecord < 1)
        {
            tooquiet.SetActive(true);

        }
        if(loudnessRecord >= 1 && !spinned && !won && !lost)
        {
            spinned= true;
            won = true;
            StartCoroutine(gameoverScreenTimeout(true));
        }
        
        volumelevel.GetComponent<Image>().fillAmount = loudnessRecord;
       
    }

    public void NextRandomRound()
    {
        SpinnerPref.GetComponent<SpinnerController>().startSpinning();
    }
    private IEnumerator listenTimeout()
    {
        
        yield return new WaitForSeconds(0f);
        if (won  || lost)
        {
            yield break;
        }

        //listenandsay.GetComponent<TMPro.TextMeshProUGUI>().text = "Repeat!";
        goPlayTimer = true;

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
            //score.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
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

    void ReduceHealth()
    {
        PlayerPrefs.SetInt("livesCount", PlayerPrefs.GetInt("livesCount") -1);
        HealthManager();
        if(PlayerPrefs.GetInt("livesCount") <=0)
        {
            GameOver();
        }
    }
    void UpdateHighscore(int scoreAdded)
    {
        currentScore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore") + scoreAdded;
    }
    void GameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
