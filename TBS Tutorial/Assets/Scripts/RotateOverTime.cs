using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public float rotateSpeed = 50f;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + (rotateSpeed*Time.deltaTime), 0f);
    }
}
