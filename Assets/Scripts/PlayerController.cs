using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float PlayerSpeed = 10f;
    [SerializeField] float ThrowSpeed;
    [SerializeField] float ThrowRotation;
    Rigidbody2D rgd;

    private float movementHorizontal;
    private float movementVertical;
    // Start is called before the first frame update
    void Start()
    {
        rgd = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTargeting(); //upravlja ciljanjem korisnika
    }

    private void FixedUpdate()
    {
        PlayerMovement(); //upravlja kretanjem
        
    }

    void PlayerMovement()
    {
        movementHorizontal = Input.GetAxisRaw("Horizontal") * PlayerSpeed * Time.deltaTime;
        movementVertical = Input.GetAxisRaw("Vertical") * PlayerSpeed * Time.deltaTime;
        Vector3 playerMovement = new Vector3(movementHorizontal, movementVertical, 0);
        rgd.velocity = playerMovement;
    }
    // Uzimamo poziciju misa te rotiramo naseg igraca prema njemu
    void PlayerTargeting()
    {
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float angle = Mathf.Atan2(positionOnScreen.y - mouseOnScreen.y, positionOnScreen.x - mouseOnScreen.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    public void ThrowWeapon(Weapon weapon, Transform throwPosition)
    {
        // Reaktiviramo nas objekt tako da je samostalan u sceni(nema parent)
        weapon.gameObject.transform.SetParent(null);
        weapon.gameObject.transform.position = throwPosition.position;
        weapon.gameObject.SetActive(true);

        // Dobivamo poziciju misa te racunamo smjer u kojem cemo baciti oruzje
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 throwDirection = (mousePosition -transform.position).normalized;

        // AddForce da bacimo, AddTorque za rotaciju
        weapon.GetComponent<Rigidbody2D>().AddForce(throwDirection * ThrowSpeed, ForceMode2D.Impulse);
        weapon.GetComponent<Rigidbody2D>().AddTorque(ThrowRotation);
    }

    private void OnTriggerStay2D(Collider2D collision)// Koristimo OnTriggerStay2D jer korisnik bi trebao biti "na" oruzju da ga moze pokupiti
    {
        if (Input.GetAxisRaw("Fire3") > 0)// Gumb je middle mouse click
        {
            bool success = this.GetComponent<WeaponHandler>().EquipWeapon(collision.GetComponent<Weapon>()); // Ako uspijemo equippat oruzje, vratimo true
            if (success) //postavljamo objekt "na" nas kako bi nas pratio okolo
            {
                collision.gameObject.transform.position = transform.position;
                collision.gameObject.transform.parent = this.transform;
                collision.gameObject.SetActive(false);
            }
        }
    }
}
