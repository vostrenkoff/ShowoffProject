using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class GamePadInput : MonoBehaviour
{
    private Gamepad dualSenseGamepad;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int buildingsCount = 15;
    [SerializeField] private int attheendbuildingsCount;
    [SerializeField] private GameObject[] buildings;
    [SerializeField] private DestroyerGame manager;
    private GameObject[] generatedbuildings = new GameObject[15];

    private float minX = -7.1f; // Minimum X position
    private float maxX = 6.33f; // Maximum X position
    private float minY = -2.8f; // Minimum Y position
    private float maxY = 2f; // Maximum Y position
    public float minDistance = 1f;
    public int maxAttempts = 20;
    private bool stopSpawning = false;
    private void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // Find the DualSense gamepad
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



        generatedbuildings = new GameObject[buildingsCount]; // Initialize the array to store spawned buildings

        for (int i = 0; i < buildingsCount; i++)
        {
            Vector2 randomPosition = GenerateRandomPosition();
            int randomIndex = Random.Range(0, buildings.Length);

            GameObject newBuilding = Instantiate(buildings[randomIndex], randomPosition, Quaternion.identity);
            generatedbuildings[i] = newBuilding;
            attheendbuildingsCount++;
            // Check for overlap with previous buildings
            bool isOverlapping = CheckOverlap(newBuilding, i);
            int attemptCount = 1;
            while (isOverlapping)
            {
                randomPosition = GenerateRandomPosition();
                newBuilding.transform.position = randomPosition;
                isOverlapping = CheckOverlap(newBuilding, i);
                attemptCount++;
                if (attemptCount > maxAttempts)
                {
                    Debug.LogWarning("Max spawn attempts reached for building " + i);
                    isOverlapping = false;
                    stopSpawning = true;
                    break;
                }
            }
            if (stopSpawning)
                break;

        }
    }
    private Vector2 GenerateRandomPosition()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector2(randomX, randomY);
    }

    private bool CheckOverlap(GameObject newObject, int index)
    {
        if (index == 0)
            return false;

        Collider2D newCollider = newObject.GetComponent<Collider2D>();

        for (int i = 0; i < index; i++)
        {
            Collider2D existingCollider = generatedbuildings[i].GetComponent<Collider2D>();
            float distance = Vector2.Distance(newCollider.bounds.center, existingCollider.bounds.center);

            if (distance < minDistance)
            {
                
                return true;
            }
        }
        return false;
    }
    private void Update()
    {
        Debug.LogWarning(attheendbuildingsCount);
        if (dualSenseGamepad == null)
            return;

        // Get the input values from the left stick
        Vector2 stickInput = dualSenseGamepad.leftStick.ReadValue();

        // Calculate the movement vector based on the input values
        Vector2 movement = new Vector2(stickInput.x, stickInput.y) * moveSpeed;

        // Apply the movement to the rigidbody
        rb.velocity = movement;

        if(rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            animator.SetBool("Walking", false);
        }
        else
            animator.SetBool("Walking", true);

        if (dualSenseGamepad.buttonSouth.wasPressedThisFrame)
        {
            Debug.Log("X button pressed");
            animator.SetTrigger("Crushing");
            CheckForCollisions();
        }

        // Flip character scale based on movement direction
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(-0.3865469f, 0.3865469f, 0.3865469f);
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(0.3865469f, 0.3865469f, 0.3865469f);
        }

        // Display input values
        /*DisplayButtonState(dualSenseGamepad.buttonSouth, "Cross");
        DisplayButtonState(dualSenseGamepad.buttonEast, "Circle");
        DisplayButtonState(dualSenseGamepad.buttonNorth, "Square");
        DisplayButtonState(dualSenseGamepad.buttonWest, "Triangle");

        DisplayAxisState(dualSenseGamepad.leftStick.x, "Left Stick X");
        DisplayAxisState(dualSenseGamepad.leftStick.y, "Left Stick Y");

        DisplayAxisState(dualSenseGamepad.rightStick.x, "Right Stick X");
        DisplayAxisState(dualSenseGamepad.rightStick.y, "Right Stick Y");

        DisplayAxisState(dualSenseGamepad.leftTrigger, "Left Trigger");
        DisplayAxisState(dualSenseGamepad.rightTrigger, "Right Trigger");*/
        foreach(GameObject go in generatedbuildings)
        {
            if(go == null)
                continue;
            if (go.CompareTag("BuildingSmall"))
            {
                if (go.transform.position.y > gameObject.transform.GetChild(0).transform.position.y - 1.6f)
                {
                    SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                    sr.sortingOrder = 0;
                }
                else
                {
                    SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                    sr.sortingOrder = 5;
                }
            }
            if (go.CompareTag("BuildingMedium"))
            {
                if (go.transform.position.y > gameObject.transform.GetChild(0).transform.position.y - 1.5f)
                {
                    SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                    sr.sortingOrder = 0;
                }
                else
                {
                    SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                    sr.sortingOrder = 5;
                }
            }
            if (go.CompareTag("BuildingBig"))
            {
                if (go.transform.position.y > gameObject.transform.GetChild(0).transform.position.y - 1)
                {
                    SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                    sr.sortingOrder = 0;
                }
                else
                {
                    SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                    sr.sortingOrder = 5;
                }
            }

        }
    }

    private void DisplayButtonState(ButtonControl button, string buttonName)
    {
        if (button.wasPressedThisFrame)
        {
            Debug.Log(buttonName + " pressed");
        }

        if (button.wasReleasedThisFrame)
        {
            Debug.Log(buttonName + " released");
        }
    }

    private void DisplayAxisState(AxisControl axis, string axisName)
    {
        if (Mathf.Abs(axis.ReadValue()) > 0.1f)
        {
            Debug.Log(axisName + ": " + axis.ReadValue());
        }
    }
    private void CheckForCollisions()
    {
        Collider2D[] colliders = new Collider2D[10]; // Adjust the size of the array as needed

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true; // Include trigger colliders in the overlap check

        int count = Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, colliders);

        for (int i = 0; i < count; i++)
        {
            Collider2D collider = colliders[i];
            if (collider.CompareTag("BuildingSmall") || collider.CompareTag("BuildingMedium") || collider.CompareTag("BuildingBig"))
            {
                attheendbuildingsCount--;
                collider.gameObject.GetComponent<Animator>().enabled = true;
                collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                if(attheendbuildingsCount == 0)
                {
                    manager.won = true;
                    Debug.LogError("win");
                }
                GetComponent<AudioSource>().Play();
            }
        }
    }
}