using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement; // Se incluye la librería para el manejo de escenas
// OJO: al incluir esta librería, no se podrá usar el nombre "SceneManager" porque
// ya hay una clase Static con dicho nombre. Por eso la clase se llama "SCManager"


public class SCManager : MonoBehaviour
{

    // Creamos una variable estática para almacenar la única instancia
    public static SCManager instance;

    // Método Awake que se llama al inicio antes de que se active el objeto. Útil para inicializar
    // variables u objetos que serán llamados por otros scripts (game managers, clases singleton, etc).
    private void Awake()
    {

        // ----------------------------------------------------------------
        // AQUÍ ES DONDE SE DEFINE EL COMPORTAMIENTO DE LA CLASE SINGLETON
        // Garantizamos que solo exista una instancia del SCManager
        // Si no hay instancias previas se asigna la actual
        // Si hay instancias se destruye la nueva
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        // ----------------------------------------------------------------

        // No destruimos el SceneManager aunque se cambie de escena
        DontDestroyOnLoad(gameObject);

    }

    // Método para cargar una nueva escena por nombre
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}

// -----------------------------------------
// EJEMPLOS DE USO ¡DESDE CUALQUIER SCRIPT!
// -----------------------------------------
// SCManager.instance.LoadScene("MainTitle");
// SCManager.instance.LoadScene("EndGame");