using UnityEngine;

public class SoundVisualizer : MonoBehaviour
{
    public AudioSource audioSource; // 音频源
    public GameObject visualizerPrefab; // 可视化器预制体
    public float spawnDistance = 1f; // 可视化器生成距离

    private GameObject[] visualizers; // 可视化器对象数组

    private void Start()
    {
        int sampleCount = 256; // 可视化器数量和音频样本数量

        // 创建可视化器对象数组
        visualizers = new GameObject[sampleCount];

        // 实例化可视化器对象
        for (int i = 0; i < sampleCount; i++)
        {
            GameObject visualizer = Instantiate(visualizerPrefab, transform);
            visualizer.transform.localPosition = i * spawnDistance * Vector3.right;
            visualizers[i] = visualizer;
        }
    }

    private void Update()
    {
        // 获取音频样本数据
        float[] samples = new float[visualizers.Length];
        AudioListener.GetSpectrumData(samples, 0, FFTWindow.Rectangular);

        // 更新可视化器的高度
        for (int i = 0; i < visualizers.Length; i++)
        {
            Vector3 scale = visualizers[i].transform.localScale;
            scale.y = Mathf.Lerp(scale.y, samples[i] * 10f, Time.deltaTime * 10f);
            visualizers[i].transform.localScale = scale;
        }
    }
}