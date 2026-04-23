using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    // ----------------------------------------------------
    // ESTADOS Y FASES DEL JEFE
    // ----------------------------------------------------
    public enum State { Idle, Chase, Attack, Ability }
    [Header("State Control")]
    public State currentState;

    public enum BossPhase { Phase1, Phase2_Ability, Phase3_Enraged }
    [Header("Phase Control")]
    public BossPhase currentPhase;

    private NavMeshAgent agent;
    private Animator animator;

    [Header("Target & Detection")]
    public Transform player;
    public float chaseDistance = 15f;

    [Header("Phase Transition Settings")]
    public float phase2Threshold = 0.7f;
    public float phase3Threshold = 0.3f;

    // --- Audio Settings (NUEVO) ---
    [Header("Audio Settings")]
    [Tooltip("Sonido que se reproduce al realizar un ataque normal.")]
    public AudioClip normalAttackClip;
    [Tooltip("Sonido que se reproduce al usar la habilidad especial de Fase 2.")]
    public AudioClip specialAbilityClip;
    [Tooltip("Sonido de rugido o gruñido ambiental.")]
    public AudioClip roarClip;

    private AudioSource audioSource;
    public float roarInterval = 10.0f;
    private float nextRoarTime;
    // ------------------------------------

    // --- Habilidad de la Fase 2 ---
    [Header("Phase 2 - Heavy Ability")]
    public float heavyAbilityCooldown = 8.0f;
    public string abilityTrigger = "HeavyAttack";
    private float nextAbilityTime;

    [Header("Movement & Attack Settings")]
    public float runSpeed_P1 = 6.0f;
    public float enragedSpeed_P3 = 10.0f;
    public float attackDistance = 2.0f;
    public float attackCooldown = 2.0f;
    public int attackDamage = 10;
    private float nextAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // >>> NUEVO: Inicializar AudioSource <<<
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1.0f; // Importante para que el sonido se escuche en 3D
            audioSource.playOnAwake = false;
        }
        nextRoarTime = Time.time + roarInterval;
        // ------------------------------------

        // 1. Inicialización y búsqueda del jugador
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null) player = playerObject.transform;
            else Debug.LogError("¡El jugador no fue encontrado! Asegúrate del Tag 'Player'.");
        }

        // 2. Configurar el agente
        if (agent != null)
        {
            agent.stoppingDistance = attackDistance - agent.radius - 0.2f;
            agent.speed = runSpeed_P1;
        }

        // 3. Inicio en Fase 1
        currentPhase = BossPhase.Phase1;
        currentState = State.Chase;
        nextAttackTime = Time.time;
        nextAbilityTime = Time.time + heavyAbilityCooldown;
    }

    void Update()
    {
        if (player == null || agent == null || animator == null) return;

        CheckPhaseTransition();

        switch (currentState)
        {
            case State.Chase:
                ChaseLogic();
                break;
            case State.Attack:
                AttackLogic();
                break;
            case State.Ability:
                AbilityLogic();
                break;
            case State.Idle:
                break;
        }

        UpdateAnimations();

        // >>> NUEVO: Lógica de Rugido Ambiental <<<
        HandleRoarSound();
        // ---------------------------------------
    }

    // ----------------------------------------------------
    // LÓGICA DE FASES Y TRANSICIONES (VACÍA/NEUTRALIZADA)
    // ----------------------------------------------------
    private void CheckPhaseTransition()
    {
        // Esta función se mantiene vacía por ahora
    }

    // ----------------------------------------------------
    // LÓGICA DE ESTADOS Y MOVIMIENTO
    // ----------------------------------------------------
    private void ChaseLogic()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackDistance)
        {
            currentState = State.Attack;
            return;
        }
        if (currentPhase == BossPhase.Phase2_Ability && Time.time >= nextAbilityTime && distanceToPlayer > attackDistance)
        {
            currentState = State.Ability;
            return;
        }

        if (agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
            }
        }
    }

    private void AttackLogic()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackDistance)
        {
            currentState = State.Chase;
            return;
        }

        if (Time.time >= nextAttackTime)
        {
            // >>> NUEVO: Llamada al sonido de ataque normal <<<
            PlaySound(normalAttackClip);

            ExecuteAttackAnimation("attack" + Random.Range(1, 5));
            nextAttackTime = Time.time + attackCooldown;
        }
        RotateTowardsTarget(30f);
    }

    private void AbilityLogic()
    {
        RotateTowardsTarget(50f);

        // >>> NUEVO: Llamada al sonido de habilidad especial <<<
        PlaySound(specialAbilityClip);

        ExecuteAttackAnimation(abilityTrigger);
        nextAbilityTime = Time.time + heavyAbilityCooldown;
        currentState = State.Chase;
    }

    private void RotateTowardsTarget(float rotationSpeed)
    {
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void ExecuteAttackAnimation(string triggerName)
    {
        if (agent.enabled) agent.ResetPath();
        animator.speed = 1.0f;
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    private void UpdateAnimations()
    {
        Vector3 velocity = agent.velocity;
        Vector3 flatVelocity = new Vector3(velocity.x, 0, velocity.z);
        float currentSpeed = flatVelocity.magnitude;
        animator.SetFloat("Speed", currentSpeed);
    }

    // ----------------------------------------------------
    // FUNCIONES DE AUDIO (NUEVAS)
    // ----------------------------------------------------
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            // Usa PlayOneShot para que no interrumpa otros sonidos (como el rugido ambiental)
            audioSource.PlayOneShot(clip);
        }
    }

    private void HandleRoarSound()
    {
        // Solo ruge si está persiguiendo (Chase) o quieto (Idle)
        if (currentState == State.Chase && Time.time >= nextRoarTime)
        {
            PlaySound(roarClip);
            // Establece el próximo tiempo de rugido con una pequeña variación
            nextRoarTime = Time.time + roarInterval + Random.Range(-2f, 2f);
        }
    }

    // ----------------------------------------------------
    // FUNCIONES DE MOVIMIENTO, DAÑO Y MUERTE
    // ----------------------------------------------------
    public void DisableMovement()
    {
        Debug.Log("BOSS: ¡Golpe recibido, pero sigo persiguiendo!");
    }
    public void EnableMovement()
    {
        if (animator != null)
        {
            animator.speed = 1.0f;
        }
    }
    public void InflictDamage()
    {
        if (player == null) return;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackDistance)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    /// <summary>
    /// Esta función debe ser llamada desde Target.cs cuando la vida del Boss llega a cero.
    /// </summary>
    public void Die()
    {
        Debug.Log("BOSS DERROTADO. Llamando a la pantalla de victoria.");

        if (audioSource != null)
        {
            audioSource.Stop(); // Detiene cualquier sonido de batalla/gruñido
        }

        if (agent != null && agent.enabled) agent.isStopped = true;

        // Desactivamos el propio objeto Boss
        gameObject.SetActive(false);

        // Llama al gestor de la pantalla de victoria (esto es lo que arregla el panel)
        // Recordatorio: WinManager.ShowWinScreen() debe manejar su propia música.
        WinManager.ShowWinScreen();
    }
}