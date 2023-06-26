using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClothesGame : MonoBehaviour
{
    //UI
    public Image getreadyBar;
    public Image timeleftBar;
    public GameObject listenandsay;
    public GameObject gj;
    public GameObject PlayScreen;
    public GameObject GetReadyScreen;
    public GameObject GameOverScreen;
    public GameObject WinSound;
    public GameObject LooseSound;
    public GameObject SpinnerPref;
    public bool goPlayTimer = false;
    public bool won = false;
    public bool lost = false;
    public bool spinned = false;
    public GameObject gameoverscore;
    public GameObject gameovermessage;
    

    public GameObject[] Hats;
    public GameObject[] Tops;
    public GameObject[] Boots;

    public GameObject WhiteMonsterMaket;

    public GameObject attachedHat;
    public GameObject attachedTop;
    public GameObject attachedBoots;
    
    GameObject randomHat;
    GameObject randomTop;
    GameObject randomBoots;


    private bool hatIsTrue = false;
    private bool topIsTrue = false;
    private bool bootsIsTrue = false;
    //score
    private int calculatedScore = 200;
    public GameObject score;
    public GameObject totalscore;

    private float duration = 10;
    private float currentDuration; // The current duration elapsed
    private float initialFillAmount = 1; // The initial fillAmount of timeleftBar

    private float getReadyDuration = 5f;
    private float currentGetReadyDuration; // The current duration elapsed for get ready bar
    private float initialGetReadyFillAmount = 1; // The initial fillAmount of getreadyBar

    private void Start()
    {
        HealthManager();
        totalscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");

        duration = duration * PlayerPrefs.GetFloat("multiplier");
        Debug.LogError("Duration " + duration);

        currentGetReadyDuration = 0f;
        initialGetReadyFillAmount = getreadyBar.fillAmount;

        currentDuration = 0f;
        initialFillAmount = timeleftBar.fillAmount;

        randomHat = Hats[Random.Range(0,Hats.Count()-1)];
        GameObject hatImg = GameObject.Find(randomHat.name+"Img");
        hatImg.GetComponent<Image>().enabled = true;
        
        randomTop = Tops[Random.Range(0,Tops.Count()-1)];
        GameObject topImg = GameObject.Find(randomTop.name + "Img");
        topImg.GetComponent<Image>().enabled = true;

        randomBoots = Boots[Random.Range(0,Boots.Count()-1)];
        GameObject bootsImg = GameObject.Find(randomBoots.name + "Img");
        bootsImg.GetComponent<Image>().enabled = true;

        

    }
    void Update()
    {

        

        if (attachedBoots != null && randomBoots != null)
        {
            if (attachedBoots.ToString() == randomBoots.ToString())
            {
                bootsIsTrue = true;
            }
            else
                bootsIsTrue = false;
              
        }
        if (attachedTop != null && randomTop != null)
        {
            if (attachedTop.ToString() == randomTop.ToString())
            {
                topIsTrue= true;
            }
            else
                topIsTrue= false;
        }
        if (attachedHat != null && randomHat != null)
        {
            if (attachedHat.ToString() == randomHat.ToString())
            {
                hatIsTrue = true;
            }
            else
                hatIsTrue = false;
        }
        //calculatedScore = Mathf.RoundToInt(playerpref.transform.position.x * 10);
        score.GetComponent<TMPro.TextMeshProUGUI>().text = "" + calculatedScore;


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
        

        if (bootsIsTrue && hatIsTrue && topIsTrue && !spinned)
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
        GetComponent<AudioSource>().enabled = false;
        GameOverScreen.SetActive(true);
        HealthManager();
        if (_won)
        {
            WinSound.SetActive(true);
            won = true;
            PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("highscore") + calculatedScore);
            PlayerPrefs.SetFloat("multiplier", PlayerPrefs.GetFloat("multiplier") -0.05f );
            totalscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
            gameovermessage.GetComponent<TMPro.TextMeshProUGUI>().text = "Level completed +" + calculatedScore;
        }
        else
        {
            gameovermessage.GetComponent<TMPro.TextMeshProUGUI>().text = "Volgende keer beter";
            gj.GetComponent<TMPro.TextMeshProUGUI>().text = "jij hebt verloren";
            LooseSound.SetActive(true);
            UpdateHighscore(0);
        }
        gameoverscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
        UpdateHighscore(0);
        yield return new WaitForSeconds(3f);
        if (!_won)
        {
            ReduceHealth();
            yield return new WaitForSeconds(3f);
            if (PlayerPrefs.GetInt("livesCount") <= 0)
            {
                GameOver();
            }
        }

            NextRandomRound();
        yield break;
    }
    void ReduceHealth()
    {
        
        PlayerPrefs.SetInt("livesCount", PlayerPrefs.GetInt("livesCount") - 1);
        
        if (PlayerPrefs.GetInt("livesCount") == 2)
        {
            GameObject[] live3 = GameObject.FindGameObjectsWithTag("Life3");
            foreach (GameObject go in live3) { go.GetComponent<Animator>().SetTrigger("explodeHeart"); }
        }
        if (PlayerPrefs.GetInt("livesCount") == 1)
        {
            GameObject[] live2 = GameObject.FindGameObjectsWithTag("Life2");
            foreach (GameObject go in live2) { go.GetComponent<Animator>().SetTrigger("explodeHeart"); }
        }
    }
    void UpdateHighscore(int scoreAdded)
    {
        //score.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore") + scoreAdded;
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
