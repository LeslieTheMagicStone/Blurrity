using System.Linq;
using UnityEngine;

public class MusicWaveInstantiater : MonoBehaviour
{
    public bool useBuffer;
    public bool useAbsolute;
    public float maxHeight;
    public float waveDistance;
    [SerializeField] private Rigidbody2D wavePrefab;
    [SerializeField] private float waveIntervalTime;
    [SerializeField] private Transform spawnPoint;
    private float waveIntervalTimer;

    private float[] bands;


    private void FixedUpdate() {
        waveIntervalTimer += Time.deltaTime;

        if (waveIntervalTimer < waveIntervalTime) return;

        waveIntervalTimer = 0f;
        
        if (!useAbsolute)
            bands = useBuffer ? BandSource.bufferedRelativeBands : BandSource.rawRelativeBands;
        else
            bands = useBuffer ? BandSource.bufferedBands : BandSource.rawBands;

        var amplitude = bands.Average();
        var height = amplitude * maxHeight;

        var wave = Instantiate(wavePrefab);
        wave.velocity = Vector3.left * waveDistance / waveIntervalTime;
        wave.transform.localScale = new(wave.transform.localScale.x, height, wave.transform.localScale.z);
        wave.transform.position = spawnPoint.position;
        wave.transform.SetParent(transform);

        Destroy(wave.gameObject, 100 * waveIntervalTime);
    }
}
