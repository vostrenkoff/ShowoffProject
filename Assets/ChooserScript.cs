using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChooserScript : MonoBehaviour
{
    public GameObject center;
    public GameObject right;
    public GameObject left;
    public GameObject up;
    public GameObject down;
    public GameObject HATplaceholder;
    public GameObject TOPplaceholder;
    public GameObject BOOTSplaceholder;
    private Gamepad dualSenseGamepad;
    Vector2 InputVector;
    bool isSticked = false;
    private void Start()
    {
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.name.Contains("DualSense"))
            {
                dualSenseGamepad = gamepad;
                break;
            }
        }

        if (dualSenseGamepad == null)
        {
            Debug.LogError("DualSense gamepad not found!");
        }
    }
    private void Update()
    {
        if (dualSenseGamepad == null)
            return;

        // Get the input values from the left stick
        Vector2 stickInput = dualSenseGamepad.leftStick.ReadValue();

        // Calculate the movement vector based on the input values
        InputVector = new Vector2(stickInput.x, stickInput.y);
        Debug.Log(InputVector);

        if (Input.GetKeyDown(KeyCode.W) || InputVector.y > 0.4f && InputVector.x < 0.5f && InputVector.x > -0.5f && !isSticked )
        {
            CheckCollision(up);
            isSticked = true;
        }
        if (Input.GetKeyDown(KeyCode.A) || InputVector.x < -0.4f && InputVector.y < 0.5f && InputVector.y > -0.5f && !isSticked)
        {
            CheckCollision(left);
            isSticked = true;
        }
        if (Input.GetKeyDown(KeyCode.S) || InputVector.y < -0.4f && InputVector.x < 0.5f && InputVector.x > -0.5f&&!isSticked)
        {
            CheckCollision(down);
            isSticked = true;
        }
        if (Input.GetKeyDown(KeyCode.D) || InputVector.x > 0.4f && InputVector.y < 0.5f && InputVector.y > -0.5f && !isSticked)
        {
            CheckCollision(right);
            isSticked = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) || dualSenseGamepad.buttonSouth.wasPressedThisFrame)
        {
            ChooseCloth();
        }
        if(InputVector.x > -0.05f && InputVector.x < 0.05f && InputVector.y > -0.05 && InputVector.y < 0.05)
        {
            isSticked= false;
        }
    }

    private void CheckCollision(GameObject direction)
    {
        Collider2D directionCollider = direction.GetComponent<Collider2D>();
        if (directionCollider == null)
        {
            Debug.LogWarning("No collider found on the direction object: " + direction.name);
            return;
        }
        List<Collider2D> colliders = new List<Collider2D>();
        directionCollider.OverlapCollider(new ContactFilter2D(), colliders);

        foreach (Collider2D collider in colliders)
        {
            Debug.Log("Collision with  ON LIST " + collider.name);
            if (collider.gameObject.CompareTag("Hat") || collider.gameObject.CompareTag("Top") || collider.gameObject.CompareTag("Boots"))
            {
                if(collider.GetComponent<DefaultVector>().choosen == false)
                {
                    Debug.Log("Collision with " + direction.name + " of tag " + collider.tag);
                    transform.position = collider.transform.position;
                    break;
                }
            }
        }
    }
    void ChooseCloth()
    {
        Collider2D directionCollider = GetComponent<Collider2D>();
        if (directionCollider == null)
        {
            Debug.LogWarning("No collider found");
            return;
        }
        List<Collider2D> colliders = new List<Collider2D>();
        directionCollider.OverlapCollider(new ContactFilter2D(), colliders);

        foreach (Collider2D collider in colliders)
        {
            Debug.Log("Collision with  ON LIST " + collider.name);
            if (collider.gameObject.CompareTag("Hat"))
            {
                Debug.Log("Collision with " + collider.name + " of tag " + collider.tag);
                ReturnCloth(HATplaceholder);
                collider.transform.position = HATplaceholder.transform.position;
                collider.GetComponent<DefaultVector>().choosen = true;
                GetComponent<AudioSource>().Play();
                break;
            }
            if (collider.gameObject.CompareTag("Top"))
            {
                Debug.Log("Collision with " + collider.name + " of tag " + collider.tag);
                ReturnCloth(TOPplaceholder);
                collider.transform.position = TOPplaceholder.transform.position;
                collider.GetComponent<DefaultVector>().choosen = true;
                GetComponent<AudioSource>().Play();
                break;
            }
            if (collider.gameObject.CompareTag("Boots"))
            {
                Debug.Log("Collision with " + collider.name + " of tag " + collider.tag);
                ReturnCloth(BOOTSplaceholder);
                collider.transform.position = BOOTSplaceholder.transform.position;
                collider.GetComponent<DefaultVector>().choosen = true;
                GetComponent<AudioSource>().Play();
                break;
            }
        }
    }
    void ReturnCloth(GameObject placeholder)
    {
        Collider2D placeholderCollider = placeholder.GetComponent<Collider2D>();
        if (placeholderCollider == null)
        {
            Debug.LogWarning("No collider found");
            return;
        }
        List<Collider2D> colliders = new List<Collider2D>();
        placeholderCollider.OverlapCollider(new ContactFilter2D(), colliders);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Hat") || collider.gameObject.CompareTag("Top") || collider.gameObject.CompareTag("Boots"))
            {
                Vector2 returnPos = collider.GetComponent<DefaultVector>().DefaultVector2;
                collider.transform.position = returnPos;
                collider.GetComponent<DefaultVector>().choosen = false;
                break;
            }
        }
    }
}