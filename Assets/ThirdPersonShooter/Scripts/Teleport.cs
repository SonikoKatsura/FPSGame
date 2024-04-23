using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform destination; // El punto de destino al que se teletransportará el jugador

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que colisionó es el jugador
        {
            Fade.OutIn();
            // Teletransporta al jugador al punto de destino utilizando el Character Controller
           other.GetComponent<CharacterController>().enabled = false; // Deshabilita el Character Controller temporalmente para cambiar la posición
            other.transform.position = destination.position;
            other.GetComponent<CharacterController>().enabled = true; // Vuelve a habilitar el Character Controller
        }
    }
}