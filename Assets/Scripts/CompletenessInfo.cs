using UnityEngine;

public class CompletenessInfo : MonoBehaviour
{
    public InputComparer inputComparer;

    public int curScore;
    public int maxScore => inputComparer.inputCount * perfectScore;
    public float completeness => (float)curScore / maxScore;
    public string completenessString => (completeness * 100f).ToString("0.00") + " %";
    public int badScore = 0;
    public int lateScore = 10;
    public int earlyScore = 10;
    public int perfectScore = 15;

    private void Start()
    {
        // Linear completeness calculation.
        // Bad Early/Late Perfect
        //  0      10       15
        inputComparer.OnBad.AddListener(() => curScore += badScore);
        inputComparer.OnLate.AddListener(() => curScore += lateScore);
        inputComparer.OnEarly.AddListener(() => curScore += earlyScore);
        inputComparer.OnPerfect.AddListener(() => curScore += perfectScore);
    }

    private void Update()
    {
        Debug.Log(completeness.ToString());
        Debug.Log(maxScore.ToString());
    }


}
