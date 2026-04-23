using UnityEngine;
using UnityEngine.AI;
// Necesario para las funciones de AudioSource/AudioClip
using Random = UnityEngine.Random;

public class ZombieAI : MonoBehaviour
{
    // ----------------------------------------------------
    // ESTADOS DISPONIBLES DEL ZOMBIE
    // ----------------------------------------------------
    public enum State { Idle, Chase, Attack }
    [Header("State Control")]
    [Tooltip("El estado actual del zombie.")]
    public State currentState;

    // Se hacen privados para asegurar que se obtengan en Start()
    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource; // REFERENCIA DE AUDIO

    [Header("Target & Detection")]
    [Tooltip("Referencia al objeto Transform del jugador. Debe estar vacío en el Prefab.")]
    public Transform player;
    // La distancia de persecución no se usa actualmente en la lógica, pero se mantiene.
    public float chaseDistance = 10f;

    [Header("Speed Control")]
    public float runSpeed = 6.0f;

    [Header("Attack Settings")]
    public float attackDistance = 2.0f;
    public float attackCooldown = 2.0f;
    public int attackDamage = 10;
    private float nextAttackTime;

    [Header("Audio Settings")] // NUEVO: Configuración de Audio
    [Tooltip("Sonido de ataque o gruñido que se reproduce al ejecutar la animación de ataque.")]
    public AudioClip attackSoundClip;

    [Header("Animation Control")]
    [Tooltip("Controla la velocidad del Animator durante el ataque (Ej: 2.0 es el doble de rápido).")]
    public float attackAnimSpeed = 1.5f;
    [Tooltip("Velocidad de rotación al perseguir.")]
    public float chaseRotationSpeed = 5f;
    [Tooltip("Velocidad de rotación al atacar (más alta para ser casi inmediata).")]
    public float attackRotationSpeed = 30f;

    void Start()
    {
        // 1. Obtener Componentes
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // CORRECCIÓN: Manejo de AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1.0f;
            audioSource.playOnAwake = false;
        }

