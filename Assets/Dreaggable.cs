using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dreaggable : MonoBehaviour
{
    ClothesGame ClothesGameManager;
    Vector3 mousePositionOffset;
    bool isMoving = false;
    private void Start()
    {
        ClothesGameManager = FindObjectOfType<ClothesGame>();
    }
    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseDown()
    {
        isMoving = true;
        Debug.Log("ban");
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }
    private void OnMouseUp()
    {
        isMoving = false;
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("CollisionTrigger");
        if(!isMoving)
        {
            if (collision.gameObject.CompareTag("BootsPlaceholder") && gameObject.CompareTag("Boots"))
            {
                ClothesGameManager.attachedBoots = gameObject;
                gameObject.transform.position = collision.gameObject.transform.position;
            }
            if (collision.gameObject.CompareTag("HatPlaceholder") && gameObject.CompareTag("Hat"))
            {
                ClothesGameManager.attachedHat = gameObject;
                gameObject.transform.position = collision.gameObject.transform.position;
            }
            if (collision.gameObject.CompareTag("TopPlaceholder") && gameObject.CompareTag("Top"))
            {
                ClothesGameManager.attachedTop = gameObject;
                gameObject.transform.position = collision.gameObject.transform.position;
            }
        }
        
    }
}
