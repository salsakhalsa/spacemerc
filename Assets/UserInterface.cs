using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserInterface : MonoBehaviour {

    public TextMeshProUGUI ammo;
    public TextMeshProUGUI magazine;
    public TextMeshProUGUI reload;
    public TextMeshProUGUI outOfAmmo;

    public float fadeRate;

    private uint ammoCount = 0;
    private uint magazineCount = 0;
    private bool reloadNeeded = false;
    private bool isAdding = false;


	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        SetAmmoText();
        SetMagazineText();

        if (ammoCount == 0 && magazineCount == 0)
        {
            DoOutOfAmmo();
        }
        else if(reloadNeeded)
        {
            DoReloadNeeded();
        }
        else
        {
            DoNormalStatus();
        }

    }

    public void SetMagazineCount(uint count)
    {
        magazineCount = count;
    }

    public void SetAmmoCount(uint count)
    {
        ammoCount = count;
    }

    private void SetAmmoText()
    {
        string ammoCountText = " 00";

        if (ammoCount < 10)
            ammoCountText = " 0" + ammoCount;
        else if (ammoCount < 100)
            ammoCountText = " " + ammoCount;
        else
            ammoCountText =  ammoCount.ToString();

        ammo.text = ammoCountText;
    }

    private void SetMagazineText()
    {
        if(magazineCount > 0)
            magazine.text = "   /" + magazineCount;
        else
            magazine.text = "   /" + 0;
    }

    public void SetReloadNeeded(bool isReloadNeeded)
    {
        reloadNeeded = isReloadNeeded;
    }

    private void DoReloadNeeded()
    {
        ammo.faceColor = magazine.faceColor = reload.faceColor = new Color(1f, 0f, 0f, GetChangedAlpha() / 255f);
        reload.enabled = true;
        outOfAmmo.enabled = false;
    }

    private void DoNormalStatus()
    {
        ammo.faceColor = magazine.faceColor = new Color(1f, 1f, 1f, 1f);
        reload.enabled = outOfAmmo.enabled = false;
    }

    private void DoOutOfAmmo()
    {
        ammo.faceColor = magazine.faceColor = outOfAmmo.faceColor = new Color(1f, 0f, 0f, GetChangedAlpha() / 255f);
        reload.enabled = false;
        outOfAmmo.enabled = true;

    }

    private float GetChangedAlpha()
    {
        float alpha = ammo.faceColor.a; ;
        if (isAdding)
        {
            alpha = alpha + Time.deltaTime * fadeRate * 50;
        }
        else
        {
            alpha = alpha - Time.deltaTime * fadeRate * 50;
        }

        if ( alpha > 255f)
        {
            alpha = 255f;
            isAdding = false;
        }

        if (alpha < 5f)
        {
            alpha = 0f;
            isAdding = true;
        }

        //Debug.Log(Time.deltaTime * fadeRate);

        return alpha;
    }
}
