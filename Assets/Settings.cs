using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

	void Start () {
        Physics.IgnoreLayerCollision(8, 9); // ignore collisons between casings and player

	}
	
}
