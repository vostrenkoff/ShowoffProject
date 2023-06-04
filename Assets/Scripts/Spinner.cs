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
    void Update()
    {
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
        genSpeed = Random.Range(2.000f, 3.000f);
        subSpeed = Random.Range(0.003f, 0.009f);
        isSpinning = true;
        
    }
    public void ChosenGame()
    {
        int randomNumber = Random.Range(0, 4);
        if (randomNumber == 0)
        {
            SceneManager.LoadScene("Roar1");
        }
        else if (randomNumber == 1)
        {
            SceneManager.LoadScene("Clothes");
        }
        else if (randomNumber == 2)
        {
            SceneManager.LoadScene("BonusLevel");
        }
        else if (randomNumber == 3)
        {
            SceneManager.LoadScene("CityDestroyer");
        }


    }
}
