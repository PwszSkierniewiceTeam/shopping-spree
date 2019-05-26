using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform : MonoBehaviour
{
    public float speed = 4f;
    //public bool platforms_moving = false;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(platforms_moving);

        //if (platforms_moving == true){
            transform.Translate(new Vector3(-1, 0, 0) * speed * Time.deltaTime);
        //}

    }
}
