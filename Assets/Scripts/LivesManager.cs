using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    GameObject live1;
    GameObject live2;
    GameObject live3;
    void Start()
    {
        live1 = GameObject.Find("Life1");
        live2 = GameObject.Find("Life2");
        live3 = GameObject.Find("Life3");
    }

    void Update()
    {

        Debug.Log("livesCount " + PlayerPrefs.GetInt("livesCount"));
        if(PlayerPrefs.GetInt("livesCount") == 3)
        {
            live1.SetActive(true);
            live2.SetActive(true);
            live3.SetActive(true);
        }
        if (PlayerPrefs.GetInt("livesCount") == 2)
        {
            live1.SetActive(true);
            live2.SetActive(true);
            live3.SetActive(false);
        }
        if (PlayerPrefs.GetInt("livesCount") == 1)
        {
            live1.SetActive(true);
            live2.SetActive(false);
            live3.SetActive(false);
        }
        if (PlayerPrefs.GetInt("livesCount") == 0)
        {
            live1.SetActive(false);
            live2.SetActive(false);
            live3.SetActive(false);
        }
    }
}
