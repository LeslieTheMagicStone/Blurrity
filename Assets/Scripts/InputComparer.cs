using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Events;

public class InputComparer : MonoBehaviour
{
    public KeyCode targetInputKey = KeyCode.Return;
    public float perfectTimingThreshold = 0.1f;
    public float earlyTimingThreshold = 0.2f;
    public float lateTimingThreshold = 0.2f;
    public float badTimingThreshold = 0.3f;
    public string inputFilePath;

    public UnityEvent OnLate, OnEarly, OnPerfect, OnBad;
    private List<float> inputTimes;

    private void Start()
    {
        string[] inputTimeStrings = File.ReadAllLines(inputFilePath);

        inputTimes = new();

        foreach (string timeString in inputTimeStrings)
        {
            if (float.TryParse(timeString, out float time))
            {
                inputTimes.Add(time);
            }
        }
    }

    private void Update()
    {
        if (inputTimes.Count == 0)
        {
            Debug.Log("Game Over");
            return;
        }


        List<float> timesToRemove = new();
        foreach (float time in inputTimes)
        {
            if (Time.time > time + lateTimingThreshold)
            {
                Debug.Log("Too Late");
                timesToRemove.Add(time);
                OnBad.Invoke();
            }
        }
        foreach (float time in timesToRemove) inputTimes.Remove(time);

        if (Input.GetKeyDown(targetInputKey))
        {
            float inputTime = inputTimes[0];
            float timeDifference = Time.time - inputTime;
            if (Mathf.Abs(timeDifference) <= perfectTimingThreshold)
            {
                inputTimes.RemoveAt(0);
                OnPerfect.Invoke();
            }
            else if (timeDifference < 0 && timeDifference >= -earlyTimingThreshold)
            {
                inputTimes.RemoveAt(0);
                OnEarly.Invoke();
            }
            else if (timeDifference > 0 && timeDifference <= lateTimingThreshold)
            {
                inputTimes.RemoveAt(0);
                OnLate.Invoke();
            }
            else if (timeDifference < 0 && timeDifference >= -badTimingThreshold)
            {
                inputTimes.RemoveAt(0);
                OnBad.Invoke();
            }
        }
    }
}