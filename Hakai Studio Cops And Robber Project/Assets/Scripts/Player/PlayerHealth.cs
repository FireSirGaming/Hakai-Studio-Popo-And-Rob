using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;

    [Networked, OnChangedRender(nameof(HealthChanged))]
    public int NetworkHealth {  get; set; }

    [SerializeField] private bool die = false;
    [SerializeField] private float maxRespawnTime = 5f;
    [SerializeField] private float currentRespawnTime;

    //this is to get all the player compoents
    [SerializeField] private CapsuleCollider notTrigger;
    [SerializeField] private CapsuleCollider trigger;
    [SerializeField] private PlayerProperties playerProperties;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private GameObject WeaponCamera;
    [SerializeField] private GameObject capsule;

    private void Awake()
    {

    }

    private void Start()
    {
        NetworkHealth = 100;
        currentRespawnTime = maxRespawnTime;
    }

    private void Update()
    {
        if (die)
        {
            currentRespawnTime -= Time.deltaTime;

            if (currentRespawnTime <= 0f)
            {
                Respawn();
                currentRespawnTime = maxRespawnTime;
                die = false;
            }
        }
    }

    void HealthChanged()
    {
        Debug.Log($"Health changed to : {NetworkHealth}");
        healthText.text = $"Health: {NetworkHealth}";

        if (NetworkHealth <= 0)
        {
            die = true;
            Die();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(int damage)
    {
        Debug.Log("Received DealDamageRpc on stateAuthority, modifying Networked variable");
        NetworkHealth -= damage;
    }

    public void Die()
    {
        WeaponCamera.SetActive(false);
        playerCanvas.SetActive(false);
        notTrigger.enabled = false;
        trigger.enabled = false;
        playerProperties.enabled = false;
        playerController.enabled = false;
        capsule.SetActive(false);
    }

    public void Respawn()
    {
        NetworkHealth = 100;
        this.gameObject.transform.position = new Vector3(-11.5f, 4f, -490f);
        WeaponCamera.SetActive(true);
        playerCanvas.SetActive(true);
        notTrigger.enabled = true;
        trigger.enabled = true;
        playerProperties.enabled = true;
        playerController.enabled = true;
        capsule.SetActive(true);
    }
}
