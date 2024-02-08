using UnityEngine;
using TMPro;

public class CompletenessText : MonoBehaviour
{
    public CompletenessInfo info;
    public TMP_Text tmp_Text;

    private void Update()
    {
        tmp_Text.text = info.completenessString;
    }
}
