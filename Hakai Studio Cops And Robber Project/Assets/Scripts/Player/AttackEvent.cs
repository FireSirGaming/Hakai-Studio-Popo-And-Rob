using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    [SerializeField] private Knife knife;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider bc;

    private void Start()
    {
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider>();
        knife = gameObject.GetComponentInParent<Knife>();
    }

    private void StartAttack()
    {
        animator.SetBool("HasAttack", true);
    }

    private void EndAttack()
    {
        animator.SetBool("HasAttack", false);
    }

    private void Attack()
    {
        bc.enabled = true;
        StartCoroutine(AttackDuration());
    }

    IEnumerator AttackDuration()
    {
        yield return new WaitForSeconds(0.1f);
        bc.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamageScript takeDamage = other.GetComponent<TakeDamageScript>();

            takeDamage.TakeDamage(knife.Damage);
        }
    }

}