        // 2. Encontrar al jugador usando el Tag "Player" (Optimizado)
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                // CORRECCIÓN: Usar Debug.LogWarning en lugar de Error, ya que el juego puede continuar
                Debug.LogWarning("¡El jugador no fue encontrado! Asegúrate de que el Tag 'Player' sea correcto.");
            }
        }

        // 3. Configurar el agente y la detención
        if (agent != null)
        {
            // CORRECCIÓN: Uso de remainingDistance para verificar la detención en ChaseLogic.
            // La propiedad stoppingDistance se ajusta correctamente:
            float agentRadius = agent.radius;
            agent.stoppingDistance = attackDistance - agentRadius - 0.2f;
            agent.speed = runSpeed;
        }

        // *** COMIENZA INMEDIATAMENTE A PERSEGUIR/CORRER! ***
        currentState = State.Chase;
        nextAttackTime = Time.time;
    }

    void Update()
    {
        // CORRECCIÓN: Si el jugador muere y el Transform es nulo, evita errores de NullReference.
        if (player == null || agent == null || animator == null)
        {
            // Si falta el jugador o un componente clave, detiene el movimiento/animaciones y sale.
            if (agent != null && agent.enabled) agent.isStopped = true;
            if (animator != null) animator.SetFloat("Speed", 0f);
            return;
        }

        // Si el agente está deshabilitado (durante ataque o GetHit), solo se actualiza la animación de reposo/daño.
        if (!agent.enabled)
        {
            UpdateAnimations();
            return;
        }

        // Ejecutar la lógica de la máquina de estados
        switch (currentState)
        {
            case State.Idle:
                // Si accidentalmente entra en Idle, pasa a Chase si el jugador está cerca.
                if (Vector3.Distance(transform.position, player.position) < chaseDistance + 2f)
                {
                    currentState = State.Chase;
                }
                break;

            case State.Chase:
                ChaseLogic();
                break;

            case State.Attack:
                AttackLogic();
                break;
        }

        UpdateAnimations();
    }

    // ----------------------------------------------------
    // LÓGICA DE ESTADOS
    // ----------------------------------------------------

    private void ChaseLogic()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Transición: Si está en rango de ataque, pasa a Attack
        if (distanceToPlayer <= attackDistance)
        {
            currentState = State.Attack;
            return;
        }

        // Acción: Perseguir
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = false; // Asegura que el agente se mueva
            agent.SetDestination(player.position);

            if (agent.speed != runSpeed)
            {
                agent.speed = runSpeed;
            }

            // Rotación suave
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * chaseRotationSpeed);
            }
        }
    }

    private void AttackLogic()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // CORRECCIÓN: Detener movimiento mientras está en estado de ataque
        if (agent.enabled)
        {
            agent.isStopped = true;
        }

        // Transición: Si el jugador se aleja, pasa a Chase
        if (distanceToPlayer > attackDistance * 1.05f) // Pequeña tolerancia para evitar spam de estados
        {
            currentState = State.Chase;
            return;
        }

        // Rotación inmediata al jugador (prioridad)
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * attackRotationSpeed);
        }

        // Acción: Atacar (controlado por cooldown)
        if (Time.time >= nextAttackTime)
        {
            ExecuteAttackAnimation();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    // ----------------------------------------------------
    // FUNCIONES AUXILIARES (Animación, Daño y Audio)
    // ----------------------------------------------------

    private void ExecuteAttackAnimation()
    {
        // CORRECCIÓN: ResetPath() no es necesario si usamos isStopped = true en AttackLogic().
        // Sin embargo, PlayAttackSound() y la lógica de animación son esenciales.

        // 1. >>> REPRODUCIR SONIDO DE ATAQUE <<<
        PlayAttackSound();

        // 2. APLICAR VELOCIDAD DE ANIMACIÓN DE ATAQUE
        if (animator != null)
        {
            animator.speed = attackAnimSpeed;
        }

        // 3. Elegir ataque al azar
        // Nota: Asegúrate de que los parámetros 'attack1' a 'attack4' existan en tu Animator.
        int randomAttackIndex = Random.Range(1, 5);
        string attackTriggerName = "attack" + randomAttackIndex;
        animator.SetTrigger(attackTriggerName);
    }

    private void PlayAttackSound() // IMPLEMENTACIÓN DE AUDIO
    {
        if (audioSource != null && attackSoundClip != null)
        {
            // PlayOneShot permite que el audio del ataque se superponga si hay otros sonidos de fondo.
            audioSource.PlayOneShot(attackSoundClip);
        }
    }

    private void UpdateAnimations()
    {
        // Si el agente está deshabilitado o detenido, la velocidad es 0.
        if (!agent.enabled || agent.isStopped)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        // Mide la velocidad actual (proyección en el plano XZ)
        Vector3 velocity = agent.velocity;
        Vector3 flatVelocity = new Vector3(velocity.x, 0, velocity.z);
        float currentSpeed = flatVelocity.magnitude;

        animator.SetFloat("Speed", currentSpeed);
    }

    // ----------------------------------------------------
    // FUNCIONES LLAMADAS POR EVENTOS (Animación / Target.cs)
    // ----------------------------------------------------

    /// <summary>
    /// Deshabilita el NavMeshAgent. Llamado al inicio de animaciones de Ataque o GetHit (por Target.cs).
    /// </summary>
    public void DisableMovement()
    {
        if (agent != null)
        {
            agent.enabled = false;
            // Detener las animaciones si no hay movimiento
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }
        }
    }

    /// <summary>
    /// Habilita el NavMeshAgent. Llamado al final de las animaciones de Ataque o GetHit (por Evento de Animación).
    /// </summary>
    public void EnableMovement()
    {
        if (agent != null)
        {
            agent.enabled = true;
            agent.isStopped = false; // Asegura que pueda reanudar el movimiento
        }
        // *** RESETEAR VELOCIDAD DE ANIMACIÓN A NORMAL ***
        if (animator != null)
        {
            animator.speed = 1.0f;
        }
        // Tras ser golpeado, vuelve a la persecución
        currentState = State.Chase;
    }

    /// <summary>
    /// Lógica de daño real: Llama al script PlayerHealth del jugador. 
    /// Esta función debe ser llamada por un Evento de Animación (en el momento de impacto).
    /// </summary>
    public void InflictDamage()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Confirma que el jugador sigue en rango en el momento del impacto
        if (distanceToPlayer <= attackDistance)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogWarning("El jugador no tiene el componente 'PlayerHealth'. No se puede infligir daño.");
            }
        }
    }
}