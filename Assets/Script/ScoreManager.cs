using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // <<< 1. NECESARIO PARA COROUTINES

public class ScoreManager : MonoBehaviour
{
    // Patrón Singleton: Permite acceder a este script desde cualquier lugar.
    public static ScoreManager Instance;

    [Header("Score")]
    public int currentScore = 0;
    [Tooltip("Puntuación requerida para pasar de nivel.")]
    public int targetScore = 50;

    [Header("Level Management")]
    [Tooltip("Nombre de la escena a cargar cuando se alcance la puntuación objetivo.")]
    public string nextSceneName = "Nivel2";

    [Header("UI Reference")]
    [Tooltip("Arrastra aquí el componente Text Mesh 3D para el puntaje actual.")]
    public TextMesh scoreText;

    [Tooltip("Arrastra aquí el componente Text Mesh 3D para mostrar el puntaje objetivo.")]
    public TextMesh targetScoreDisplay;

    void Awake()
    {
        // Inicialización del Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        UpdateScoreDisplay();
        InitializeTargetDisplay();

        // <<< 3. INICIA LA COROUTINE AQUÍ >>>
        StartCoroutine(HideGoalAfterDelay(2f));
    }

    // <<< 2. NUEVA FUNCIÓN DE COROUTINE >>>
    /// <summary>
    /// Espera un tiempo y luego oculta el texto de la meta.
    /// </summary>
    IEnumerator HideGoalAfterDelay(float delay)
    {
        // Pausa la ejecución por la cantidad de segundos especificada
        yield return new WaitForSeconds(delay);

        // Desactiva el GameObject que contiene el texto de la meta
        if (targetScoreDisplay != null)
        {
            targetScoreDisplay.gameObject.SetActive(false);
        }
    }
    // <<< FIN COROUTINE >>>

    /// <summary>
    /// Muestra el puntaje necesario para completar el nivel (al inicio).
    /// </summary>
    void InitializeTargetDisplay()
    {
        if (targetScoreDisplay != null)
        {
            targetScoreDisplay.text = "Goal: " + targetScore.ToString();
            // Aseguramos que esté activo al inicio para que se muestre por 1 segundo
            targetScoreDisplay.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Suma puntos a la puntuación total y comprueba si se alcanzó la meta.
    /// </summary>
    /// <param name="points">Los puntos a sumar.</param>
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();

        if (currentScore >= targetScore)
        {
            LoadNextLevel();
        }
    }

    /// <summary>
    /// Actualiza el texto en la interfaz (UI) con la puntuación actual.
    /// </summary>
    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }
    }

    /// <summary>
    /// Intenta cargar la siguiente escena.
    /// </summary>
    void LoadNextLevel()
    {
        Debug.Log("¡Puntuación objetivo alcanzada! Cargando siguiente nivel: " + nextSceneName);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("El campo 'Next Scene Name' está vacío.");
        }
    }
}