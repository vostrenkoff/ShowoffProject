using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugClass : MonoBehaviour
{
    public int id;
    public bool available;
    private Vector3 targetPosition = new Vector3(0f, -0.73f, 0f); // Target position to move towards
    float moveSpeed = 0.25f; // Speed of the movement
    [SerializeField] GameObject gameManager;
    public void SetID(int value)
    {
        available = false;
        id = value;
    }
    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    private void Update()
    {
        // Calculate the new position using Vector3.Lerp
        Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Update the object's position
        transform.position = newPosition;
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Player")
        {
            gameManager.GetComponent<DefendScript>().GetDamage(0.25f);
            Destroy(gameObject);
        }
    }*/
    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Circle")
        {
            Debug.Log("eblan");
            available = true;
        }
    }
}