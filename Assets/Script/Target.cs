using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    [Header("Health & Score")]
    [Tooltip("Salud actual del objetivo.")]
    public float health = 50f;
    [Tooltip("Puntos que otorga al ser destruido.")]
    public int scoreValue = 10;

    [Tooltip("Tiempo en segundos que tarda en desaparecer después de morir.")]
    public float destroyDelay = 2.0f;

    // --- Referencias de Componentes ---
    private Animator animator;
    private NavMeshAgent agent;

    // Referencia general a la IA (Puede ser ZombieAI o BossAI si se implementa una interfaz)
    // Usaremos BossAI para la lógica de victoria y ZombieAI para el stagger.
    private BossAI bossAI;
    private ZombieAI zombieAI;

    private bool isBoss = false; // Bandera para saber si es el Jefe

    void Start()
    {
        // Obtener componentes al inicio
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // >>> VERIFICACIÓN Y ASIGNACIÓN DE IA <<<
        // Comprobar si tiene el script del Boss
        bossAI = GetComponent<BossAI>();
        if (bossAI != null)
        {
            isBoss = true;
            Debug.Log("TARGET: Detectado como Jefe. La muerte activará la pantalla de victoria.");
        }
        else
        {
            // Si no es Boss, asumimos que es un enemigo normal con ZombieAI
            zombieAI = GetComponent<ZombieAI>();
            if (zombieAI == null)
            {
                Debug.LogError("Target.cs requiere el componente BossAI O ZombieAI en el mismo GameObject.");
            }
        }
    }

    /// <summary>
    /// Resta daño a la salud del objetivo y comprueba si debe morir.
    /// </summary>
    /// <param name="amount">La cantidad de daño a restar.</param>
    public void TakeDamage(float amount)
    {
        // Ignora el daño si ya está muerto o en proceso de morir
        if (health <= 0f) return;

        health -= amount;

        // 1. DETENER AL ENEMIGO/BOSS al ser golpeado (Stagger)
        if (isBoss && bossAI != null)
        {
            // El Boss es implacable, pero llamamos a la función de golpe para animación
            bossAI.DisableMovement();
        }
        else if (!isBoss && zombieAI != null)
        {
            // Llama a la función en ZombieAI que deshabilita el NavMeshAgent
            zombieAI.DisableMovement();
        }

        // 2. Activar animación de Ser Golpeado
        if (animator != null)
        {
            // "GetHit" debe ser un Trigger en tu Animator Controller
            animator.SetTrigger("GetHit");
        }

        Debug.Log(gameObject.name + " golpeado. Vida restante: " + health);

        // 3. Comprobar si ha muerto
        if (health <= 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// Ejecuta la lógica final de la muerte del objetivo.
    /// </summary>
    void Die()
    {
        // Aseguramos que la vida sea exactamente cero
        health = 0f;

        // 1. DESACTIVAR MOVIMIENTO E IA PERMANENTEMENTE
        if (agent != null)
        {
            agent.enabled = false;
        }

        // Desactivar el script de IA (ZombieAI o BossAI) para que no siga ejecutando lógica.
        if (bossAI != null)
        {
            bossAI.enabled = false;
        }
        else if (zombieAI != null)
        {
            zombieAI.enabled = false;
        }

        // Desactivar el Collider (para que el cuerpo no estorbe)
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // 2. Activar animación de Muerte
        if (animator != null)
        {
            // "Death" debe ser un Trigger en tu Animator Controller
            animator.SetTrigger("Death");
        }

        // 3. LÓGICA DE VICTORIA Y PUNTOS

        // *** ¡SOLUCIÓN DE PANTALLA DE VICTORIA! ***
        // Si el objeto es el Boss, activa la pantalla de victoria.
        if (isBoss && bossAI != null)
        {
            // Usamos la función Die del BossAI, que llama a WinManager.ShowWinScreen()
            bossAI.Die();
            // Como el BossAI.Die() ya llama a Destroy(gameObject), no lo hacemos aquí
            return;
        }

        // Si es un enemigo común, solo suma puntos.
        // Asumiendo que ScoreManager es un Singleton.
        if (ScoreManager.Instance != null)
        {
            // NOTA: Si no tienes ScoreManager, quita este bloque.
            ScoreManager.Instance.AddScore(scoreValue);
        }

        // 4. Destruir el objeto después del retraso de la animación (SOLO PARA ENEMIGOS COMUNES)
        Destroy(gameObject, destroyDelay);
    }
}