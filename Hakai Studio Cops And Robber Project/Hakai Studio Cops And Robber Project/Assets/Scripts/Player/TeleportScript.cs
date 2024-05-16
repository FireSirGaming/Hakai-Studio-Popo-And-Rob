using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public Transform player;
    public PlayerController playerController;

    public Transform teleporter;
    public Transform teleportTarget;

    private Vector3 playerRelativePos;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        teleporter = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerController.teleportCooldown > 0) playerController.teleportCooldown -= Time.deltaTime;
    }

    // Teleporter
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (teleportTarget != null && playerController.teleportCooldown <= 0)
            {
                playerController.teleportCooldown = playerController.teleportDelay;
                playerRelativePos = player.position - teleporter.position;
                playerRelativePos = new Vector3(-playerRelativePos.x, playerRelativePos.y, playerRelativePos.z);
                player.position = teleportTarget.position - playerRelativePos;
                player.rotation = teleportTarget.rotation;
            }
        }
    }
}
