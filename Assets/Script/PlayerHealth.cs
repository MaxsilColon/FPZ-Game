using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI References")]
    [Tooltip("Arrastra aquí el componente Image del Relleno (Fill) de la barra de vida.")]
    public Image healthFillImage;

    [Tooltip("Arrastra aquí el objeto principal (Panel o GameObject) de la pantalla GAME OVER.")]
    public GameObject gameOverScreen;

    [Header("Control Components")]
    [Tooltip("Arrastra aquí el script de movimiento del jugador.")]
    public MonoBehaviour playerMovementScript;
    [Tooltip("Arrastra aquí el script de mirada del mouse.")]
    public MonoBehaviour mouseLookScript;
    [Tooltip("Arrastra aquí el script de gestión de armas o disparo (Ej: Gun Inventory).")]
    public MonoBehaviour gunScript;
    // Eliminamos la referencia al Collider para evitar la caída al congelar el tiempo.

    [Header("Game Over Settings")]
    [Tooltip("Nombre de la escena a cargar al reiniciar (Debe ser 'Level 1').")]
    public string retrySceneName = "Level 1";
    [Tooltip("Nombre de la escena de Menú Principal a cargar al presionar Salir.")]
    public string mainMenuSceneName = "MainMenu";

    void Start()
    {
        // 1. Inicializar el estado del juego y el cursor
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentHealth = maxHealth;
        UpdateHealthUI();

        // 2. Ocultar la pantalla de Game Over al inicio
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    /// <summary>
    /// Resta daño a la salud del jugador.
    /// </summary>
    /// <param name="amount">Cantidad de daño recibido.</param>
    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Actualiza la barra de vida y asegura que llegue a cero visualmente.
    /// </summary>
    void UpdateHealthUI()
    {
        if (healthFillImage != null)
        {
            float healthPercentage = currentHealth / maxHealth;

            healthFillImage.fillAmount = healthPercentage;

            // Comprobación doble para la barra de vida
            if (currentHealth <= 0.01f)
            {
                healthFillImage.fillAmount = 0f;
            }
        }
    }

    void Die()
    {
        Debug.Log("¡Jugador ha muerto! GAME OVER");

        // 1. Desactivar el control y la interacción del jugador (se congela en su sitio)
        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (mouseLookScript != null) mouseLookScript.enabled = false;
        if (gunScript != null) gunScript.enabled = false;

        // 2. MOSTRAR y LIBERAR el mouse para interacción con la UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 3. CONGELAR el juego completamente
        Time.timeScale = 0f;

        // 4. Mostrar la pantalla de Game Over
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }

    // --- FUNCIONES PARA BOTONES ---

    /// <summary>
    /// Función llamada por el botón "Volver a Intentar". Carga la escena de reintento.
    /// </summary>
    public void RetryGame()
    {
        // 1. RESTAURAR EL TIEMPO
        Time.timeScale = 1f;

        // 2. RESTAURAR EL CURSOR (Bloquearlo de nuevo para el juego)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 3. Cargar la escena especificada (por defecto "Level 1")
        SceneManager.LoadScene(retrySceneName);
    }

    /// <summary>
    /// Función llamada por el botón "Salir". Carga la escena de Menú Principal.
    /// </summary>
    public void QuitGame()
    {
        // 1. RESTAURAR EL TIEMPO
        Time.timeScale = 1f;

        // 2. RESTAURAR EL CURSOR (Bloquearlo antes de cargar la escena)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 3. CARGAR LA ESCENA DE MENÚ PRINCIPAL
        SceneManager.LoadScene(mainMenuSceneName);
    }
}