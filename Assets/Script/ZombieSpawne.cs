using UnityEngine;
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject zombiePrefab;

    [Tooltip("Tiempo base (inicial) entre cada aparición.")]
    public float baseTimeBetweenSpawns = 5f;

    [Tooltip("Tiempo actual entre spawns (se reduce con la dificultad).")]
    private float timeBetweenSpawns;

    [Tooltip("Máximo de zombies que pueden estar activos en la escena.")]
    public int maxZombiesInScene = 10;

    // Timer para controlar cuándo debe aparecer el próximo zombie
    private float spawnTimer;

    [Header("Difficulty Scaling")]
    [Tooltip("Cada cuánto tiempo (segundos) la dificultad debe aumentar (ej: 60s = 1 minuto).")]
    public float difficultyInterval = 60f;

    [Tooltip("Factor para reducir el tiempo de spawn (ej: 0.9 reducirá el tiempo en 10%).")]
    public float difficultyMultiplier = 0.9f;

    [Tooltip("El tiempo mínimo al que puede llegar el spawn rate (ej: no más rápido de 1 segundo).")]
    public float minSpawnTime = 1f;

    private float difficultyTimer; // Contador para saber cuándo subir la dificultad

    [Header("Spawn Locations")]
    public Transform[] spawnPoints;

    // *** ¡MODIFICADO! Usamos "Target" para contar los enemigos ***
    private string enemyTag = "Target";

    void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No hay puntos de spawn asignados al Spawner.");
            enabled = false;
            return;
        }

        // Inicializar el sistema:
        timeBetweenSpawns = baseTimeBetweenSpawns;
        spawnTimer = timeBetweenSpawns;
        difficultyTimer = difficultyInterval;
    }

    void Update()
    {
        // 1. Control del Spawn
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy(); // Llamamos a la función con el nuevo nombre
            spawnTimer = timeBetweenSpawns;
        }

        // 2. Control de Dificultad (El contador de dificultad)
        difficultyTimer -= Time.deltaTime;
        if (difficultyTimer <= 0f)
        {
            IncreaseDifficulty();
            difficultyTimer = difficultyInterval;
        }
    }

    /// <summary>
    /// Spawnea un enemigo, comprobando el límite máximo usando el Tag "Target".
    /// </summary>
    void SpawnEnemy()
    {
        // Cuenta los enemigos con el Tag "Target"
        // Nota: cambié el nombre de la variable local a 'enemyTag' para ser más general.
        if (zombiePrefab == null)
        {
            Debug.LogError("ERROR: El Prefab del Zombie no está asignado o ha sido destruido. Revisa el Inspector.");
            return;
        }

        int currentEnemyCount = GameObject.FindGameObjectsWithTag(enemyTag).Length;

        if (currentEnemyCount < maxZombiesInScene)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];

            // Instancia el prefab del zombie en el punto de spawn
            Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    /// <summary>
    /// Reduce el tiempo entre spawns multiplicándolo por el factor de dificultad.
    /// </summary>
    void IncreaseDifficulty()
    {
        // Aplicar el multiplicador (ej: 5s * 0.9 = 4.5s)
        float newTime = timeBetweenSpawns * difficultyMultiplier;

        // Limitar la reducción al tiempo mínimo (ej: no menor a 1 segundo)
        timeBetweenSpawns = Mathf.Max(newTime, minSpawnTime);

        Debug.Log("Dificultad Aumentada! Nuevo tiempo de spawn: " + timeBetweenSpawns.ToString("F2") + "s");
    }
}