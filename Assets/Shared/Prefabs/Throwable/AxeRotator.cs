using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeRotator : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, -180) * Time.deltaTime);
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    Destroy(this);
    //}
}
