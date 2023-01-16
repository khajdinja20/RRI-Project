using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject BulletPrefab; //sto pucamo
    public string type; //kakvo imamo oruzje
    public float BulletSpeed; //koliko brzo metci putuju kroz svijet
    public float FireRate; //koliko brzo mozemo metke pucati
    [SerializeField] int bullets; //koliko metka pucamo sa jednim "klikom"
    [SerializeField] float spread; //koliko puno se meci sire 
    [SerializeField] int magazine; //koliko metaka ima puska(nemamo reloading)

    //za random generiranje brojka kod brzine metaka(FireShotgun)
    [SerializeField] float minRandom;
    [SerializeField] float maxRandom;
    //za random generiranje brojka kod spreada metaka(FireShotgun)
    [SerializeField] float maxSpreadRandom;
    [SerializeField] float minSpreadRandom;
    public string desc; //kratak opis same puske, flavour text

    //Funkcija koja ustvari puca nas metak tamo gdje je mis postavljen
    public void FireWeapon(Transform FirePoint)
    {
        if (magazine > 0)//ako puska nije prazna
        {
            float spreadRadians = spread * Mathf.Deg2Rad;//izracunaj koliko se metci moraju rasprsiti, pretvorba u radijane iz stupnja
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-spreadRadians, spreadRadians));//dobi potrebnu rotaciju

            Vector3 mouseOnScreen = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);//dobi poziciju misa

            GameObject bullet = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);//kreiraj metak tamo gdje je nasa pozicija za pucanje(saljemo je kroz funkciju)
            Vector3 fireDirection = rotation * (mouseOnScreen - transform.position).normalized;//dobi smjer u kojem pucamo, mis

            bullet.GetComponent<Rigidbody2D>().velocity = fireDirection * BulletSpeed;//dodaj brzinu metku
            magazine--;//jedan metak manje
        }
        else//prazni smo
        {
            Debug.Log("Out of ammo!"); //Najbolje bi bilo dodati SFX kako bi igrac znao da nema municije
        }
    }

    public void FireShotgun(Transform FirePoint)
    {
        if(magazine > 0)
        {
            float spreadRadians = spread * Mathf.Deg2Rad; // konverzija u radijane za spread
            float spreadIncrement = spreadRadians / (bullets - 1); //izracinamo koliko moramo povecavati spread za broj metaka

            List<Quaternion> rotations = new List<Quaternion>(); //kreiramo listu rotacija koje cemo primjenjivati
            for(int i = 0; i< bullets; i++)
            {
                float angle = spreadIncrement * i - spreadRadians /2 *Random.Range(minSpreadRandom, maxSpreadRandom);//izracunaj kut koji treba primjeniti
                rotations.Add(Quaternion.Euler(0,0,angle));//dodaj ga u listu
            }

            rotations = rotations.OrderBy(x => Random.value).ToList(); //malo ih promjesamo kako bi pucanje bilo zanimljivije

            foreach (Quaternion rotation in rotations)//pucamo metke, isto kao i kod prve metode
            {
                Vector3 direction = rotation * FirePoint.up;

                GameObject bullet = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);

                bullet.GetComponent<Rigidbody2D>().velocity = direction* BulletSpeed * Random.Range(minRandom, maxRandom);//random je tu za brzine metaka
            }
            magazine--;
        }
        else//prazni smo
        {
            Debug.Log("Out of ammo!"); //Najbolje bi bilo dodati SFX kako bi igrac znao da nema municije
        }
    }
}
