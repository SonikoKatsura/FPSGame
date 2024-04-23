using UnityEngine;

public class WinArea : MonoBehaviour
{
    
    private GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
        private void OnTriggerEnter(Collider other)
    {
        // Ralentiza el juego
        Time.timeScale = 0.5f;

        // Cambiar HUD
        gameManager.hud.SetActive(false);
        gameManager.winHud.SetActive(true);
        gameManager.loseHud.SetActive(false);

        //bloquea que salga defeat
        gameManager.timeLeft = 100f;

        // Cambia la música
        gameManager.backSource.Stop(); // Detiene la música actual
        gameManager.backSource.clip = gameManager.winMusic; // Asigna la nueva música
        gameManager.backSource.Play(); // Reproduce la nueva música


        // Realiza una acción después de 5 segundos
        Invoke("DoAction", 5f);
    }

     void DoAction()
    {
        
        Time.timeScale = 1f;
        SCManager.instance.LoadScene("Playground");
    }

}