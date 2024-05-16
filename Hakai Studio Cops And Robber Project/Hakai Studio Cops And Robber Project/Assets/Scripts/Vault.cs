using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Vault : MonoBehaviour
{
    [SerializeField] private int startingMoney = 1000;
    public int StartMoney { get { return startingMoney; } set {  startingMoney = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Melee") /*&&*/)
        {
            TakeMoney(100, other.GetComponentInParent<PlayerProperties>().gameObject);
        }
    }

    private void TakeMoney(int money, GameObject player)
    {
        PlayerProperties playerInfo = player.GetComponent<PlayerProperties>();

        if (playerInfo.PlayerGroup != 0)
        {
            return;
        }

        startingMoney -= money;
    }

    private void ReturnMoney(int money, GameObject player)
    {
        
    }
}
