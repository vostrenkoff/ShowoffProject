using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClothesGame : MonoBehaviour
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
    public int calculatedScore = 0;
    public GameObject score;
    public GameObject totalscore;
    private void Start()
    {
        HealthManager();
        totalscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");

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

        if(bootsIsTrue && hatIsTrue && topIsTrue && !spinned)
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
