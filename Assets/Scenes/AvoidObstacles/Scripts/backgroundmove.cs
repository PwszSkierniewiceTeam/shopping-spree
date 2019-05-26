using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundmove : MonoBehaviour
{
    public float speed; //speed of moving texture
    Vector2 offset; //opposite by which we are going to move the texture on the quad 


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        offset = new Vector2(Time.time * speed, 0);
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}
