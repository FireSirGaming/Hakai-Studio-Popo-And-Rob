using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    public int Damage { get { return damage; } }

    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider bc;

    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        bc = transform.GetChild(0).GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        animator.SetBool("InputAttack", true);
        StartCoroutine(InputDuration());
    }

    IEnumerator InputDuration()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("InputAttack", false);
    }
}
