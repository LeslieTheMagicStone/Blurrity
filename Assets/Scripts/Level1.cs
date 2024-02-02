using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Level1 : MonoBehaviour
{
    public BeatType beatType => GetBeatType();
    public float bpm => GetBpm();
    public int[] playersBeats => GetPlayersBeats();
    public bool IsPlayersBeat => GetIsPlayersBeat();

    [SerializeField]
    private Piano playerPiano, computerPiano;
    [SerializeField]
    private float _bpm;
    [SerializeField]
    private BeatType _beatType;

    private float beatIntervalTime => GetBeatIntervalTime();
    private float beatTimer;
    private int beatCount;
    private bool isPlayersBeat;

    private void Start()
    {
        computerPiano.SetEnablePlayerInput(false);
    }

    private void FixedUpdate()
    {
        beatTimer += Time.deltaTime;
        int newBeatCount = (int)(beatTimer / beatIntervalTime);

        if (newBeatCount <= beatCount) return;

        beatCount = newBeatCount;

        if (playersBeats.Contains(beatCount))
        {
            isPlayersBeat = true;
            playerPiano.SetEnablePlayerInput(true);
        }
        else
        {
            isPlayersBeat = false;
            playerPiano.SetEnablePlayerInput(false);
            computerPiano.Play(Random.Range(0, 88), 0.5f);
        }
    }

    private bool GetIsPlayersBeat()
    {
        return isPlayersBeat;
    }

    private float GetBeatIntervalTime()
    {
        return beatType switch
        {
            BeatType.FourFour => 15 / bpm,
            BeatType.TwoTwo => 30 / bpm,
            BeatType.ThreeFour => 20 / bpm,
            _ => 60 / bpm,
        };
    }

    private BeatType GetBeatType()
    {
        return _beatType;
    }

    private float GetBpm()
    {
        return _bpm;
    }

    private int[] GetPlayersBeats()
    {
        int length = 100;
        List<int> ans = new();

        for (int i = 0; i < length; i++)
        {
            ans.Add(4 * i);
        }

        return ans.ToArray();
    }
}
