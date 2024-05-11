using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MultiplayerCamera : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera WeaponCamera;
    [SerializeField] private AudioListener audioListener;

}
