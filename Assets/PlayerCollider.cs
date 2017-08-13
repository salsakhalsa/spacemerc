using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
       

        if (collision.gameObject.tag == "Casing" || collision.gameObject.tag == "DeadCasing" || collision.gameObject.tag == "Projectile")
        {
            if (collision.gameObject.tag == "Casing")
                Debug.Log("CASING!");
            //Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
