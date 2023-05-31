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
    
    public GameObject playerpref;
    public bool goPlayTimer = false;
    public bool won = false;
    public bool spinned = false;

    //score
    public int calculatedScore=0;
    public GameObject score;
    public GameObject totalscore;
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
            getreadyBar.fillAmount -= 0.002f;
        }
        else if (!won)
        {
            GetReadyScreen.SetActive(false);
            //PlayScreen.SetActive(true);
            StartCoroutine(timeout());
        }

        if(PlayerPosition.transform.position.y < -7 && !spinned)
        {
            spinned = true;
            StartCoroutine(winScreenTimeout());
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
    private IEnumerator winScreenTimeout()
    {
        
        PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("highscore") + calculatedScore);
        totalscore.GetComponent<TMPro.TextMeshProUGUI>().text = ""+ PlayerPrefs.GetInt("highscore");
        won = true;
        //tooquiet.SetActive(false);
        yield return new WaitForSeconds(2f);
        NextRandomRound();

        yield break;
    }
}
