using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager instance;

    public string trackData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("Multiple Game Managers coexist!");
            Destroy(this);
        }
    }
}
