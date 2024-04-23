using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static BulletProjectile;

public class PickUp : MonoBehaviour
{
    public float floatHeight = 1f; // Altura a la que flota el objeto
    public float floatSpeed = 1f; // Velocidad a la que flota el objeto
    public float rotateSpeed = 50f; // Velocidad de rotación del objeto

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    public bool dataGot = false;

    public GameObject enemies;

    public GameObject pfSound;
    public AudioClip clip;

    private GameManager gameManager;
    // Declaraci�n del delegado
    public delegate void DatabaseGot(bool dbs);
    // Definici�n del evento
    public static event DatabaseGot OnDatabaseGot;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        enemies.SetActive(false);
        // Store the original position for reference
        originalPosition = transform.position;
    }

    void Update()
    {
        // Calcular la posición de destino del objeto

        targetPosition = originalPosition + Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        // Mover el objeto hacia la posición de destino
        transform.position = targetPosition;

        // Rotar el objeto
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (OnDatabaseGot != null)
        {
            if (gameManager != null)
            {
                // Inicia el temporizador cuando recojas el objeto
                gameManager.StartTimer();
            }
            dataGot = true;
            OnDatabaseGot(dataGot);
            enemies.SetActive(true);
            //pick sound
            GameObject sfx = Instantiate(pfSound, this.transform.position, Quaternion.identity);
            sfx.GetComponent<AudioSource>().clip = clip;
            sfx.GetComponent<AudioSource>().Play();
            Destroy(sfx, 1f);
        }
        Destroy(this.gameObject);

    }
}