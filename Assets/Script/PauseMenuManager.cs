using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Referencias de UI")]
    [Tooltip("Arrastra aquí el panel principal del Menú de Pausa.")]
    public GameObject pauseMenuPanel;

    [Tooltip("Arrastra aquí el panel de Controles dentro del menú.")]
    public GameObject controlsPanel;

    [Header("Control del Jugador")]
    [Tooltip("Arrastra aquí el script de movimiento del jugador.")]
    public MonoBehaviour playerMovementScript;
    [Tooltip("Arrastra aquí el script de mirada del mouse.")]
    public MonoBehaviour mouseLookScript;
    [Tooltip("Arrastra aquí el script de gestión de armas o disparo.")]
    public MonoBehaviour gunScript;

    [Header("Configuración de Escenas")]
    [Tooltip("Nombre de la escena de Menú Principal a la que se regresa al salir.")]
    public string mainMenuSceneName = "MainMenu";

    [Tooltip("Nombre de la escena del primer nivel del juego.")]
    public string level1SceneName = "Level1";

    public static bool isGamePaused = false;

    void Start()
    {
        // El juego siempre comienza corriendo.
        ResumeGame();
        Debug.Log("PAUSE MANAGER: Juego iniciado. Estado inicial: Corriendo.");
    }

    void Update()
    {
        // Lógica para alternar la pausa con la tecla ESCAPE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    /// <summary>
    /// Pausa el juego: Deshabilita scripts, congela tiempo y muestra el menú.
    /// </summary>
    public void PauseGame()
    {
        isGamePaused = true;
        Debug.Log("PAUSE EVENTO: Pausa activada (Tecla ESC).");

        // 1. DESACTIVAR el control y la interacción del jugador
        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (mouseLookScript != null) mouseLookScript.enabled = false;
        if (gunScript != null) gunScript.enabled = false;

        // 2. Congelar el tiempo
        Time.timeScale = 0f;

        // 3. Mostrar el menú de pausa principal (asegurando que controles esté oculto)
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(false);
        }

        // 4. Liberar y mostrar el cursor para interacción con la UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Reanuda el juego: Habilita scripts, restaura tiempo y oculta el menú.
    /// </summary>
    public void ResumeGame()
    {
        isGamePaused = false;
        Debug.Log("PAUSE EVENTO: Juego reanudado.");

        // 1. Reanudar el tiempo
        Time.timeScale = 1f;

        // 2. HABILITAR el control y la interacción del jugador
        if (playerMovementScript != null) playerMovementScript.enabled = true;
        if (mouseLookScript != null) mouseLookScript.enabled = true;
        if (gunScript != null) gunScript.enabled = true;

        // 3. Ocultar todos los paneles
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(false);
        }

        // 4. Bloquear y ocultar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Carga la escena Level 1 (Asignado al botón "EMPEZAR").
    /// </summary>
    public void LoadLevel1()
    {
        Debug.Log("BOTÓN EMPEZAR: Cargando escena: " + level1SceneName);
        // MUY IMPORTANTE: Reanudar el tiempo antes de cargar la escena
        Time.timeScale = 1f;
        SceneManager.LoadScene(level1SceneName);
    }

    /// <summary>
    /// Muestra u oculta el panel de controles y ajusta la visibilidad del panel de pausa principal.
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

        // 2. Ocultar/Mostrar el panel de Pausa principal.
        // Si 'show' es TRUE (abrir Controles), ocultamos el menú principal (!show = FALSE).
        // Si 'show' es FALSE (cerrar Controles/Volver), mostramos el menú principal (!show = TRUE).
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(!show);
        }
    }

    /// <summary>
    /// Sale de la galería y carga el Menú Principal (Asignado al botón "EXIT/SALIR").
    /// </summary>
    public void ExitToMainMenu()
    {
        Debug.Log("BOTÓN EXIT/SALIR: Cargando escena: " + mainMenuSceneName);
        // MUY IMPORTANTE: Reanudar el tiempo antes de cargar la escena
        Time.timeScale = 1f;

        // Carga la escena de menú de inicio
        SceneManager.LoadScene(mainMenuSceneName);
    }
}