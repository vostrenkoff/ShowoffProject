using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public AudioSource source;
    public float multiplier;
    public AudioLoudnessDetection detector;
    public GameObject volumelevel;
    public GameObject tooquiet;
    public float loudnessSensibility = 100;
    public float Walkthreshold =  0.1f;
    public float jumpthreshold =  0.1f;
    float loudnessRecord = 0;

    private Animator playerAnimator;
    private void Start()
    {
    }
    void Update()
    {
        float loudness = (detector.GetLoudnessFromMicrophone() * multiplier)/10;
        if(loudness > loudnessRecord) 
        {
            loudnessRecord = loudness;
        }
        loudnessRecord -= 0.0008f;
        if(loudnessRecord >0.5 && loudnessRecord< 1) 
        { 
            tooquiet.SetActive(true);
        }
        Debug.Log(loudness);
        volumelevel.GetComponent<Image>().fillAmount= loudnessRecord;
        if (loudness > Walkthreshold)
        {
           
            
        }
        if(loudness > jumpthreshold)
        {

        }

    }

}
