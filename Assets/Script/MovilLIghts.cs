using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovilLIghts : MonoBehaviour
{
    public GameObject[] lights;
    private float timeMax = 5.0f;
    private float timeMin = 2.0f;
    private float timeToSwitch;
    private float timer;
    private int currentIndex = 0;
    private int newIndex;
    // Start is called before the first frame update
    void Start()
    {
        lights[currentIndex].SetActive(true);
        lights[1].SetActive(false);
        lights[2].SetActive(false);
        lights[3].SetActive(false);
        lights[4].SetActive(false);
        timeToSwitch = Random.Range(timeMin, timeMax);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToSwitch)
        {
            int newIndex = Random.Range(0, lights.Length);
            currentIndex = newIndex;                     
            if (currentIndex == 0)
            {
                lights[currentIndex].SetActive(true);
                lights[1].SetActive(false);
                lights[2].SetActive(false);
                lights[3].SetActive(false);
                lights[4].SetActive(false);
                timer = 0.0f;
                timeToSwitch = Random.Range(timeMin, timeMax);
            }
            if (currentIndex == 1)
            {
                lights[0].SetActive(false);
                lights[currentIndex].SetActive(true);
                lights[2].SetActive(false);
                lights[3].SetActive(false);
                lights[4].SetActive(false);
                timer = 0.0f;
                timeToSwitch = Random.Range(timeMin, timeMax);
            }
            if (currentIndex == 2)
            {
                lights[0].SetActive(false);
                lights[1].SetActive(false);
                lights[currentIndex].SetActive(true);
                lights[3].SetActive(false);
                lights[4].SetActive(false);
                timer = 0.0f;
                timeToSwitch = Random.Range(timeMin, timeMax);
            }
            if (currentIndex == 3)
            {
                lights[0].SetActive(false);
                lights[1].SetActive(false);
                lights[2].SetActive(false);
                lights[currentIndex].SetActive(true);
                lights[4].SetActive(false);
                timer = 0.0f;
                timeToSwitch = Random.Range(timeMin, timeMax);
            }
            if (currentIndex == 4)
            {
                lights[0].SetActive(false);
                lights[1].SetActive(false);
                lights[2].SetActive(false);
                lights[3].SetActive(false);
                lights[currentIndex].SetActive(true);
                timer = 0.0f;
                timeToSwitch = Random.Range(timeMin, timeMax);
            }
        }
    }
}
