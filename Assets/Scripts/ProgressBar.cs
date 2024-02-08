using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public AudioSource target;
    public Image bar;

    private void Update()
    {
        float fill = target.time / target.clip.length * Screen.width;
        bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fill);
    }
}
