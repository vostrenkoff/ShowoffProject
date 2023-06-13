using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VoiceController : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject GameManager;
    public Animator playerAnimator;
    public AudioSource source;
    public float multiplier;
    public float movementSpeed;
    public float jumpForce;
    public AudioLoudnessDetection detector;

    public float loudnessSensibility = 100;
    public float Walkthreshold =  0.1f;
    public float jumpthreshold =  0.1f;


    public float totalTime = 5.0f; // Total time for the countdown
    private float timeLeft; // Time left in the countdown
    public Text timerText; // Text object to display the countdown
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeLeft = totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.GetComponent<BonusLevelScript>().goPlayTimer == false)
        {
            return;
        }
        float loudness = detector.GetLoudnessFromMicrophone() * multiplier;
        //Debug.Log(loudness);
        if (loudness > Walkthreshold)
        {
            // move the character forward by a certain amount
            rb.AddForce(transform.right * movementSpeed, ForceMode2D.Impulse);
            
        }
        if(loudness > jumpthreshold)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
        if(transform.position.y < -10 || transform.position.y > 25 )
        {
            Respawn();
        }

        timeLeft -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        //timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timeLeft <= 0)
        {
            Respawn();
        }

        //Animations
        if(rb.velocity.y > 0)
        {
            playerAnimator.SetBool("Falling", false);
            playerAnimator.SetBool("Idling", false);
            playerAnimator.SetBool("Jumping", true);
        }
        if (rb.velocity.y < 0)
        {
            playerAnimator.SetBool("Jumping", false);
            playerAnimator.SetBool("Idling", false);
            playerAnimator.SetBool("Falling", true);
        }
        else if (rb.velocity.y < 1 && rb.velocity.y > -1)
        {
            playerAnimator.SetBool("Idling", true);
            playerAnimator.SetBool("Jumping", false);
            playerAnimator.SetBool("Falling", false);
        }
        Debug.Log(rb.velocity);
    }
    void Respawn()
    {
        timeLeft = totalTime;
        //transform.position = new Vector3(-10, -3, 0);
        rb.velocity = Vector3.zero;
    }
    
}
