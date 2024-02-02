using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.UI;
using System.Collections;

public class Piano : MonoBehaviour
{
    public string audioRoot;
    public bool inThreeBandMode;
    public bool enablePlayerInput;
    [SerializeField]
    private float keyStartTimeOffset;
    [SerializeField]
    private AudioSource low, mid, high;
    private AudioClip[] keys;
    private Dictionary<string, int> keyboardMap, keyboardLowMap, keyboardMidMap, keyboardHighMap;
    private Dictionary<string, AudioSource> keyboardAudioSourceMap;
    private AudioSource[] audioSources;
    private Coroutine[] playCoroutines;


    private void Awake()
    {
        // Load all keys' audio.
        AudioClip[] unorderedKeys = Resources.LoadAll<AudioClip>(audioRoot);
        keys = unorderedKeys.OrderBy(k => int.Parse(k.name[(k.name.LastIndexOf('_') + 1)..])).ToArray();

        // Initialize keyboard-keynote maps.
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

        // Add audio sources mapping each key index.
        audioSources = new AudioSource[keys.Length];
        for (int i = 0; i < keys.Length; i++)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = keys[i];

            // Map key index to audio source.
            audioSources[i] = audioSource;
        }

        // Map input key to audio source.
        keyboardAudioSourceMap = new() { };
        foreach (var kvp in keyboardMap)
        {
            keyboardAudioSourceMap.Add(kvp.Key, audioSources[kvp.Value]);
        }

        // Initialize play coroutines.
        playCoroutines = new Coroutine[keys.Length];
    }

    // Each frame check keyboard input and convert to piano key.
    private void Update()
    {
        if (!enablePlayerInput) return;

        if (inThreeBandMode) ThreeBandModeUpdate();
        else NormalModeUpdate();
    }

    /// <summary>
    /// Enable player input to play the piano or not.
    /// </summary>
    public void SetEnablePlayerInput(bool value)
    {
        enablePlayerInput = value;
    }

    /// <summary>
    /// Play a certain key.
    /// </summary>
    /// <param name="index">the index of the key</param>
    public void Play(int index)
    {
        Stop(index);
        audioSources[index].time = keyStartTimeOffset;
        audioSources[index].Play();
    }

    /// <summary>
    /// Play a certain key for a duration.
    /// </summary>
    /// <param name="index">the index of the key</param>
    /// <param name="duration">the duration of the play</param>
    public void Play(int index, float duration)
    {
        Stop(index);
        var playCoroutine = StartCoroutine(PlayCoroutine(index, duration));
        playCoroutines[index] = playCoroutine;
    }

    /// <summary>
    /// Stop a certain key.
    /// </summary>
    /// <param name="index">the index of the key</param>
    public void Stop(int index)
    {
        audioSources[index].Stop();
        if (playCoroutines[index] != null)
        {
            StopCoroutine(playCoroutines[index]);
            playCoroutines[index] = null;
        }
    }


    /// <summary>
    /// Stop all playing keys.
    /// </summary>
    public void StopAll()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            Stop(i);
        }
    }

    private IEnumerator PlayCoroutine(int index, float duration)
    {
        audioSources[index].time = keyStartTimeOffset;
        audioSources[index].Play();
        yield return new WaitForSeconds(duration);
        Stop(index);
    }

    private void NormalModeUpdate()
    {
        foreach (var kvp in keyboardMap)
        {
            if (Input.GetKeyDown(kvp.Key))
            {
                Play(kvp.Value);
            }
            else if (Input.GetKeyUp(kvp.Key))
            {
                Stop(kvp.Value);
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
