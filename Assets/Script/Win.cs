using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // Necesario para el EventSystem

public class WinManager : MonoBehaviour
{
    // ----------------------------------------------------
    // SINGLETON (PUNTO DE ACCESO ESTÁTICO)
    // ----------------------------------------------------
    private static WinManager instance;

    [Header("UI Control")]
    [Tooltip("Arrastra aquí el panel 'WinScreen' del Canvas. Debe estar INACTIVO al inicio.")]
    public GameObject winScreenPanel;

    [Header("Scene Settings")]
    [Tooltip("Nombre de la escena de Menú Principal a cargar al presionar Salir.")]
    public string mainMenuSceneName = "Main Menu";

    void Awake()
    {
        // Inicializa el Singleton y se asegura de que solo exista una instancia.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Muestra la pantalla de victoria y pausa el juego.
    /// Se llama estáticamente desde el script de muerte del Boss (BossAI.Die()).
    /// </summary>
    public static void ShowWinScreen()
    {
        if (instance != null)
        {
            // 1. ACTIVAR EL PANEL
            instance.winScreenPanel.SetActive(true);

            // 2. FORZAR ACTIVACIÓN DEL EVENT SYSTEM (SOLUCIÓN para botones que no responden al Time.timeScale = 0f)
            if (EventSystem.current != null)
            {
                // Establece el panel de victoria como el objeto seleccionado para reactivar la entrada de la UI
                EventSystem.current.SetSelectedGameObject(instance.winScreenPanel);
            }

            // 3. DETENER EL TIEMPO
            Time.timeScale = 0f;

            // 4. LIBERAR el cursor para interacción con la UI
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log("Pantalla de victoria activada.");
        }
        else
        {
            Debug.LogError("WinManager no encontrado en la escena. ¿Está activo?");
        }
    }

    /// <summary>
    /// Se llama al presionar el botón 'EXIT' de la UI.
    /// Reanuda el tiempo y carga la escena del menú principal.
    /// </summary>
    public void LoadMainMenu()
    {
        // >>> DEBUG AGREGADO <<<
        Debug.Log("--- BOTÓN EXIT PRESIONADO: Iniciando carga de escena '" + mainMenuSceneName + "' ---");

        // 1. Reanudar el tiempo
        Time.timeScale = 1f;

        // 2. Restaurar el cursor (aunque no es estrictamente necesario antes de cargar)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 3. Cargar la escena del menú principal (usa la variable pública)
        SceneManager.LoadScene(mainMenuSceneName);
    }
}