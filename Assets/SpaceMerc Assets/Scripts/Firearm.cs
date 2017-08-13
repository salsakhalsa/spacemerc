using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firearm : MonoBehaviour
{

    public GameObject projectile;
    public GameObject casing;
    public GameObject muzzleFlash;
    public Transform projectileSpawn;
    public Transform[] casingSpawns;
    public Transform muzzleFlashLocation;
    public AudioSource fireSound;
    public float accuracy;
    public float roundsPerSecond; // rounds per second

    public uint ammoCount;
    public uint magazineCount;
    public uint magazineMax;
    public float reloadTime;

    
    private bool doFire = false;
    private bool isReloading = false;
    private bool isFirstShot = false;
    private float nextShootTime;

    // Use this for initialization
    void Start()
    {
        muzzleFlash = Instantiate(muzzleFlash, muzzleFlashLocation.position, muzzleFlashLocation.transform.rotation, transform);
        muzzleFlash.SetActive(false);


        nextShootTime = Time.time + roundsPerSecond;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoFire()
    {

        doFire = true;
        if (isFirstShot)
        {
            isFirstShot = false;
            nextShootTime = Time.time + roundsPerSecond;
        }
        FireProjectile();
    }

    public void EndFire()
    {
        doFire = false;
        isFirstShot = true;
        muzzleFlash.SetActive(false);
        

    }

    public void Reload()
    {
        if (!isReloading && magazineCount > 0)
        {
            isReloading = true;
            StartCoroutine(DoReload());
        }
    }

    private void FireProjectile()
    {

        bool doPauseForFlash = (1f / roundsPerSecond) - (1f / 20f) > 0 ? true : false;
        Debug.Log(Time.time > nextShootTime);
        while (doFire && ammoCount > 0 && !isReloading && Time.time > nextShootTime)
        {
            

            Instantiate(projectile, projectileSpawn.position, 
                projectileSpawn.transform.rotation * Quaternion.Euler(Random.Range(0, accuracy), Random.Range(0, accuracy), Random.Range(0, accuracy)));

            fireSound.Play();

            muzzleFlash.transform.rotation *= Quaternion.Euler(0f, 0f, Mathf.Floor(Random.Range(1, 2)) * 90f);
            muzzleFlash.SetActive(true);
            if (doPauseForFlash)
            {
                muzzleFlash.SetActive(false);
            }

            if(ammoCount > 0)
                ammoCount--;
            Transform casingSpawn = casingSpawns[(int)Mathf.Floor(Random.Range(0, casingSpawns.Length))];
            Instantiate(casing, casingSpawn.position, casingSpawn.localRotation * Quaternion.Euler(Random.Range(0,5), Random.Range(0, 5), Random.Range(0, 5)));
            nextShootTime += roundsPerSecond;
        }
        muzzleFlash.SetActive(false);
    }

    private IEnumerator DoReload()
    {
            yield return new WaitForSeconds(reloadTime);
            ammoCount = magazineMax;
            magazineCount--;

            isReloading = false;
    }

}
