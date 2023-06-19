using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BonusLevelScript : MonoBehaviour
{
    //UI
    public Image getreadyBar;
    public GameObject PlayScreen;
    public GameObject PlayerPosition;
    public GameObject GetReadyScreen;
    public GameObject SpinnerPref;
    public GameObject GameOverScreen;
    public GameObject gameovermessage;
    public GameObject gameoverscore;
    
    public GameObject playerpref;
    public bool goPlayTimer = false;
    public bool won = false;
    public bool spinned = false;
    

    //score
    public int calculatedScore=0;
    public GameObject score;
    public GameObject totalscore;

    private float getReadyDuration = 8f;
    private float currentGetReadyDuration; // The current duration elapsed for get ready bar
    private float initialGetReadyFillAmount = 1; // The initial fillAmount of getreadyBar

    private void Start()
    {
        totalscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
    }
    void Update()
    {
        calculatedScore = Mathf.RoundToInt(playerpref.transform.position.x*10);
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

        if (PlayerPosition.transform.position.y < -7 && !spinned)
        {
            spinned = true;
            StartCoroutine(gameoverScreenTimeout(true));
        }
        /*if (timeleftBar.fillAmount > 0 && goPlayTimer)
        {
            if (!won)
                timeleftBar.fillAmount -= 0.0008f;
            else
                timeleftBar.fillAmount = 1;
        }*/


        //loudness
       
        /* if (loudness > Walkthreshold)
         {


         }
         if (loudness > jumpthreshold)
         {

         }*/
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
            //listenandsay.GetComponent<TMPro.TextMeshProUGUI>().text = "Well done.";
            yield break;
        }
        //listenandsay.GetComponent<TMPro.TextMeshProUGUI>().text = "Repeat!";

        

        yield break;
    }
    /*private IEnumerator winScreenTimeout()
    {
        
        PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("highscore") + calculatedScore);
        totalscore.GetComponent<TMPro.TextMeshProUGUI>().text = ""+ PlayerPrefs.GetInt("highscore");
        won = true;
        //tooquiet.SetActive(false);
        yield return new WaitForSeconds(2f);
        NextRandomRound();

        yield break;
    }*/
    private IEnumerator gameoverScreenTimeout(bool _won)
    {
        GameOverScreen.SetActive(true);
        if (_won)
        {
            won = true;
            PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("highscore") + calculatedScore);
            totalscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
            gameovermessage.GetComponent<TMPro.TextMeshProUGUI>().text = "Level completed +" + calculatedScore;
        }
        gameoverscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
        yield return new WaitForSeconds(3f);
        

        NextRandomRound();
        yield break;
    }

}
