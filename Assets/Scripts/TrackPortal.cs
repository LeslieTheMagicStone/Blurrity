using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackPortal : MonoBehaviour
{
    public DisplayType displayType;
    public string trackDataPath;

    // Time window for detecting double click.
    public float doubleClickTime = 0.3f;

    // Track the click count and timer.
    private int clickCount = 0;
    private float clickTimer = 0f;

    private void OnMouseDown()
    {
        // Check for mouse button down
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;

            // Check if it's the first click or a subsequent click
            if (clickCount == 1)
            {
                // Start the timer for double click detection
                clickTimer = Time.time;
            }
            else if (clickCount == 2 && Time.time - clickTimer <= doubleClickTime)
            {
                // Double click detected
                clickCount = 0;
                LoadScene();
            }
            else
            {
                // Reset click count and timer
                clickCount = 1;
                clickTimer = Time.time;
            }
        }
    }

    private void LoadScene()
    {
        switch (displayType)
        {
            case DisplayType.EightBars:
                SceneManager.LoadScene("Eightbars");
                break;
        }
    }

}
