using UnityEngine;

public class CapsuleInstantiater : MonoBehaviour
{
    public float maxHeight;
    public bool useBuffer;
    [SerializeField]
    private Transform capsulePrefab;
    private Transform[] capsules;

    private void Start()
    {
        var len = BandSource.rawBands.Length;
        capsules = new Transform[len];

        for (int i = 0; i < len; i++)
        {
            var capsule = Instantiate(capsulePrefab);
            capsule.SetParent(transform);
            float posX = (float)i * 10 / len - 5;
            capsule.localPosition = new Vector3(posX, 0, 0);
            capsule.localScale = new Vector3((float)10 / len, 1, 1);
            capsules[i] = capsule;
        }
    }

    private void Update()
    {
        for (int i = 0; i < capsules.Length; i++)
        {
            if (capsules[i] == null) continue;

            var output = useBuffer ? BandSource.bufferedRelativeBands[i] : BandSource.rawRelativeBands[i];
            var height = output * maxHeight;
            var origScale = capsules[i].localScale;
            capsules[i].localScale = new Vector3(origScale.x, height, origScale.z);
            var origPos = capsules[i].localPosition;
            capsules[i].localPosition = new Vector3(origPos.x, height / 2, origPos.z);
        }
    }


}
