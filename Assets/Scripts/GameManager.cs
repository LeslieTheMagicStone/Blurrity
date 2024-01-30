using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject Bubble;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            var bubble = Instantiate(Bubble);
            bubble.transform.position = new(Random.Range(-5, 5), Random.Range(3, 4), 0);
            var randomScale = Random.Range(0.9f, 1.2f);
            bubble.transform.localScale = new(randomScale, randomScale, 1);
        }
    }
}
