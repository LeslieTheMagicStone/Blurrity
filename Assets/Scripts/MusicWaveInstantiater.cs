using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicWaveInstantiater : MonoBehaviour
{
    public bool useBuffer;
    public bool useAbsolute;
    public float maxHeight;
    public float waveDistance;
    public Level1 level;
    public BandSource masterSource, playerSource;

    [SerializeField] private Rigidbody2D wavePrefab, playersWavePrefab;
    [SerializeField] private Transform spawnPoint;
    private BeatType beatType => level.beatType;
    private float bpm => level.bpm;
    private float waveIntervalTime => GetWaveIntervalTime();
    private float waveIntervalTimer;
    private int waveCount;
    private float[] bands;
    private bool isPlayersBeat => level.IsPlayersBeat;


    private void FixedUpdate()
    {
        waveIntervalTimer += Time.deltaTime;

        int newWaveCount = (int)(waveIntervalTimer / waveIntervalTime);
        if (newWaveCount <= waveCount) return;

        waveCount = newWaveCount;

        BandSource bandSource = isPlayersBeat ? playerSource : masterSource;

        if (!useAbsolute)
            bands = useBuffer ? bandSource.bufferedRelativeBands : bandSource.rawRelativeBands;
        else
            bands = useBuffer ? bandSource.bufferedBands : bandSource.rawBands;

        var amplitude = bands.Average();
        var height = amplitude * maxHeight + 1;

        var wave = Instantiate(isPlayersBeat ? playersWavePrefab : wavePrefab);
        wave.velocity = Vector3.left * waveDistance / waveIntervalTime;
        wave.transform.localScale = new(wave.transform.localScale.x, height, wave.transform.localScale.z);
        wave.transform.position = spawnPoint.position;
        wave.transform.SetParent(transform);

        Destroy(wave.gameObject, 100 * waveIntervalTime);
    }



    private float GetWaveIntervalTime()
    {
        return beatType switch
        {
            BeatType.FourFour => 7.5f / bpm,
            BeatType.TwoTwo => 15 / bpm,
            BeatType.ThreeFour => 10 / bpm,
            _ => 30 / bpm,
        };
    }
}
