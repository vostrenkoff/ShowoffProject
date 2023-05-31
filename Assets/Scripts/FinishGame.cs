using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    public GameObject winTXT;

    void OnCollisionEnter2D(Collision2D col)
    {
        winTXT.SetActive(true);
    }
}
