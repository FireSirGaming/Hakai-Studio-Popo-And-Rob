using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class CreateAndJoinUI : MonoBehaviour
{
    private PlayerManager playerManager;

    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;

    private void Awake()
    {
        playerManager = GameObject.FindAnyObjectByType<PlayerManager>();

        createRoomButton.onClick.AddListener(() =>
        {
            if (playerManager.NetworkRunner == null)
            {
                playerManager.RoomCreate(GameMode.Host);
            }

        });

        joinRoomButton.onClick.AddListener(() =>
        {
            if (playerManager.NetworkRunner == null)
            {
                playerManager.RoomCreate(GameMode.Client);
            }
        });
    }
}
