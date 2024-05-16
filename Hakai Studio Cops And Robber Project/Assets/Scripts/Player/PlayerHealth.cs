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

    private void Start()
    {
        NetworkHealth = 100;
    }

    void HealthChanged()
    {
        Debug.Log($"Health changed to : {NetworkHealth}");
        healthText.text = $"Health: {NetworkHealth}";
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(int damage)
    {
        Debug.Log("Received DealDamageRpc on stateAuthority, modifying Networked variable");
        NetworkHealth -= damage;
    }
}
