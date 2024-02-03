using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BandSource : MonoBehaviour
{
    public enum SourceType { Global, Local };

    [Header("Important!!")]
    public SourceType sourceType = SourceType.Local;
    public float initialBufferDecrease = 0.01f;
    [SerializeField]
    public float[] absoluteVolumeThreshold = { 0.002f, 0.00001f, 0.00001f, 0.00001f, 0.00001f, 0.0000001f, 0.0000001f, 0.0000001f };
    public float relativeVolumeThreshold = 0.01f;
    public bool accelerateBufferDecrease = false;

    public float[] samples = new float[512];
    public float[] rawBands = new float[8];
    public float[] bufferedBands = new float[8];

    /// <summary>
    /// It ranges from zero to one and represents the amplitude of each band relative to its band history value.
    /// </summary>
    public float[] rawRelativeBands = new float[8];
    /// <summary>
    /// It ranges from zero to one and represents the amplitude of each band relative to its band history value.
    /// </summary>
    public float[] bufferedRelativeBands = new float[8];

    private float[] bandHighests = new float[8];
    private float[] bufferDecrease = new float[8];

    private AudioSource[] audioSources;

    private void Awake()
    {
        // Init raw bands.
        for (int i = 0; i < rawBands.Length; i++)
        {
            rawBands[i] = 0;
        }

        // Init buffered bands.
        for (int i = 0; i < bufferedBands.Length; i++)
        {
            bufferedBands[i] = 0;
        }
    }

    private void Start()
    {
        GetAudioSources();
    }

    private void Update()
    {
        GetAudioSources();
        GetAudioSpectrumSource();
        UpdateOutput();
    }

    private void GetAudioSources()
    {
        // Get Audio Sources if needed
        if (sourceType == SourceType.Local)
        {
            var bounds = GetComponent<Collider>().bounds;
            AudioSource[] sounds = FindObjectsOfType<AudioSource>();
            audioSources = sounds.Where(s => bounds.Contains(s.transform.position)).ToArray();
        }
    }

    private void GetAudioSpectrumSource()
    {
        if (sourceType == SourceType.Global)
        {
            AudioListener.GetSpectrumData(samples, 0, FFTWindow.Blackman);
        }
        else
        {
            samples = new float[samples.Length];
            foreach (var audioSource in audioSources)
            {
                var sourceSamples = new float[samples.Length];
                audioSource.GetSpectrumData(sourceSamples, 0, FFTWindow.Blackman);

                for (int i = 0; i < samples.Length; i++)
                    samples[i] += sourceSamples[i];
            }
        }
    }

    private void UpdateOutput()
    {
        // Update raw bands.
        int sampleIndex = 0;
        for (int bandIndex = 0; bandIndex < rawBands.Length; bandIndex++)
        {
            int bandSampleCount = (int)MathF.Pow(2, bandIndex + 1);
            if (bandIndex == 7) bandSampleCount += 2;

            float sampleSum = 0;
            for (int j = 0; j < bandSampleCount; j++)
            {
                sampleSum += samples[sampleIndex];
                sampleIndex++;
            }
            float average = sampleSum / bandSampleCount;

            // Update band highests.
            if (average > bandHighests[bandIndex]) bandHighests[bandIndex] = average;

            if (average < absoluteVolumeThreshold[bandIndex]) average = 0;

            float relativeValue = bandHighests[bandIndex] == 0 ? 0 : average / bandHighests[bandIndex];

            rawBands[bandIndex] = relativeValue > relativeVolumeThreshold ? average : 0;
        }

        // Update buffered bands.
        for (int i = 0; i < bufferedBands.Length; i++)
        {
            if (rawBands[i] > bufferedBands[i])
            {
                bufferedBands[i] = rawBands[i];
                bufferDecrease[i] = rawBands[i] * initialBufferDecrease;
            }
            else
            {
                bufferedBands[i] -= bufferDecrease[i];

                if (accelerateBufferDecrease)
                {
                    bufferDecrease[i] *= 1.02f;
                }

                bufferedBands[i] = Mathf.Max(bufferedBands[i], 0);
            }
        }

        // Update raw relative bands.
        for (int i = 0; i < rawRelativeBands.Length; i++)
        {
            float relativeValue = bandHighests[i] == 0 ? 0 : rawBands[i] / bandHighests[i];
            rawRelativeBands[i] = relativeValue;
        }


        // Update buffered relative bands.
        for (int i = 0; i < bufferedRelativeBands.Length; i++)
        {
            float relativeValue = bandHighests[i] == 0 ? 0 : bufferedBands[i] / bandHighests[i];
            bufferedRelativeBands[i] = relativeValue;
        }
    }
}
