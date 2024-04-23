using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRagdoll : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] Animator animator;
    public int life = 3;

    private Rigidbody[] rigidbodies;
    public GameObject pfSound;
    public AudioClip deathClip; // clips de audio para la muerte

    private bool deathSoundAssigned = false; // Variable para controlar si el sonido de muerte ya se ha asignado
    private bool deathSoundPlayed = false; // Variable para controlar si el sonido de muerte ya se ha reproducido

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        SetEnabled(false);
    }

    void SetEnabled(bool enabled)
    {
        bool isKinematic = !enabled;
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = isKinematic;
        }

        animator.enabled = !enabled;
    }

    void Update()
    {
        if (life <= 0 && !deathSoundAssigned) // Verifica si el enemigo ha muerto y el sonido de muerte no se ha asignado
        {
            if (!deathSoundPlayed)
            { 
                SetEnabled(true);
                // Reproducir sonido de muerte
                gameManager.AssignDeathSound(this);
                GameObject sfx = Instantiate(pfSound, transform.position, Quaternion.identity);
                sfx.GetComponent<AudioSource>().clip = deathClip;
                sfx.GetComponent<AudioSource>().Play();
                Destroy(sfx, deathClip.length); // Destruye el objeto después de que termine el clip

                deathSoundAssigned = true; // Marca que el sonido de muerte ha sido asignado
                deathSoundPlayed = true; // Marca que el sonido de muerte ha sido reproducido

                gameManager.EnemyDied();
                Destroy(gameObject, 5f); // Destruye el enemigo después de 5 segundos.
                gameManager.RemoveEnemy(this);
            }
        }
    }

    public void TakeDamage(int damage, GameObject hit)
    {
        TargetRagdoll dummy = hit.GetComponentInParent<TargetRagdoll>();
        if (dummy == this.gameObject.GetComponent<TargetRagdoll>())
        {
            life -= damage;
        }
    }



    // Suscribirse al evento cuando se habilita el objeto BulletProjectile
    private void OnEnable()
    {
        BulletProjectile.OnHitBody += TakeDamage;
        BulletProjectile.OnHitHead += TakeDamage;
    }

    // Darse de baja del evento cuando se deshabilita el objeto BulletProjectile
    private void OnDisable()
    {
        BulletProjectile.OnHitBody -= TakeDamage;
        BulletProjectile.OnHitHead -= TakeDamage;
    }
}
