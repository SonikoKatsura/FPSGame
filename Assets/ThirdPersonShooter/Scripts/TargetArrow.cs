using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetArrow : MonoBehaviour
{
    public Transform playerCamera;
    public Transform disk;
    public Transform player;
    public Transform winArea;
    public float rotationSpeed;
    private TargetRagdoll closestEnemy; // Referencia al enemigo más cercano
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        if (disk != null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(disk.position - playerCamera.position), rotationSpeed * Time.deltaTime);
        }
        else if (gameManager.enemies.Count == 0 && winArea != null)
        {
            // Si no hay enemigos y la winArea está presente, apunta hacia la winArea
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(winArea.position - playerCamera.position), rotationSpeed * Time.deltaTime);
        }
        else
        {


            FindClosestEnemy();

            if (closestEnemy != null)
            {
                // Apunta hacia el enemigo más cercano
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(closestEnemy.transform.position - playerCamera.position), rotationSpeed * Time.deltaTime);
            }
        }
       
    }

    void FindClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        closestEnemy = null;

        // Itera a través de la lista de enemigos para encontrar el enemigo más cercano
        foreach (TargetRagdoll enemy in gameManager.enemies)
        {
            float distance = Vector3.Distance(player.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
    }
}
