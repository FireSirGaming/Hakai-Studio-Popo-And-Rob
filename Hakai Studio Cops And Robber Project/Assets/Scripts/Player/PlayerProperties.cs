using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static Unity.Collections.Unicode;

public class PlayerProperties : NetworkBehaviour
{
    [SerializeField] private int playerGroup = 0;
    public int PlayerGroup { get { return playerGroup; } }

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject weaponCamera;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            mainCamera.SetActive(true);
            weaponCamera.SetActive(true);
        }

        else
        {
            mainCamera.SetActive(false);
            weaponCamera.SetActive(false);
        }
    }
}
