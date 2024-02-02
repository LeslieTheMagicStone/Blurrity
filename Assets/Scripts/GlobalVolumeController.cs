using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolumeController : MonoBehaviour
{
    public BandSource bandSource;
    public bool useBuffer;
    public float maxBloomIntensity;
    private Bloom bloom;

    private void Awake()
    {
        var globalVolume = GetComponent<Volume>();
        globalVolume.profile.TryGet(out bloom);

    }

    private void Update()
    {
        var relativeBands = useBuffer ? bandSource.bufferedRelativeBands : bandSource.rawRelativeBands;
        var intensity = relativeBands.Average() * maxBloomIntensity;
        bloom.intensity.Override(intensity);
    }
}
