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
    public GameObject SpinnerPref;
    public Animator Flower;
    public bool goPlayTimer = false;
    public bool won = false;
    public bool lost = false;
    public bool spinned = false;

    //loudnessControl
    public AudioSource source;
    public float multiplier;
    public AudioLoudnessDetection detector;
    public GameObject volumelevel;
    public GameObject totalvolume;
    public GameObject tooquiet;
    public float loudnessSensibility = 100;
    float loudnessRecord = 0;
    private void Start()
    {
        UpdateHighscore(0);
        HealthManager();
    }
    void Update()
    {
        //UI
        if(getreadyBar.fillAmount > 0)
        {
            getreadyBar.fillAmount -= 0.002f;
        }
        else if(!won && !lost)
        {
            GetReadyScreen.SetActive(false);
            PlayScreen.SetActive(true);
            
            StartCoroutine(listenTimeout());
        }
        if (timeleftBar.fillAmount > 0 && goPlayTimer)
        {
            if (!won && !lost)
                timeleftBar.fillAmount -= 0.0008f;
            else
                timeleftBar.fillAmount = 1;
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
            StartCoroutine(loseScreenTimeout());
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
            StartCoroutine(winScreenTimeout());
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
    private IEnumerator winScreenTimeout()
    {
        UpdateHighscore(100);
        listenandsay.GetComponent<TMPro.TextMeshProUGUI>().text = "Well done.";
        won = true;
        tooquiet.SetActive(false);
        Flower.SetBool("wokeup", true);

        yield return new WaitForSeconds(5f);
        NextRandomRound();
        yield break;
    }
    private IEnumerator loseScreenTimeout()
    {
        ReduceHealth();
        UpdateHighscore(0);
        listenandsay.GetComponent<TMPro.TextMeshProUGUI>().text = "Better luck next time.";
        tooquiet.SetActive(false);


        yield return new WaitForSeconds(2f);
        NextRandomRound();
        yield break;
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
