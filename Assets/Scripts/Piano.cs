using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Piano : MonoBehaviour
{
    public string audioRoot;
    [SerializeField]
    private float keyStartTimeOffset;
    [SerializeField]
    private AudioSource low, mid, high;
    private AudioClip[] keys = new AudioClip[88];
    private Dictionary<string, int> keyboardLowMap, keyboardMidMap, keyboardHighMap = new() { };



    private void Awake()
    {
        // Load all keys' audio.
        AudioClip[] unorderedKeys = Resources.LoadAll<AudioClip>(audioRoot);
        keys = unorderedKeys.OrderBy(k => int.Parse(k.name[(k.name.LastIndexOf('_') + 1)..])).ToArray();

        // Initialize keyboard-keynote maps
        keyboardLowMap = new Dictionary<string, int>()
        {
            { "z", 0 },
            { "x", 2 },
            { "c", 4 },
            { "v", 5 },
            { "b", 7 },
            { "n", 9 },
            { "m", 11 }
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



    }

    // Each frame check keyboard input and convert to piano key.
    private void Update()
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
