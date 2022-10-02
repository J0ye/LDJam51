using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Connections")]
    public List<Enemy> enemies = new List<Enemy>();
    public TMP_Text timer;
    [Header("Values")]
    public string nextLevel = "";
    public int playerHealth = 3;
    public float startDelay = 3f;
    public bool pause = false;
    public bool finishedLevel = false;
    [Header("Graphics")]
    public GameObject bloodPrefab;

    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnFinish = new UnityEvent();
    public UnityEvent OnGameOver = new UnityEvent();
    public UnityEvent OnOutOfTime = new UnityEvent();
    public UnityEvent OnDamage = new UnityEvent();

    public delegate void EmptyHandler();
    public event EmptyHandler OnAction;

    protected float time = 0f;
    protected int round = 0;
    protected int cycleCount = 0;

    private static GameManager instance;
    public static GameManager Instance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        Invoke("InvokeStart", startDelay);
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    public void InvokeAction()
    {
        if (!pause)
        {
            OnAction.Invoke();
            round++;
            Invoke("ReadyUp", 1f);
            if(round < 10)
            {
                pause = true;
            }
            else
            {
                Invoke("OutOfTime", 1.1f);
            }
        }
    }

    public void OutOfTime()
    {
        if (!finishedLevel)
        {
            playerHealth = 0;
            DamagePlayer();
            OnOutOfTime.Invoke();
        }
    }

    public void DamagePlayer()
    {
        OnDamage.Invoke();
        playerHealth--;
        if (playerHealth <= 0)
        {
            OnGameOver.Invoke();
            Instantiate(bloodPrefab, MouseController.playerPosition, Quaternion.identity);
            MouseController.Kill();
            Invoke("OpenGameOver", 3f);
        }
    }

    public void FinishLevel()
    {
        SceneManager.LoadSceneAsync(nextLevel);
        OnFinish.Invoke();
    }

    public void OpenGameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);

    }

    protected void ReadyUp()
    {
        pause = false;
    }

    protected void Timer()
    {
        if(pause)
        {
            time += Time.deltaTime;
            time = Mathf.Clamp(time, 0, round);
        }
        else
        {
            time = round;
        }
        timer.text = time.ToString("0.00");
    }

    protected void InvokeStart()
    {
        OnStart.Invoke();
    }
}
