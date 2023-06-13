using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonPlayer : MonoBehaviour
{
    public DefendScript gameManager;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "X")
        {
            Destroy(gameManager.currentBugsX[0]);
            gameManager.currentBugsX.RemoveAt(0);
        }
        if (collision.gameObject.tag == "Y")
        {
            Destroy(gameManager.currentBugsY[0]);
            gameManager.currentBugsY.RemoveAt(0);
        }
        if (collision.gameObject.tag == "O")
        {
            Destroy(gameManager.currentBugsO[0]);
            gameManager.currentBugsO.RemoveAt(0);
        }
    }
}
