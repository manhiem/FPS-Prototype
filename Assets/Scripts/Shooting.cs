using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    RaycastHit hit;

    public Text textCurrentAmmo;
    public Text textCarriedAmmo;
    public int currentAmmo = 6;
    public int maxAmmo = 6;
    public int carriedAmmo = 30;
    bool isReloading;


    [SerializeField]
    Transform shootPoint;

    //Rate of Fire
    [SerializeField]
    float rateOffFire;
    float nextFire = 0;

    [SerializeField]
    float weaponRange;

    //To damage Enemy
    [SerializeField]
    float damage;

    //Weapon Effects
    public ParticleSystem muzzleFlash;

    //Blood
    public GameObject bloodEffect;

    //Stone
    public GameObject stoneEffect;

    //Shoot Audio
    AudioSource gunAS;
    public AudioClip shootAC;
    public AudioClip dryFireAC;

    Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();
        muzzleFlash.Stop();
        gunAS = GetComponent<AudioSource>();
        UpdateAmmoUI();
    }
    private void Update()
    {
        if(Input.GetMouseButton(0) && currentAmmo>0 )
        {
            shoot();
        }
        else if (Input.GetMouseButton(0) && currentAmmo <= 0 && !isReloading)
        {
            DryFire();
        }
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo <= maxAmmo && !isReloading)
        {
            isReloading = true;
            Reload();
        }

    }

    void DryFire()
    {
        if (Time.time > nextFire)
        {
            nextFire = 0f;

            nextFire = Time.time + rateOffFire;

            //ADD DRY FIRE ANIM

            gunAS.PlayOneShot(dryFireAC);
            Debug.Log("Dry Firing");
        }
    }
    public void shoot()
    {
        if(Time.time > nextFire)
        {
            nextFire = 0f;
            nextFire = Time.time + rateOffFire;
            anim.SetTrigger("Shoot");

            currentAmmo--;

            gunAS.PlayOneShot(shootAC);
            StartCoroutine(WeaponEffects());

            if(Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponRange))
            {
                if(hit.transform.tag == "Enemy")
                {
                    //Debug.Log("HitEnemy");
                    EnemyHealth enemyHealthScript = hit.transform.GetComponent<EnemyHealth>();
                    enemyHealthScript.DeductHealth(damage);
                    Instantiate(bloodEffect, hit.point, Quaternion.identity);
                }
                else
                {
                    Instantiate(stoneEffect, hit.point, Quaternion.identity);
                }
            }
        }

        UpdateAmmoUI();
    }

    IEnumerator ReloadCountdown(float timer)
    {
        while(timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        if(timer <= 0f)
        {
            int bulletsNeedToFillMag = maxAmmo - currentAmmo;
            int bulletsToDeduct = (carriedAmmo >= bulletsNeedToFillMag) ? bulletsNeedToFillMag : carriedAmmo;

            carriedAmmo -= bulletsToDeduct;
            currentAmmo += bulletsToDeduct;
            isReloading = false;
            UpdateAmmoUI();
        }
    }

    void UpdateAmmoUI()
    {
        textCurrentAmmo.text = currentAmmo.ToString();
        textCarriedAmmo.text = carriedAmmo.ToString();
    }

    void Reload()
    {
        if (carriedAmmo <= 0) return;
        anim.SetTrigger("Reload");
        StartCoroutine(ReloadCountdown(2f));
    }

    IEnumerator WeaponEffects()
    {
        muzzleFlash.Play();
        yield return new WaitForEndOfFrame();
        muzzleFlash.Stop();
    }
}
