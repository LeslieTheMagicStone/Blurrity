using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.UI;

public class Piano : MonoBehaviour
{
    public string audioRoot;
    public bool inThreeBandMode;
    [SerializeField]
    private float keyStartTimeOffset;
    [SerializeField]
    private AudioSource low, mid, high;
    private AudioClip[] keys;
    private Dictionary<string, int> keyboardMap, keyboardLowMap, keyboardMidMap, keyboardHighMap;
    private Dictionary<string, AudioSource> keyboardAudioSourceMap;


    private void Awake()
    {
        // Load all keys' audio.
        AudioClip[] unorderedKeys = Resources.LoadAll<AudioClip>(audioRoot);
        keys = unorderedKeys.OrderBy(k => int.Parse(k.name[(k.name.LastIndexOf('_') + 1)..])).ToArray();

        // Initialize keyboard-keynote maps
        keyboardLowMap = new Dictionary<string, int>()
        {
            { "z", 12 },
            { "x", 14 },
            { "c", 16 },
            { "v", 17 },
            { "b", 19 },
            { "n", 21 },
            { "m", 23 }
        };

        keyboardMidMap = new Dictionary<string, int>
        {
            { "a", 24 },
            { "s", 26 },
            { "d", 28 },
            { "f", 29 },
            { "g", 31 },
            { "h", 33 },
            { "j", 35 },
            { "k", 36 },
            { "l", 38 }
        };

        keyboardHighMap = new Dictionary<string, int>
        {
            { "q", 48 },
            { "w", 50 },
            { "e", 52 },
            { "r", 53 },
            { "t", 55 },
            { "y", 57 },
            { "u", 59 },
            { "i", 60 },
            { "o", 62 }
        };

        keyboardMap = new Dictionary<string, int>() { };
        foreach (var kvp in keyboardLowMap)
            keyboardMap.Add(kvp.Key, kvp.Value);
        foreach (var kvp in keyboardMidMap)
            keyboardMap.Add(kvp.Key, kvp.Value);
        foreach (var kvp in keyboardHighMap)
            keyboardMap.Add(kvp.Key, kvp.Value);

        // Add audio sources mapping each key
        keyboardAudioSourceMap = new() { };
        foreach (var kvp in keyboardMap)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = keys[kvp.Value];
            keyboardAudioSourceMap.Add(kvp.Key, audioSource);
        }


    }

    // Each frame check keyboard input and convert to piano key.
    private void Update()
    {
        if (inThreeBandMode) ThreeBandModeUpdate();
        else NormalModeUpdate();
    }


    private void NormalModeUpdate()
    {
        foreach (var kvp in keyboardMap)
        {
            var audioSource = keyboardAudioSourceMap[kvp.Key];

            if (Input.GetKeyDown(kvp.Key))
            {
                audioSource.time = keyStartTimeOffset;
                audioSource.Play();
            }
            else if (Input.GetKeyUp(kvp.Key))
            {
                audioSource.Stop();
            }
        }
    }

    private void ThreeBandModeUpdate()
    {
        foreach (var kvp in keyboardLowMap)
        {
            if (Input.GetKeyDown(kvp.Key))
            {
                low.clip = keys[kvp.Value];
                low.time = keyStartTimeOffset;
                low.Play();
            }
            if (Input.GetKeyUp(kvp.Key))
            {
                low.Stop();
            }
        }

        foreach (var kvp in keyboardMidMap)
        {
            if (Input.GetKeyDown(kvp.Key))
            {
                mid.clip = keys[kvp.Value];
                mid.time = keyStartTimeOffset;
                mid.Play();
            }
            if (Input.GetKeyUp(kvp.Key))
            {
                mid.Stop();
            }
        }

        foreach (var kvp in keyboardHighMap)
        {
            if (Input.GetKeyDown(kvp.Key))
            {
                high.clip = keys[kvp.Value];
                high.time = keyStartTimeOffset;
                high.Play();
            }
            if (Input.GetKeyUp(kvp.Key))
            {
                high.Stop();
            }
        }
    }


}
