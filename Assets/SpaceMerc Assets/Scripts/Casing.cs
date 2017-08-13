using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour {

    public float speed;
    private Rigidbody casingRigidBody;
    public AudioSource groundSound;

    // Use this for initialization
    void Start()
    {
        casingRigidBody = GetComponent<Rigidbody>();
        casingRigidBody.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(casingRigidBody && casingRigidBody.velocity.magnitude < float.Epsilon)
        {
            Destroy(casingRigidBody);
            gameObject.tag = "DeadCasing";
            enabled = false;
            
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Casing" || col.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(col.collider, GetComponent<Collider>());
        }

        if (col.gameObject.tag == "Environment")
        {
            groundSound.Play();
        }

    }
}
