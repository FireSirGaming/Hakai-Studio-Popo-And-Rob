using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReady : NetworkBehaviour
{
    [SerializeField] private Button readyButton;

    private void Awake()
    {
        readyButton.onClick.AddListener(() =>
        {
            if (Object.HasInputAuthority)
            {
                PlayerReadyManager.Instance.SetPlayerReady();
            }
            else
            {
                Debug.Log($"{Runner.LocalPlayer} has no input authority!");
            }
        });
    }
}
