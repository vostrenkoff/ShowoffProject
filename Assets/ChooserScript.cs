using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            CheckCollision(up);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            CheckCollision(left);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            CheckCollision(down);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            CheckCollision(right);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChooseCloth();
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
                break;
            }
            if (collider.gameObject.CompareTag("Top"))
            {
                Debug.Log("Collision with " + collider.name + " of tag " + collider.tag);
                ReturnCloth(TOPplaceholder);
                collider.transform.position = TOPplaceholder.transform.position;
                collider.GetComponent<DefaultVector>().choosen = true;
                break;
            }
            if (collider.gameObject.CompareTag("Boots"))
            {
                Debug.Log("Collision with " + collider.name + " of tag " + collider.tag);
                ReturnCloth(BOOTSplaceholder);
                collider.transform.position = BOOTSplaceholder.transform.position;
                collider.GetComponent<DefaultVector>().choosen = true;
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