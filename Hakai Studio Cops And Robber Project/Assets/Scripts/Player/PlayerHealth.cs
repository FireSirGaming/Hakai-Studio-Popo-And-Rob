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
    [SerializeField] private List <MonoBehaviour> scripts = new List<MonoBehaviour>();
    [SerializeField] private List <CapsuleCollider> capsules = new List<CapsuleCollider>();
    [SerializeField] private List <GameObject> gameObjects = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI dieText;
    [SerializeField] private TextMeshProUGUI respawnCountdown;

    private void Awake()
    {

    }

    private void Start()
    {
        NetworkHealth = 100;
        currentRespawnTime = maxRespawnTime;
        dieText.enabled = false;
        respawnCountdown.enabled = false;
    }

    private void Update()
    {
        if (die)
        {
            currentRespawnTime -= Time.deltaTime;

            respawnCountdown.text = $"Respawn in: {currentRespawnTime.ToString("F0")}s";

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
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }

        foreach (CapsuleCollider capsule in capsules)
        {
            capsule.enabled = false;
        }

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(false);
        }

        dieText.enabled = true;
        respawnCountdown.enabled = true;
    }

    public void Respawn()
    {
        NetworkHealth = 100;
        this.gameObject.transform.position = new Vector3(-11.5f, 4f, -490f);

        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;
        }

        foreach (CapsuleCollider capsule in capsules)
        {
            capsule.enabled = true;
        }

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(true);
        }

        dieText.enabled = false;
        respawnCountdown.enabled = false;
    }
}
