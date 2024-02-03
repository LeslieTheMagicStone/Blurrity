using System.IO;
using UnityEngine;

public class InputLogger : MonoBehaviour
{
    public bool clearOnPlay;
    [SerializeField]
    private string filePath;

    private void Start()
    {
        if (clearOnPlay)
        {
            File.WriteAllText(filePath, string.Empty);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            var input = Input.inputString;
            LogInput(Time.time.ToString());
        }
    }

    private void LogInput(string input)
    {
        using var file = new StreamWriter(filePath, true);
        file.WriteLine(input);
    }
}
