using System.Collections;
using System.IO;
using UnityEngine;

public class DrumPlayer : MonoBehaviour
{
    public AudioClip drumSound;

    [SerializeField]
    private string inputFilePath;

    private void Start()
    {
        string[] inputTimes = File.ReadAllLines(inputFilePath);

        foreach (string timeString in inputTimes)
        {
            if (float.TryParse(timeString, out float time))
            {
                PlayDelayed(time);
            }
        }
    }

    private void PlayDelayed(float delay)
    {
        StartCoroutine(PlayDelayedCoroutine(delay));
    }

    private IEnumerator PlayDelayedCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioSource.PlayClipAtPoint(drumSound, transform.position);
    }
}