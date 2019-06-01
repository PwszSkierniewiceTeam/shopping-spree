using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platforms : MonoBehaviour
{
    public float speed = 2f;
    //public bool platforms_moving = false;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(platforms_moving);

        //if (platforms_moving == true){
            transform.Translate(new Vector3(0, 1, 0) * speed * Time.deltaTime);
        //}

    }
}
