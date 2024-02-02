using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BandSource : MonoBehaviour
{
    public enum SourceType { Master, AudioSource };

    [Header("Important!!")]
    public SourceType sourceType;

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

    public float initialBufferDecrease;
    public float bandVolumeThreshold;
    public bool accelerateBufferDecrease;

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

        // Init band highests.
        for (int i = 0; i < bandHighests.Length; i++)
        {
            bandHighests[i] = 0.0001f;
        }

    }

    private void Start()
    {
        // Get Audio Sources if needed
        if (sourceType == SourceType.AudioSource)
        {
            audioSources = GetComponents<AudioSource>();
        }
    }

    private void Update()
    {
        GetAudioSpectrumSource();
        UpdateOutput();
    }

    private void GetAudioSpectrumSource()
    {
        if (sourceType == SourceType.Master)
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
            rawBands[bandIndex] = average > bandVolumeThreshold ? average : 0;
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
                    bufferDecrease[i] *= 1.2f;
                }

                bufferedBands[i] = Mathf.Max(bufferedBands[i], 0);
            }
        }

        // Update band highests.
        for (int i = 0; i < bandHighests.Length; i++)
        {
            if (rawBands[i] > bandHighests[i])
            {
                bandHighests[i] = rawBands[i];
            }
        }

        // Update raw relative bands.
        for (int i = 0; i < rawRelativeBands.Length; i++)
            rawRelativeBands[i] = rawBands[i] / bandHighests[i];

        // Update buffered relative bands.
        for (int i = 0; i < bufferedRelativeBands.Length; i++)
            bufferedRelativeBands[i] = bufferedBands[i] / bandHighests[i];
    }
}
