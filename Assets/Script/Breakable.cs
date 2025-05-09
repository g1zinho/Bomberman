using UnityEngine;
using System;

public class Breakable : MonoBehaviour
{

    protected virtual void Explode()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Explosion"))
        {
            Explode();

        }
    }




}
