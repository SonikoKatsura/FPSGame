using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] GameObject vfxTarget;
    [SerializeField] GameObject vfxHeadshot;
    [SerializeField] GameObject vfxOther;

    // Declaraci�n del delegado
    public delegate void HitBody(int dmg, GameObject hit);
    // Definici�n del evento
    public static event HitBody OnHitBody;

    // Declaraci�n del delegado
    public delegate void HitHead(int dmg, GameObject hit);
    // Definici�n del evento
    public static event HitHead OnHitHead;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Head"))
        {
            // Crear efecto visual
            GameObject targetHit = Instantiate(vfxHeadshot, transform.position, Quaternion.identity);
            Destroy(targetHit, 1f);
            if (OnHitHead != null)
            {
                OnHitHead(3, other.gameObject);
            }
        }
        if (other.gameObject.CompareTag("Target"))
        {
            // Crear efecto visual
            GameObject targetHit = Instantiate(vfxTarget, transform.position, Quaternion.identity);
            Destroy(targetHit, 1f);
            if (OnHitBody != null)
            {
                OnHitBody(1, other.gameObject);
            }
        }
        else
        {
            // Crear efecto visual
            GameObject otherHit = Instantiate(vfxOther, transform.position, Quaternion.identity);
            Destroy(otherHit, 1f);
        }
        this.gameObject.SetActive(false);
    }
}