using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;
using System.Linq;
using static PickUp;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera aimVirtualCamera;

    [Tooltip("Sensitivity of the camera")]
    [SerializeField] float lookSensitivity;

    [Tooltip("Sensitivity of the aiming camera")]
    [SerializeField] float aimSensitivity;

    [Tooltip("Colission mask for the aim cursor")]
    [SerializeField] LayerMask aimColliderMask = new LayerMask();

    [Tooltip("Debug Aim location marker")]
    [SerializeField] Transform debugTransform;

    [Tooltip("Aim location distance from player")]
    [SerializeField] float aimDistance = 10f;

    [Tooltip("Bullet speed when shoot")]
    [SerializeField] float bulletSpeed;

    [Tooltip("Bullet spawn location")]
    [SerializeField] Transform spawnBulletPos;

    [SerializeField] GameObject Pistol;

    [Tooltip("Audio list for the pistol")]
    [SerializeField] AudioClip[] ShootAudioClips;

    [Tooltip("Pistol shoot volume")]
    [Range(0, 1)] public float ShootAudioVolume = 0.5f;

    [Tooltip("Sound model to use")]
    [SerializeField] GameObject pfSound;

    [Tooltip("Database in mopdel")]
    [SerializeField] GameObject Data;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs aimInputs;
    private Animator animator;
    private float aimRigWeight;
    private Rig rig;
    // Start is called before the first frame update
    void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        aimInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        rig = GetComponentInChildren<Rig>();
    }

    // Update is called once per frame
    void Update()
    {
        rig.weight = Mathf.Lerp(rig.weight, aimRigWeight, Time.deltaTime * 20f);

        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            if (Vector3.Dot(ray.direction, Vector3.forward) > 0) // Si estás mirando hacia adelante (en dirección positiva del eje Z)
            {
                if (raycastHit.point.z > spawnBulletPos.position.z + aimDistance)
                {
                    debugTransform.position = raycastHit.point;
                    mouseWorldPosition = raycastHit.point;
                }
                else
                {
                    debugTransform.position = ray.GetPoint(100);
                    mouseWorldPosition = ray.GetPoint(100);
                }
            }
            else // Si estás mirando hacia atrás (en dirección negativa del eje Z)
            {
                if (raycastHit.point.z < spawnBulletPos.position.z - aimDistance)
                {
                    debugTransform.position = raycastHit.point;
                    mouseWorldPosition = raycastHit.point;
                }
                else
                {
                    debugTransform.position = ray.GetPoint(100);
                    mouseWorldPosition = ray.GetPoint(100);
                }
            }
        }
        else
        {
            debugTransform.position = ray.GetPoint(100);
            mouseWorldPosition = ray.GetPoint(100);
        }
        if (aimInputs.aim)
        {
            //change camera and sensitivity
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            animator.SetLayerWeight(1, 1);
            aimRigWeight = 1f;
            Pistol.SetActive(true);

            if (aimInputs.shoot)
            {
                //bullet sound
                GameObject sfx = Instantiate(pfSound, spawnBulletPos.position, Quaternion.identity);
                var index = Random.Range(0, ShootAudioClips.Length);
                sfx.GetComponent<AudioSource>().clip = ShootAudioClips[index];
                sfx.GetComponent<AudioSource>().volume = ShootAudioVolume;
                sfx.GetComponent<AudioSource>().Play();
                Destroy(sfx, 1f);

                // -------------------------------------------------------------------------------------
                // Utilizamos una bala del Object Pool (sustituye a la instanciaci�n si no hay impacto)
                // -------------------------------------------------------------------------------------
                Vector3 aimDir = (mouseWorldPosition - spawnBulletPos.position).normalized;
                GameObject bullet = ObjectPool.instance.GetPooledObject();
                if (bullet != null)
                {
                    bullet.transform.position = spawnBulletPos.position;
                    bullet.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
                    bullet.SetActive(true);
                    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
                }

                aimInputs.shoot = false;

            }
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetRotateOnMove(true);
            thirdPersonController.SetSensitivity(lookSensitivity);
            aimInputs.shoot = false;
            animator.SetLayerWeight(1, 0);
            aimRigWeight = 0f;
            Pistol.SetActive(false);
        }




    }
    public void EnableDatabase(bool dbs)
    {
        if (dbs)
        {
            Data.SetActive(true);
        }
        else { Data.SetActive(false); }
    }



    // Nos suscribimos al evento cuando se habilita el objeto BulletProjectile
    private void OnEnable()
    {

        PickUp.OnDatabaseGot += EnableDatabase;

    }
    // Nos damos de baja del evento cuando se deshabilita el objeto BulletProjectile
    private void OnDisable()
    {
        PickUp.OnDatabaseGot -= EnableDatabase;
    }
}
