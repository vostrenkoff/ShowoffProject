using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefendScript : MonoBehaviour
{
    //UI
    public Image getreadyBar;
    public Image timeleftBar;
    public Image healthBar;
    public GameObject PlayScreen;
    public GameObject GetReadyScreen;
    public GameObject GameOverScreen;
    public GameObject SpinnerPref;
    public bool goPlayTimer = false;
    public bool won = false;
    public bool lost = false;
    public bool spinned = false;

    //score
    private int calculatedScore = 200;
    public GameObject score;
    public GameObject totalscore;
    public GameObject gameoverscore;
    public GameObject gameovermessage;

    private float duration = 30f;
    private float currentDuration; // The current duration elapsed
    private float initialFillAmount = 1; // The initial fillAmount of timeleftBar

    private float getReadyDuration = 5f;
    private float currentGetReadyDuration; // The current duration elapsed for get ready bar
    private float initialGetReadyFillAmount = 1; // The initial fillAmount of getreadyBar

    public GameObject[] bugPrefabs; // Prefab to instantiate
    public List<GameObject> currentBugsX = new List<GameObject>();
    public List<GameObject> currentBugsO = new List<GameObject>();
    public List<GameObject> currentBugsY = new List<GameObject>();
    private float creationInterval = 0.8f; // Interval between object creations
    private int minID = 1; // Minimum ID value
    private int maxID = 3; // Maximum ID value

    private float timer = 0f; // Timer to track object creation

    private void Start()
    {
        creationInterval = creationInterval * PlayerPrefs.GetFloat("multiplier");
        Debug.LogError("Duration " + creationInterval);
        HealthManager();
        totalscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
    }
    void Update()
    {
        BugSpawner();

        if (Input.GetKeyUp(KeyCode.X))
        {
            if (currentBugsX.Count > 0)
            {
                if (currentBugsX[0].GetComponent<BugClass>().available)
                {
                    Destroy(currentBugsX[0]);
                    currentBugsX.RemoveAt(0);
                }
                else
                {
                    GetDamage(0.08f);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.O))
        {
            if (currentBugsO.Count > 0)
            {
                if (currentBugsO[0].GetComponent<BugClass>().available)
                {
                    Destroy(currentBugsO[0]);
                    currentBugsO.RemoveAt(0);
                }
                else
                {
                    GetDamage(0.08f);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Y))
        {
            if (currentBugsY.Count > 0)
            {
                if (currentBugsY[0].GetComponent<BugClass>().available)
                {
                    Destroy(currentBugsY[0]);
                    currentBugsY.RemoveAt(0);
                }
                else
                {
                    GetDamage(0.08f);
                }
            }
        }//calculatedScore = Mathf.RoundToInt(playerpref.transform.position.x * 10);
        //score.GetComponent<TMPro.TextMeshProUGUI>().text = "" + calculatedScore;
        //UI
        if (getreadyBar.fillAmount > 0)
        {
            float deltaAmount = (initialGetReadyFillAmount / getReadyDuration) * Time.deltaTime;
            getreadyBar.fillAmount -= deltaAmount;
            getreadyBar.fillAmount = Mathf.Clamp(getreadyBar.fillAmount, 0f, initialGetReadyFillAmount);
            currentGetReadyDuration += Time.deltaTime;

            if (currentGetReadyDuration >= getReadyDuration)
            {
                getreadyBar.fillAmount = 0;
                GetReadyScreen.SetActive(false);
                StartCoroutine(timeout());
            }
        }
        else if (!won)
        {
            GetReadyScreen.SetActive(false);
            StartCoroutine(timeout());
        }
        /*if (timeleftBar.fillAmount <= 0 && !won && !lost)
        {
            lost = true;
            StartCoroutine(loseScreenTimeout());
        }*/
        if (timeleftBar.fillAmount > 0 && goPlayTimer)
        {
            if (!won)
            {
                float deltaAmount = (initialFillAmount / duration) * Time.deltaTime;
                timeleftBar.fillAmount -= deltaAmount;
                timeleftBar.fillAmount = Mathf.Clamp(timeleftBar.fillAmount, 0f, initialFillAmount);
                currentDuration += Time.deltaTime;

                if (currentDuration >= duration)
                {
                    timeleftBar.fillAmount = 0;
                    if (timeleftBar.fillAmount <= 0 && !won && !lost)
                    {
                        won = true;
                        //StartCoroutine(gameoverScreenTimeout(true));
                    }
                }
            }
            else
            {
                timeleftBar.fillAmount = 1;
            }
        }

        if (won && !spinned)
        {
            spinned = true;
            won = true;
            StartCoroutine(gameoverScreenTimeout(true));
        }
    }

    void BugSpawner()
    {
        if (goPlayTimer)
        {
            timer += Time.deltaTime;

            if (timer >= creationInterval)
            {
                CreateBug();
                timer = 0f;
            }
        }
    }

    private void CreateBug()
    {
        int side  = Random.Range(1,4);
        GameObject newObj = Instantiate(bugPrefabs[Random.Range(0, 3)], transform.position, Quaternion.identity);

        switch (side)
        {
            case 1:
            newObj.transform.position = new Vector3(Random.Range(-8.5f,8.5f),4.6f,0);
                break;
            case 2:
                newObj.transform.position = new Vector3(-8.5f, Random.Range(-4.6f, 4.6f), 0);
                break;
            case 3:
                newObj.transform.position = new Vector3(8.5f, Random.Range(-4.6f, 4.6f), 0);
                break;
        }
        int randomID = Random.Range(minID, maxID + 1);
        BugClass bugClass = newObj.GetComponent<BugClass>();

        if (bugClass != null)
        {
            //bugClass.SetID(randomID);
        }
        
        switch (bugClass.id)
        {
            case 1:
                currentBugsX.Add(newObj);
                break;
            case 2:
                currentBugsO.Add(newObj);
                break;
            case 3:
                currentBugsY.Add(newObj);
                break;
        }
    }



    public void NextRandomRound()
    {
        Debug.Log("spinner started");
        SpinnerPref.GetComponent<SpinnerController>().startSpinning();
    }
    private IEnumerator timeout()
    {
        goPlayTimer = true;
        yield return new WaitForSeconds(3f);
        if (won)
        {
            yield break;
        }
        yield break;
    }
    /*private IEnumerator winScreenTimeout()
    {

        PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("highscore") + 100);
        totalscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
        won = true;
        yield return new WaitForSeconds(2f);
        NextRandomRound();

        yield break;
    }*/
    /*private IEnumerator loseScreenTimeout()
    {
        ReduceHealth();
        UpdateHighscore(0);
        yield return new WaitForSeconds(2f);
        NextRandomRound();
        yield break;
    }*/
    private IEnumerator gameoverScreenTimeout(bool _won)
    {
        GameOverScreen.SetActive(true);
        HealthManager();
        if (_won)
        {
            won = true;
            PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("highscore") + calculatedScore);
            PlayerPrefs.SetFloat("multiplier", PlayerPrefs.GetFloat("multiplier") - 0.05f);
            totalscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
            gameovermessage.GetComponent<TMPro.TextMeshProUGUI>().text = "Level completed +" + calculatedScore;
        }
        else
        {
            gameovermessage.GetComponent<TMPro.TextMeshProUGUI>().text = "Better luck next time";
            
            UpdateHighscore(0);
        }
        gameoverscore.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore");
        UpdateHighscore(0);
        yield return new WaitForSeconds(3f);
        if (!_won)
        {
            ReduceHealth();
            yield return new WaitForSeconds(3f);
            if (PlayerPrefs.GetInt("livesCount") <= 0)
            {
                GameOver();
            }
        }

        NextRandomRound();
        yield break;
    }
    void ReduceHealth()
    {

        PlayerPrefs.SetInt("livesCount", PlayerPrefs.GetInt("livesCount") - 1);

        if (PlayerPrefs.GetInt("livesCount") == 2)
        {
            GameObject[] live3 = GameObject.FindGameObjectsWithTag("Life3");
            foreach (GameObject go in live3) { go.GetComponent<Animator>().SetTrigger("explodeHeart"); }
        }
        if (PlayerPrefs.GetInt("livesCount") == 1)
        {
            GameObject[] live2 = GameObject.FindGameObjectsWithTag("Life2");
            foreach (GameObject go in live2) { go.GetComponent<Animator>().SetTrigger("explodeHeart"); }
        }
    }
    void UpdateHighscore(int scoreAdded)
    {
        //score.GetComponent<TMPro.TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("highscore") + scoreAdded;
    }
    void GameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }
    public void GetDamage(float amount)
    {
        if(healthBar.fillAmount <= 0)
        {
            StartCoroutine(gameoverScreenTimeout(false));
        }
        healthBar.fillAmount -= amount;
    }
    
    void HealthManager()
    {
        GameObject[] live1 = GameObject.FindGameObjectsWithTag("Life1");
        GameObject[] live2 = GameObject.FindGameObjectsWithTag("Life2");
        GameObject[] live3 = GameObject.FindGameObjectsWithTag("Life3");

        if (PlayerPrefs.GetInt("livesCount") == 3)
        {
            foreach (GameObject go in live1) { go.SetActive(true); }
            foreach (GameObject go in live2) { go.SetActive(true); }
            foreach (GameObject go in live3) { go.SetActive(true); }
        }
        if (PlayerPrefs.GetInt("livesCount") == 2)
        {
            foreach (GameObject go in live1) { go.SetActive(true); }
            foreach (GameObject go in live2) { go.SetActive(true); }
            foreach (GameObject go in live3) { go.SetActive(false); }
        }
        if (PlayerPrefs.GetInt("livesCount") == 1)
        {
            foreach (GameObject go in live1) { go.SetActive(true); }
            foreach (GameObject go in live2) { go.SetActive(false); }
            foreach (GameObject go in live3) { go.SetActive(false); }
        }
        if (PlayerPrefs.GetInt("livesCount") <= 0)
        {

            foreach (GameObject go in live1) { go.SetActive(false); }
            foreach (GameObject go in live2) { go.SetActive(false); }
            foreach (GameObject go in live3) { go.SetActive(false); }
        }
    }

}
