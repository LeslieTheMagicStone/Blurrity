using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    [SerializeField] private GameObject bubble;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            var bubble = Instantiate(this.bubble);
            bubble.transform.position = new(Random.Range(-5, 5), Random.Range(3, 4), 0);
            var randomScale = Random.Range(0.9f, 1.2f);
            bubble.transform.localScale = new(randomScale, randomScale, 1);
        }
    }
}
