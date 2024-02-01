using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    public static float[] samples = new float[512];
    public static float[] outputs = new float[8];

    private void Update()
    {
        GetAudioSpectrumSource();
        UpdateOutput();
    }

    private void GetAudioSpectrumSource()
    {
        AudioListener.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    private void UpdateOutput()
    {
        for (int i = 0; i < outputs.Length; i++)
        {
            outputs[i] = 0;
        }

        for (int i = 0; i < samples.Length; i++)
        {
            int index = (int)((float)i / samples.Length * outputs.Length);
            outputs[index] += samples[i] / ((float)samples.Length / outputs.Length);
        }
    }
}
