using System.Collections;
using System.IO;
using UnityEngine;

public class DrumPlayer : Instrument
{
    public AudioClip drumSound;
    public float timeOffset;

    private void Start()
    {
        string[] inputTimes = File.ReadAllLines(inputFilePath);

        foreach (string timeString in inputTimes)
        {
            if (float.TryParse(timeString, out float time))
            {
                PlayDelayed(time + timeOffset);
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

        var drumAudio = new GameObject("OneShotDrumAudio");
        drumAudio.transform.SetParent(transform);
        drumAudio.transform.localPosition = new(0, 0, 0);
        var audioSource = drumAudio.AddComponent<AudioSource>();
        audioSource.PlayOneShot(drumSound);
    }
}