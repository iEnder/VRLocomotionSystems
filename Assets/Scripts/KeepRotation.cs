using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepRotation : MonoBehaviour
{
     public float speed = 5f;

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * speed);
    }

}
