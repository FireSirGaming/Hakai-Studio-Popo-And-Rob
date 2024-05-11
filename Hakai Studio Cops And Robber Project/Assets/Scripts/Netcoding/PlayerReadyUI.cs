using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReadyUI : NetworkBehaviour
{
    [SerializeField] private Button readyButton;
    [SerializeField] private Button testButton;

    public void Awake()
    {
        readyButton.onClick.AddListener(() =>
        {
            PlayerReadyManager.Instance.SetPlayerReady();
        });

        testButton.onClick.AddListener(() =>
        {
            PlayerReadyManager.Instance.SetTest();
        });
    }
}
