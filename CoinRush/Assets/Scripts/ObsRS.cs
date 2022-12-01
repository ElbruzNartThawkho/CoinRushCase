using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsRS : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<Animator>().speed = Random.Range(0.1f, 0.8f);
    }
}
