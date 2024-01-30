using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class CapsuleInstantiater : MonoBehaviour
{
    [SerializeField]
    private Transform capsulePrefab;
    [SerializeField]
    private float maxHeight = 5;
    private Transform[] capsules = new Transform[512];

    private void Start()
    {
        var len = AudioVisualizer.outputs.Length;

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

            var output = AudioVisualizer.outputs[i];
            var height = output * maxHeight;
            var origScale = capsules[i].localScale;
            capsules[i].localScale = new Vector3(origScale.x, height, origScale.z);
            var origPos = capsules[i].localPosition;
            capsules[i].localPosition = new Vector3(origPos.x, height / 2, origPos.z);
        }
    }
}
