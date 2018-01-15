using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]

public class Player : MonoBehaviour
{
    public int Health = 30;

    public Rigidbody MyBody;
    public float Speed;
    private Vector3 Movement;

    public const int Magazine = 36;
    public int Ammo = 3600;
    public int CurMagazine = 36;

    public bool CanAttack = true, Reload = false;

    public Text DisAmmo, 
                DisMagazine, 
                DisHealth,
                DisScore;

    public GameObject   Bullet, 
                        StartBullet,
                        Weapon, 
                        Camera;

    public float RotSpeedWeapon;
    Ray ray;
    RaycastHit hitRay;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        MyBody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            StartCoroutine(Fire());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(StartReload());
        }

        ray = new Ray(Camera.transform.position, Camera.transform.forward);
        Physics.Raycast(ray, out hitRay);
        Vector3 rot;
        if (hitRay.collider == null)
        {
            rot = Weapon.transform.forward;
        }
        else
        {
            rot = hitRay.point - Weapon.transform.position;
        }

        Weapon.transform.rotation = Quaternion.Slerp(Weapon.transform.rotation, Quaternion.LookRotation(rot), RotSpeedWeapon * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        float Right = Input.GetAxisRaw("Horizontal");
        float Forward = Input.GetAxisRaw("Vertical");

        Movement.Set(Forward, 0f, Right);

        MyBody.AddForce(transform.forward * Forward * Speed, ForceMode.Impulse);
        MyBody.AddForce(transform.right * Right * Speed, ForceMode.Impulse);
    }

    IEnumerator Fire()
    {
        //create clone - Bullet is clonning, StartBullet - is the position where Bullet is start to fly
        //
        if (CanAttack && CurMagazine > 0 && !Reload)
        {
            CanAttack = false;
            CurMagazine--;

            DisMagazine.text = "Magazine: " + CurMagazine;

            Instantiate(Bullet, StartBullet.transform.position, StartBullet.transform.rotation);

            if (CurMagazine <= 0)
            {
                StartCoroutine(StartReload());
                Reload = true;
            }
            yield return new WaitForSeconds(0.05f);

            CanAttack = true;
        }
    }

    IEnumerator StartReload()
    {
        yield return new WaitForSeconds(1f);

        if (Ammo > Magazine)
        {
            int Num = Magazine;
            Num -= CurMagazine;
            Ammo -= Num;
            CurMagazine = Magazine;
        }
        else
        {
            CurMagazine = Ammo;
            Ammo = 0;
        }
        DisAmmo.text = "Ammo: " + Ammo;

        Reload = false;
    }


    public void TakeDamage(int Damage)
    {

        Health -= Damage;

        DisHealth.text = "Health: " + Health;

        if (Health <= 0)
        {
            MyBody.constraints = RigidbodyConstraints.None;
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

}
