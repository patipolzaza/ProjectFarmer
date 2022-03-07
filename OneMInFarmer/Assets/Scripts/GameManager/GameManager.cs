using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int dayPlayed { get; private set; }
    public float defaultTimePerDay { get; private set; } = 15;
    public float timeForNextDay { get; private set; }
    public float currentTimeLeft { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        timeForNextDay = defaultTimePerDay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRound();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            AddTimeForNextDay(5);
        }
    }

    private IEnumerator CountTime()
    {
        while (currentTimeLeft > 0)
        {
            if (currentTimeLeft > 10)
            {
                yield return new WaitForSeconds(1);
                currentTimeLeft -= 1;
            }
            else
            {
                yield return new WaitForFixedUpdate();
                currentTimeLeft -= Time.deltaTime;
            }
        }

        currentTimeLeft = 0;
        timeForNextDay = defaultTimePerDay;
        dayPlayed++;
        Debug.Log("Round End.");
    }

    public void StartRound()
    {
        currentTimeLeft = timeForNextDay;

        StartCoroutine(CountTime());
    }

    public void AddTimeForNextDay(float time)
    {
        timeForNextDay += time;
    }
}
