using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InputFeedbackFrame : MonoBehaviour
{
    [SerializeField]
    private InputComparer inputComparer;
    [SerializeField]
    private GameObject earlyFramePrefab, lateFramePrefab, perfectFramePrefab, badFramePrefab;

    private void Start()
    {
        inputComparer.OnEarly.AddListener(() => ShowFrame(earlyFramePrefab));
        inputComparer.OnLate.AddListener(() => ShowFrame(lateFramePrefab));
        inputComparer.OnPerfect.AddListener(() => ShowFrame(perfectFramePrefab));
        inputComparer.OnBad.AddListener(() => ShowFrame(badFramePrefab));
    }

    private void ShowFrame(GameObject framePrefab)
    {
        StartCoroutine(ShowFrameCoroutine(framePrefab));
    }

    private IEnumerator ShowFrameCoroutine(GameObject framePrefab)
    {
        GameObject canvas = Instantiate(framePrefab);
        Image frame = canvas.GetComponentInChildren<Image>();
        frame.GetComponent<CanvasRenderer>().SetAlpha(0);

        // Fade in.
        float fadeInTime = 0.3f;
        frame.CrossFadeAlpha(255f, fadeInTime, false);
        yield return new WaitForSeconds(fadeInTime);

        // Wait.
        float waitTime = 0.2f;
        yield return new WaitForSeconds(waitTime);

        // Fade out.
        float fadeOutTime = 0.3f;
        frame.CrossFadeAlpha(0f, fadeOutTime, false);
        yield return new WaitForSeconds(fadeOutTime);

        // Destroy.
        Destroy(canvas);
    }

}
