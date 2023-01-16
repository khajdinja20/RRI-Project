using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Weapon weaponEquipped = null; //oruzje koje smo pokupili
    public Transform FirePoint; //mijesto od kud moraju ici meci

    private float lastFireTime = 0f; //vrijeme od kad smo zadnji puta pucali

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.tag == "Player" && weaponEquipped != null)// ako smo igraci i imamo nekakvo oruzje mozemo pokusati pucati
        {
            if (Input.GetAxis("Fire1") > 0)// gumb je lijevi klik
            {
                float timeSinceLastFire = Time.time - lastFireTime; //racunamo vrijeme od zadnji puta kada smo pucali
                if (timeSinceLastFire > 1 / weaponEquipped.FireRate && weaponEquipped.type != "shotgun")// ako nam firerate dopusta i ako nemamo shotgun nego nesto drugo
                {
                    weaponEquipped.FireWeapon(FirePoint);// pucaj
                    lastFireTime = Time.time;//zapamti kad smo ispucali
                }
                else if (timeSinceLastFire > 1 / weaponEquipped.FireRate)// ako imamo shotgun i firerate nam dopusta
                {
                    weaponEquipped.FireShotgun(FirePoint);//pucaj shotgun
                    lastFireTime = Time.time;//zapamti kad smo ispucali
                }
            }
            if (Input.GetButtonDown("Fire2") && weaponEquipped != null) //gumb je lijevi klik i ako imamo nekakvo oruzje
            {
                GetComponent<PlayerController>().ThrowWeapon(weaponEquipped, FirePoint);//baci oruzje
                weaponEquipped = null;// nemamo vise oruzje jer smo ga bacili 
            }
        }
    }

    public bool EquipWeapon(Weapon weapon)
    {
        if (weaponEquipped == null)
        {
            weaponEquipped = weapon;
            Debug.Log("Equipped: " + weaponEquipped.desc);
            return true;
        }
        else
        {
            Debug.Log("I already have a weapon!");// mogli bi i odsvirati SFX nekakav da igrac zna da vec ima oruzje
            return false;
        }
    }
}
