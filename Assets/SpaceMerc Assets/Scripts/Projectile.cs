using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed;
    private Rigidbody projectile;
    

	// Use this for initialization
	void Start () {
        projectile = GetComponent<Rigidbody>();
        projectile.velocity = transform.up * speed;
        Destroy(gameObject, 20f);

	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Casing" || col.gameObject.tag == "DeadCasing" || col.gameObject.tag == "Projectile")
        {
            Physics.IgnoreCollision(col.collider, GetComponent<Collider>());
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
