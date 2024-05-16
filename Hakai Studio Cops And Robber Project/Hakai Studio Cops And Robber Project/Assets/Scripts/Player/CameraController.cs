using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera weaponCamera;

    private void Start()
    {
        if (IsLocalPlayer)
        {
            playerCamera.gameObject.SetActive(true);
            weaponCamera.gameObject.SetActive(true);
        }
    }
}
