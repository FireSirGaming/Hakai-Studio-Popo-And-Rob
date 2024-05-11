using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : NetworkBehaviour
{
    public Camera fpsCam;
    public Recoil recoil;
    [SerializeField] private WeaponRecoil weaponRecoil;
    private WeaponSwitcherScript weaponSwitcher;
    [SerializeField] private Gun[] gun = new Gun[3];
    public int selectWeapon;
    public float radius = 20f;

    [SerializeField]
    private bool bulletSpread = true;

    [SerializeField]
    private Vector3 maxBulletSpreadvariance = new Vector3(0.1f, 0.1f, 0.1f);

    [SerializeField]
    private Vector3 minBulletSpreadvariance = new Vector3(0.01f, 0.01f, 0.01f);

    [SerializeField]
    private Vector3 currentBulletSpreadVariance;

    [SerializeField]
    private ParticleSystem shootingSystem;

    [SerializeField]
    private Transform gunMuzzle;

    [SerializeField]
    private ParticleSystem ImpactParticleSystem;

    [SerializeField]
    private TrailRenderer bulletTrail;

    [SerializeField]
    private PlayerController playerController;

    void Start()
    {
        if (IsLocalPlayer)
        {
            weaponSwitcher = this.gameObject.GetComponent<WeaponSwitcherScript>();
            weaponRecoil = transform.GetChild(0).GetComponent<WeaponRecoil>();
        }

        currentBulletSpreadVariance = minBulletSpreadvariance;
    }

    void Update()
    {
        if (weaponSwitcher != null)
        {
            if (weaponSwitcher.selectedWeapon == 0)
            {
                selectWeapon = 0;
            }

            if (weaponSwitcher.selectedWeapon == 1)
            {
                selectWeapon = 1;
            }

            if (weaponSwitcher.selectedWeapon == 2)
            {
                selectWeapon = 2;
            }
        }

        AdjustSpread();
    }

    private void AdjustSpread()
    {
        if (playerController.IsMoving)
        {
            currentBulletSpreadVariance.x += 0.5f * Time.deltaTime;
            currentBulletSpreadVariance.y += 0.5f * Time.deltaTime;
            currentBulletSpreadVariance.z += 0.5f * Time.deltaTime;
        }

        else
        {
            currentBulletSpreadVariance.x -= 0.5f * Time.deltaTime;
            currentBulletSpreadVariance.y -= 0.5f * Time.deltaTime;
            currentBulletSpreadVariance.z -= 0.5f * Time.deltaTime;
        }

        currentBulletSpreadVariance.x = Mathf.Clamp(currentBulletSpreadVariance.x, minBulletSpreadvariance.x, maxBulletSpreadvariance.x);
        currentBulletSpreadVariance.y = Mathf.Clamp(currentBulletSpreadVariance.y, minBulletSpreadvariance.y, maxBulletSpreadvariance.y);
        currentBulletSpreadVariance.z = Mathf.Clamp(currentBulletSpreadVariance.z, minBulletSpreadvariance.z, maxBulletSpreadvariance.z);
    }

    public void Shoot(float range)
    {
        recoil.RecoilFire();
        weaponRecoil.RecoilFire();

        shootingSystem.Play();
        Vector3 direction = GetDirection();

        if (Physics.Raycast(fpsCam.transform.position, direction, out RaycastHit hit, range))
        {
            TrailRenderer trail = Instantiate(bulletTrail, gunMuzzle.transform.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit));
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;

        if (bulletSpread)
        {
            direction += new Vector3
                (
                  Random.Range(-currentBulletSpreadVariance.x, currentBulletSpreadVariance.x),
                  Random.Range(-currentBulletSpreadVariance.y, currentBulletSpreadVariance.y),
                  Random.Range(-currentBulletSpreadVariance.z, currentBulletSpreadVariance.z)
                );

            direction.Normalize();
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < TimeCheck(trail, hit))
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        Debug.Log(time);

        trail.transform.position = hit.point;
        CheckEnemy(hit);
        Instantiate(ImpactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
    }

    private float TimeCheck(TrailRenderer trail, RaycastHit hit)
    {
        float speed = 50f;
        float distance = Vector3.Distance(trail.transform.position, hit.point);

        return distance / speed;

    }

    private void CheckEnemy(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Enemy"))
        {
            TakeDamageScript takeDamage = hit.collider.GetComponent<TakeDamageScript>();
            takeDamage.TakeDamage(10);
        }
    }
}
