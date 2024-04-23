using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Reflection;

public class GameManager : MonoBehaviour
{
    public List<AudioClip> deathSounds;
    public List<TargetRagdoll> enemies;

    public AudioClip backMusic;
    public AudioClip winMusic;
    public AudioClip loseMusic;
    public AudioSource backSource;

    private int currentSoundIndex = 0; //indice de sonido actual
    private int currentEnemyIndex = 0; //indice de enemigo actual


    public GameObject winGoal; // Referencia al GameObject de victoria

    private bool winCondition = false; // Bandera para controlar si tienes las condiciones de victoria
    private bool musicChanged = false; // Bandera para controlar si la música ha cambiado

    private bool timerStarted = false; // Bandera para indicar si el temporizador ha empezado
    public float totalTime = 180f; // Duración total del temporizador en segundos (3 minutos)
    public float timeLeft; // Tiempo restante

    public TextMeshProUGUI timerText; // Referencia al TextMeshPro para mostrar el temporizador
    public TextMeshProUGUI fpsText; // Referencia al TextMeshPro para mostrar los FPS

    public GameObject hud; // Referencia al GameObject de hud
    public GameObject winHud; // Referencia al GameObject de hud victoria
    public GameObject loseHud; // Referencia al GameObject de hud victoria

    private void Start()
    {
        backSource.Play();
        hud.SetActive(true);
        winHud.SetActive(false);
        loseHud.SetActive(false);
    }
    private void Update()
    {
        if (timerStarted)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;

                // Actualiza el TextMeshPro con el tiempo restante formateado
                timerText.text = FormatTime(timeLeft);
            }
            else
            {
                // Ralentiza el juego
                Time.timeScale = 0.5f;

                // Cambiar HUD
                hud.SetActive(false);
                winHud.SetActive(false);
                loseHud.SetActive(true);

                //Cambiar musica
                if (!musicChanged)
                {
                    backSource.Stop(); // Detiene la música actual
                    backSource.clip = loseMusic; // Asigna la nueva música
                    backSource.Play(); // Reproduce la nueva música
                    musicChanged = true;
                }
                // El tiempo ha terminado, puedes hacer algo aquí
                Invoke("DoAction", 5f);
            }
        }
        // Calcula los FPS actuales
        int fps = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);

        // Actualiza el TextMeshPro con los FPS
        fpsText.text = "FPS: " + fps.ToString();
    }

    // Método para iniciar el temporizador
    public void StartTimer()
    {
        timerStarted = true;
        timeLeft = totalTime;
        backSource.clip = backMusic;
        backSource.Play();
    }
    void DoAction()
    {
        Time.timeScale = 1f;
        SCManager.instance.LoadScene("Playground");
    }
    // Método para formatear el tiempo en minutos y segundos
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("Time Left: " + "{0:00}:{1:00}", minutes, seconds);
    }

    public void AssignDeathSound(TargetRagdoll enemy)
    {
        // Asigna el clip de audio solo si el enemigo no tiene uno asignado
        if (enemy.deathClip == null)
        {
            enemy.deathClip = deathSounds[currentSoundIndex];
            currentSoundIndex = (currentSoundIndex + 1) % deathSounds.Count;
        }
    }
    public void EnemyDied()
    {
        // Mueve al siguiente índice de sonido de muerte
        currentEnemyIndex++;

        // Reinicia el índice si ya hemos reproducido todos los sonidos
        if (currentEnemyIndex >= deathSounds.Count)
        {
            currentEnemyIndex = 0;
        }

    }
    public void RemoveEnemy(TargetRagdoll enemy)
    {
        enemies.Remove(enemy);
        // Verifica si la lista de enemigos está vacía
        if (enemies.Count == 0)
        {
            winCondition = true;
            // Activa el GameObject cuando la lista esté vacía
            winGoal.SetActive(true);
            winGoal.SendMessage("ActivateSelf", winCondition);
        }
    }
}