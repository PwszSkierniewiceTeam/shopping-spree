using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController : MonoBehaviour
{
    public event EventHandler<Collider2D> Hit;
    public event EventHandler Stoped;
    protected virtual void OnHit(Collider2D collision)
    {
        Hit?.Invoke(this, collision);
    }
    protected virtual void OnStoped()
    {
        Stoped?.Invoke(this, EventArgs.Empty);
    }
    void Update()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Killz"))
        {
            transform.Rotate(new Vector3(0, 0, -180) * Time.deltaTime);
        }
        else
        {
            transform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime);
        }
    }
    private void FixedUpdate()
    {
        if (GetComponent<Rigidbody2D>().velocity.magnitude <1)
        {
            OnStoped();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnHit(collision);
    }
}
