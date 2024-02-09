using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Floatter : MonoBehaviour
{
    private float amplitude = 0.1f;
    private float frequency = 0.7f;

    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    void Start()
    {
        posOffset = transform.position;
    }

    void Update()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}
