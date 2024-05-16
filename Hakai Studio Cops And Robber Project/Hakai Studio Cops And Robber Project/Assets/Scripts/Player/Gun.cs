using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    // private variables
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float aimRecoilX;
    [SerializeField] private float aimRecoilY;
    [SerializeField] private float aimRecoilZ;

    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;

    //public variables
    public float RecoilX { get { return recoilX; } }
    public float RecoilY { get { return recoilY; } }
    public float RecoilZ { get { return recoilZ; } }

    public float AimRecoilX { get { return aimRecoilX; } }
    public float AimRecoilY { get { return aimRecoilY; } }
    public float AimRecoilZ { get { return aimRecoilZ; } }

    public float Snappiness { get { return snappiness; } }
    public float ReturnSpeed { get { return returnSpeed; } }
    //public int maxAmmo;
    //public int currentAmmo;
    //public float reloadTime;

    private float nextTimeToFire = 0f;

    //public WeaponSwitcherScript weaponSwitcher;

    public Shooting shoot;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            shoot.Shoot(range);
            nextTimeToFire = Time.time + 1f / fireRate;
        }
    }
}
