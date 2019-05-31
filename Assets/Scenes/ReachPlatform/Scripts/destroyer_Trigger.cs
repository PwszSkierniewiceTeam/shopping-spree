using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyer_Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("zderzylem sie 0");
        if (collision.gameObject.tag == "platform_stuff" || collision.gameObject.tag == "enemies")
        {
            Destroy(collision.gameObject);
        }
    }



}
