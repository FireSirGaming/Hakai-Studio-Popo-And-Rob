using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    //public WeaponSwitcherScript weaponSwitcher;
    [SerializeField] private Gun[] gun = new Gun[3];
    public int selectWeapon;

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [SerializeField] private Gun aim;

    private bool isAiming;

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, gun[selectWeapon].ReturnSpeed * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, gun[selectWeapon].Snappiness* Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        //if (isAiming)
        //{
        //    targetRotation += new Vector3(gun[selectWeapon].aimrecoilX, Random.Range(-gun[selectWeapon].aimrecoilY, gun[selectWeapon].aimrecoilY), Random.Range(-gun[selectWeapon].aimrecoilZ, gun[selectWeapon].aimrecoilZ));
        //}
        //else
        //{
        //    targetRotation += new Vector3(gun[selectWeapon].recoilX, Random.Range(-gun[selectWeapon].recoilY, gun[selectWeapon].recoilY), Random.Range(-gun[selectWeapon].recoilZ, gun[selectWeapon].recoilZ));
        //}

        targetRotation += new Vector3(gun[selectWeapon].RecoilX, Random.Range(-gun[selectWeapon].RecoilY, gun[selectWeapon].RecoilY), Random.Range(-gun[selectWeapon].RecoilZ, gun[selectWeapon].RecoilZ));
    }
}
