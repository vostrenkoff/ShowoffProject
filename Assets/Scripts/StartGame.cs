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
        spinner.SetActive(true);
        spinner.GetComponent<SpinnerController>().startSpinning();

    }
    
}
