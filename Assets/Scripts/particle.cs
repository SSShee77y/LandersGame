using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle : MonoBehaviour
{   
    public static particle instance;

    public float lifetime = -999;

    public void Awake()
    {
        instance = this;
    }

    void OnCollisionEnter(Collision other)
    {   
        if (other.gameObject.tag == "Player"){
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
        }
    }

    void FixedUpdate()
    {
        if (lifetime > -999) lifetime-= 0.1f;
        if (lifetime <= 0 && lifetime > -999) Destroy(gameObject);
    }

    public void randomLifetime() 
    {
        lifetime = Random.Range(8f, 12f);
    }

    public void randomLifetime(float addTime) 
    {
        lifetime = addTime + Random.Range(8f, 12f);
    }



}
