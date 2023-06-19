using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spinner : MonoBehaviour
{
    public float genSpeed;
    public float subSpeed;
    public bool isSpinning = false;
    public bool isSpinned = false;
    public ArrowScript arrowScript;
    private bool bonus1active = false;
    private bool bonus2active = false;
    private bool bonus3active = false;
    private bool bonus4active = false;
    private bool bonus5active = false;
    private bool bonus6active = false;
    void Update()
    {
        //Debug.Log(transform.rotation.eulerAngles);
        //Debug.Log("ROTATION: "+transform.rotation);
        if (isSpinning)
        {
            transform.Rotate(0, 0, genSpeed, Space.World);
            genSpeed -= subSpeed;
            isSpinned = true;
        }
        if(genSpeed <=0)
        {
            genSpeed= 0;
            isSpinning= false;
            if(isSpinned)
                ChosenGame();
        }
    }
    public void SpinWheel()
    {
        genSpeed = Random.Range(2.000f, 5.000f);
        subSpeed = Random.Range(0.003f, 0.009f);
        isSpinning = true;
        
    }
    public void ChosenGame()
    {
        if(PlayerPrefs.GetFloat("highscore") >=400&& PlayerPrefs.GetInt("bonus1active") < 1)
        {
            PlayerPrefs.SetInt("bonus1active", 1);
            SceneManager.LoadScene("BonusLevel");
        }
        if (PlayerPrefs.GetFloat("highscore") >= 800&& PlayerPrefs.GetInt("bonus2active") < 1)
        {
            PlayerPrefs.SetInt("bonus2active", 1);
            SceneManager.LoadScene("BonusLevel");
        }
        if (PlayerPrefs.GetFloat("highscore") >= 1500&& PlayerPrefs.GetInt("bonus3active") < 1)
        {
            PlayerPrefs.SetInt("bonus3active", 1);
            SceneManager.LoadScene("BonusLevel");
        }
        if (PlayerPrefs.GetFloat("highscore") >= 2000 && PlayerPrefs.GetInt("bonus4active") < 1)
        {
            PlayerPrefs.SetInt("bonus4active", 1);
            SceneManager.LoadScene("BonusLevel");
        }
        if (PlayerPrefs.GetFloat("highscore") >= 2400 && PlayerPrefs.GetInt("bonus5active") < 1)
        {
            PlayerPrefs.SetInt("bonus5active", 1);
            SceneManager.LoadScene("BonusLevel");
        }
        
        else if (arrowScript.gameID == 2)
        {
            SceneManager.LoadScene("Roar1");
        }
        else if (arrowScript.gameID == 4)
        {
            SceneManager.LoadScene("Clothes");
        }
        else if (arrowScript.gameID == 1)
        {
            SceneManager.LoadScene("DefendTheField");
        }
        else if (arrowScript.gameID == 3)
        {
            SceneManager.LoadScene("CityDestroyer");
        }
        
    }
}
    
