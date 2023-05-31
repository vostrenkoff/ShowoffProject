using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class SpinnerController : MonoBehaviour
{
    public GameObject spinner;
    public GameObject arrow;
    public GameObject background;
    private bool enableSpinner = false;
    int currentpos = 2000;
    public void startSpinning()
    {

        transform.localPosition = new Vector3(0, currentpos, 0);
        
        spinner.SetActive(true);
        arrow.SetActive(true);
        enableSpinner = true;
        //background.SetActive(true);
        StartCoroutine(timeout());

    }
    private void Update()
    {
        if(enableSpinner && transform.localPosition.y > 0)
        {
            currentpos = currentpos - 20;
            transform.localPosition = new Vector3(0, currentpos, 0);

        }
    }
    private IEnumerator timeout()
    {
        yield return new WaitForSeconds(1.5f);
        spinner.GetComponent<Spinner>().SpinWheel();
        yield break;
    }
}
