using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject spinner;
    public void StartAndSpin()
    {
        PlayerPrefs.SetInt("livesCount", 3);
        PlayerPrefs.SetInt("highscore", 0);
        PlayerPrefs.SetFloat("multiplier", 1);
        PlayerPrefs.SetInt("bonus1active", 0);
        PlayerPrefs.SetInt("bonus2active", 0);
        PlayerPrefs.SetInt("bonus3active", 0);
        PlayerPrefs.SetInt("bonus4active", 0);
        PlayerPrefs.SetInt("bonus5active", 0);
        spinner.SetActive(true);
        spinner.GetComponent<SpinnerController>().startSpinning();

    }
    
}
