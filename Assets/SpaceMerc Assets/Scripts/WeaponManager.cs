using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    public GameObject currentFirearm;
    private UserInterface userInterface;
    private Firearm firearm;

	// Use this for initialization
	void Start () {
        userInterface = GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>();
        firearm = currentFirearm.GetComponent<Firearm>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        currentFirearm.transform.localRotation = Quaternion.Euler(90f, 0 , 0) * Quaternion.Euler(Camera.main.transform.localRotation.eulerAngles.x, 0, 0);

        if(Input.GetButton
            ("Fire1"))
        {
            firearm.DoFire();
        }

        if (!Input.GetButton("Fire1"))
        {
            firearm.EndFire();
        }

        if (Input.GetButtonDown("Reload"))
        {
            firearm.Reload();
        }

        UpdateUI();

    }

    private void UpdateUI()
    {
        userInterface.SetAmmoCount(firearm.ammoCount);
        userInterface.SetMagazineCount(firearm.magazineCount);

        if(firearm.ammoCount == 0)
        {
            userInterface.SetReloadNeeded(true);
        }
        else
        {
            userInterface.SetReloadNeeded(false);
        }
    }

}
