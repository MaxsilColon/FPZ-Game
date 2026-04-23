using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Configuración de Escenas")]
    [Tooltip("El nombre exacto de la escena de la galería de tiro (el juego).")]
    public string gameSceneName = "ShootingRange";

    [Header("Referencias de UI")]
    [Tooltip("Arrastra aquí el Panel de Controles.")]
    public GameObject controlsPanel;

    [Tooltip("Arrastra aquí el Panel Principal del Menú (el que contiene 'Empezar').")]
    public GameObject mainMenuPanel;

    void Start()
    {
        // 1. Aseguramos que el tiempo esté corriendo.
        Time.timeScale = 1f;

        // 2. Asegura que los paneles estén en su estado inicial correcto (Controles oculto, Principal visible).
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);

        // 3. Liberamos el cursor para interacción.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("MAIN MENU MANAGER: Menú principal listo.");
    }

    void Update()
    {
        // Esta función se mantiene vacía, ya que el menú principal no usa la tecla ESC.
    }

    /// <summary>
    /// Asignado al botón EMPEZAR. Carga la escena de juego.
    /// </summary>
    public void StartGame()
    {
        Debug.Log("BOTÓN EMPEZAR: Cargando escena de juego: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// Muestra u oculta el panel de controles y ajusta la visibilidad del panel principal.
    /// </summary>
    public void ToggleControlsPanel(bool show)
    {
        if (show)
        {
            Debug.Log("BOTÓN CONTROLES: Abriendo panel de controles.");
        }
        else
        {
            Debug.Log("BOTÓN VOLVER: Cerrando panel de controles.");
        }

        // 1. Mostrar/Ocultar el panel de Controles
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(show);
        }

        // 2. Ocultar/Mostrar el panel Principal de Botones
        // El panel principal se activa cuando show es FALSE (Botón VOLVER).
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(!show);
        }
    }

    /// <summary>
    /// Asignado al botón SALIR. Cierra la aplicación.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("BOTÓN SALIR: Cerrando aplicación.");
        Application.Quit();

        // Código para detener el editor de Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}