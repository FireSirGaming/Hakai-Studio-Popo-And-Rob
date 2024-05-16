using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageScript : MonoBehaviour
{
    private HealthScript healthScript;

    private MeshRenderer meshRenderer;
    private Material currentMaterial;
    public Material flashMaterial;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        currentMaterial = GetComponent<MeshRenderer>().material;
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(Flashing());
        healthScript = GetComponent<HealthScript>();
        healthScript.CurrentHealth -= damage;
    }

    IEnumerator Flashing()
    {
        meshRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.25f);
        meshRenderer.material = currentMaterial;
    }
}
