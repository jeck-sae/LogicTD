using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOverTime : MonoBehaviour
{
    public Vector3 speed = Vector3.right;

    void Update()
    {
        transform.position += speed * Time.deltaTime;
    }
}