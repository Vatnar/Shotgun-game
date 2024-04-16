using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector2 offset = new Vector2(0,0);
    [SerializeField] private float period = 1f;
    Vector2 startPosition;
    float t = 0;
    bool up = true;
    void Start()
    {
        startPosition = transform.position;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Vector3 scale = new Vector3(transform.localScale.x*3, transform.localScale.y / 2, 0);
        Gizmos.DrawCube(transform.position+(Vector3)offset, scale);
    }

    void Update()
    {

        if (t > period || !up)
        {
            t -= Time.deltaTime;
            up = false;
        }
        else {t += Time.deltaTime;}

        if (t < 0) { up = true;}


        transform.position = Vector2.Lerp(startPosition, startPosition+offset, t/period);

    }
}
