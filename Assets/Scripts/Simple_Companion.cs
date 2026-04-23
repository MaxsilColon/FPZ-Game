using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Compañero simple que sigue al jugador. Sin cámaras, sin complicaciones.
/// </summary>
public class Simple_Companion : MonoBehaviour
{
    [Header("Configuración")]
    public float followDistance = 3f;
    public float moveSpeed = 3.5f;
    
    private Transform player;
    private NavMeshAgent navAgent;
    
    void Start()
    {
        // Buscar al jugador
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        // Configurar NavMeshAgent
        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent != null)
        {
            navAgent.speed = moveSpeed;
            navAgent.stoppingDistance = followDistance;
        }
        
        // Eliminar cámara si tiene una
        Camera companionCamera = GetComponentInChildren<Camera>();
        if (companionCamera != null)
        {
            companionCamera.gameObject.SetActive(false);
        }
        
        // Eliminar AudioListener si tiene uno
        AudioListener audioListener = GetComponentInChildren<AudioListener>();
        if (audioListener != null)
        {
            audioListener.enabled = false;
        }
        
        Debug.Log("Compañero simple creado - seguirá al jugador");
    }
    
    void Update()
    {
        if (player != null && navAgent != null)
        {
            // Calcular distancia al jugador
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            // Si está muy lejos, seguir al jugador
            if (distanceToPlayer > followDistance)
            {
                navAgent.SetDestination(player.position);
            }
            else
            {
                // Si está cerca, parar
                navAgent.ResetPath();
            }
        }
    }
}