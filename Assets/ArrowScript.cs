using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public int gameID;
    private void Update()
    {
        Debug.Log(gameID);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "wakey")
        {
            gameID= 2   ;
        }
        if (collision.gameObject.tag == "defend")
        {
            gameID = 1;
        }
        if (collision.gameObject.tag == "destroy")
        {
            gameID = 3;
        }
        if (collision.gameObject.tag == "clothes")
        {
            gameID = 4;
        }
    }
}
