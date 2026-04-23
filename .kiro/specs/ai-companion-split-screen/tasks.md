# Implementation Plan: AI Companion Split-Screen System

## Overview

Este plan de implementación desglosa el diseño del sistema de split-screen con compañero IA en tareas discretas de codificación. El sistema permitirá que un jugador humano y un compañero controlado por IA compartan la pantalla con vistas independientes, donde el compañero IA ayudará activamente en el combate contra zombies usando el sistema Easy FPS existente.

## Tasks

- [x] 1. Configurar estructura base del proyecto y NavMesh
  - [x] 1.1 Configurar NavMesh en la escena zombie.unity
    - Abrir la escena zombie.unity
    - Marcar superficies caminables como "Navigation Static"
    - Configurar NavMesh baking con agent radius 0.5, height 2.0, max slope 45
    - Bake NavMesh y verificar cobertura del área de juego
    - _Requirements: 3.1, 3.4, 8.5_
  
  - [x] 1.2 Crear estructura de carpetas para scripts del sistema
    - Crear carpeta Assets/Scripts/SplitScreen/
    - Crear subcarpetas: Managers/, AI/, Camera/
    - _Requirements: 10.1_

- [x] 2. Implementar Split_Screen_Manager
  - [x] 2.1 Crear script Split_Screen_Manager.cs
    - Crear clase con propiedades: playerOnePrefab, aiCompanionPrefab, spawn points
    - Agregar propiedades de configuración de cámara: fieldOfView (17.4f), cullingMask
    - Agregar propiedades de spawn: minSpawnDistance (2f), aiSpawnOffset
    - Agregar referencias privadas: playerOneInstance, aiCompanionInstance, cameras
    - _Requirements: 1.1, 2.1, 6.1, 6.2, 10.3_
  
  - [x] 2.2 Implementar método InitializeSplitScreen()
    - Validar que playerOnePrefab y aiCompanionPrefab no sean null
    - Llamar a SpawnPlayers()
    - Llamar a ConfigureCameraViewports()
    - Agregar manejo de errores con Debug.LogError si falla
    - _Requirements: 2.1, 10.3_
  
  - [x] 2.3 Implementar método SpawnPlayers()
    - Instanciar playerOnePrefab en playerOneSpawnPoint
    - Calcular posición de spawn para AI con offset (2 unidades mínimo)
    - Validar que la posición de spawn del AI esté en NavMesh usando NavMesh.SamplePosition
    - Instanciar aiCompanionPrefab en posición calculada
    - Obtener referencias a las cámaras de ambos jugadores
    - _Requirements: 2.1, 2.2, 2.3, 2.4_
  
  - [x] 2.4 Implementar método ConfigureCameraViewports()
    - Configurar viewport de playerOneCamera: Rect(0, 0, 0.5f, 1f), depth 0
    - Configurar viewport de aiCompanionCamera: Rect(0.5f, 0, 0.5f, 1f), depth 1
    - Establecer fieldOfView a 17.4 para ambas cámaras
    - Configurar cullingMask para ambas cámaras
    - Deshabilitar AudioListener en aiCompanionCamera si existe
    - Validar que solo haya un AudioListener activo
    - _Requirements: 1.2, 1.3, 1.5, 6.1, 6.2, 6.3, 6.4, 6.5, 6.6, 9.2_
  
  - [x] 2.5 Agregar Split_Screen_Manager a la escena zombie.unity
    - Crear GameObject vacío llamado "SplitScreenManager"
    - Agregar componente Split_Screen_Manager
    - Asignar referencias a prefabs y spawn points en el Inspector
    - Llamar a InitializeSplitScreen() en Start()
    - _Requirements: 1.4, 10.3_

- [x] 3. Checkpoint - Verificar configuración de split-screen
  - Ensure all tests pass, ask the user if questions arise.

- [x] 4. Crear prefab AI_Companion
  - [x] 4.1 Duplicar prefab Player existente
    - Duplicar Assets/Prefabs/Player.prefab
    - Renombrar a "AI_Companion.prefab"
    - Cambiar tag a "AICompanion" (crear tag si no existe)
    - _Requirements: 2.2, 2.5_
  
  - [x] 4.2 Agregar componente NavMeshAgent al prefab AI_Companion
    - Agregar NavMeshAgent con speed 3.5, angular speed 120
    - Configurar stopping distance 2.0, acceleration 8
    - Configurar radius 0.5, height 2.0
    - _Requirements: 3.1, 3.6_
  
  - [x] 4.3 Modificar componentes de control en AI_Companion
    - Deshabilitar PlayerMovementScript (el NavMeshAgent controlará el movimiento)
    - Deshabilitar MouseLookScript (la IA controlará la rotación)
    - Remover AudioListener del componente Camera
    - _Requirements: 2.5, 9.2_

- [x] 5. Implementar AI_Behavior_Controller
  - [x] 5.1 Crear script AI_Behavior_Controller.cs con enum AIState
    - Crear enum AIState: Idle, Following, Combat, Retreating
    - Crear clase con propiedades: playerOne, navAgent, gunScript
    - Agregar settings: updateInterval (0.2f), combatRange (15f), followDistance (4f)
    - Agregar propiedades de salud: maxHealth (100f), currentHealth, lowHealthThreshold (30f)
    - Agregar variables privadas: currentState, lastUpdateTime
    - _Requirements: 3.2, 4.1, 7.1_
  
  - [x] 5.2 Implementar método UpdateAIState()
    - Validar referencia a playerOne (buscar si es null)
    - Implementar lógica de transición de estados basada en salud y enemigos cercanos
    - Idle → Following cuando playerOne existe
    - Following → Combat cuando detecta enemigos en combatRange
    - Combat → Retreating cuando currentHealth < lowHealthThreshold
    - Retreating → Following cuando currentHealth > lowHealthThreshold y no hay enemigos cerca
    - _Requirements: 3.2, 7.4_
  
  - [x] 5.3 Implementar método TakeDamage(float damage)
    - Reducir currentHealth por damage
    - Clamp currentHealth entre 0 y maxHealth
    - Si currentHealth <= 0, desactivar GameObject y detener comportamiento
    - Si currentHealth < lowHealthThreshold, forzar estado Retreating
    - _Requirements: 7.2, 7.3_
  
  - [x] 5.4 Implementar Update() con intervalo de actualización
    - Verificar si Time.time - lastUpdateTime >= updateInterval
    - Si es tiempo de actualizar, llamar a UpdateAIState()
    - Actualizar lastUpdateTime
    - _Requirements: 8.2_
  
  - [x] 5.5 Agregar AI_Behavior_Controller al prefab AI_Companion
    - Agregar componente al prefab
    - Asignar referencias en Start(): navAgent, gunScript
    - playerOne se asignará dinámicamente desde Split_Screen_Manager
    - _Requirements: 2.5_

- [x] 6. Implementar AI_Movement_System
  - [x] 6.1 Crear script AI_Movement_System.cs
    - Crear clase con propiedades: navAgent, playerOne, currentTarget
    - Agregar settings: moveSpeed (3.5f), rotationSpeed (5f), stoppingDistance (2f)
    - Agregar combatPositionDistance (10f)
    - _Requirements: 3.1, 3.6_
  
  - [x] 6.2 Implementar método FollowPlayer()
    - Calcular distancia a playerOne
    - Si distancia > followDistanceMax (5f), mover hacia playerOne
    - Si distancia < followDistance (3f), detener movimiento
    - Usar navAgent.SetDestination() para mover
    - Llamar a LookAtTarget(playerOne) para rotar hacia el jugador
    - _Requirements: 3.2, 3.5_
  
  - [x] 6.3 Implementar método MoveToCombaPosition(Transform target)
    - Calcular posición óptima: entre el AI y el target a combatPositionDistance
    - Validar que la posición esté en NavMesh usando NavMesh.SamplePosition
    - Si es válida, usar navAgent.SetDestination()
    - Si no es válida, buscar posición alternativa cercana
    - _Requirements: 3.3, 3.4_
  
  - [x] 6.4 Implementar método LookAtTarget(Transform target)
    - Calcular dirección hacia el target
    - Usar Quaternion.Slerp para rotación suave con rotationSpeed
    - Aplicar rotación solo en el eje Y (mantener upright)
    - _Requirements: 3.5_
  
  - [x] 6.5 Implementar método IsDestinationReachable(Vector3 destination)
    - Usar NavMeshPath para calcular path
    - Retornar true si path.status == NavMeshPathStatus.PathComplete
    - Retornar false en caso contrario
    - _Requirements: 3.4_
  
  - [x] 6.6 Agregar AI_Movement_System al prefab AI_Companion
    - Agregar componente al prefab
    - Asignar referencias en Start(): navAgent
    - playerOne y currentTarget se asignarán dinámicamente
    - _Requirements: 3.1_

- [x] 7. Checkpoint - Verificar movimiento del AI
  - Ensure all tests pass, ask the user if questions arise.

- [x] 8. Implementar AI_Target_Selection_System
  - [x] 8.1 Crear script AI_Target_Selection_System.cs
    - Crear clase con propiedades: playerOne, aiTransform
    - Agregar settings: scanInterval (0.3f), threatRadius (15f), playerThreatRadius (5f)
    - Agregar zombieLayer LayerMask
    - Agregar variables privadas: detectedEnemies (List), currentTarget, lastScanTime
    - _Requirements: 4.1, 5.1_
  
  - [x] 8.2 Implementar método ScanForEnemies()
    - Usar Physics.OverlapSphereNonAlloc con buffer de Collider[20]
    - Buscar enemigos en threatRadius con zombieLayer
    - Limpiar y actualizar lista detectedEnemies
    - Filtrar enemigos que no estén activos o destruidos
    - _Requirements: 4.1, 5.1_
  
  - [x] 8.3 Implementar método CalculateThreatPriority(Transform enemy)
    - Calcular distancia del enemigo a playerOne
    - Calcular distancia del enemigo al AI
    - Si enemigo está a < 5 unidades de playerOne, retornar prioridad máxima (100)
    - Si no, calcular score basado en: 50 / distanceToAI + 30 / distanceToPlayer
    - Agregar bonus si tiene línea de visión (+20)
    - _Requirements: 5.2, 5.3_
  
  - [x] 8.4 Implementar método SelectBestTarget()
    - Iterar sobre detectedEnemies
    - Calcular CalculateThreatPriority() para cada enemigo
    - Seleccionar enemigo con mayor threatScore
    - Verificar línea de visión con Physics.Raycast
    - Retornar el mejor target o null si no hay válidos
    - _Requirements: 4.3, 5.2, 5.4_
  
  - [x] 8.5 Implementar Update() con intervalo de escaneo
    - Verificar si Time.time - lastScanTime >= scanInterval
    - Si es tiempo, llamar a ScanForEnemies() y SelectBestTarget()
    - Actualizar lastScanTime
    - _Requirements: 5.1, 8.2_
  
  - [x] 8.6 Agregar AI_Target_Selection_System al prefab AI_Companion
    - Agregar componente al prefab
    - Asignar referencias en Start(): aiTransform (transform)
    - Configurar zombieLayer en el Inspector
    - playerOne se asignará dinámicamente
    - _Requirements: 4.1_

- [x] 9. Implementar AI_Combat_System
  - [x] 9.1 Crear script AI_Combat_System.cs
    - Crear clase con propiedades: gunScript, firePoint, enemyLayer
    - Agregar settings: detectionRange (15f), fireRateMin (0.5f), fireRateMax (1.0f)
    - Agregar aimAccuracy (0.9f)
    - Agregar variables privadas: nextFireTime, currentTarget, canShoot
    - _Requirements: 4.1, 4.4_
  
  - [x] 9.2 Implementar método HasLineOfSight(Transform target)
    - Calcular dirección desde firePoint hacia target
    - Usar Physics.Raycast desde firePoint hacia target
    - Verificar si el raycast golpea al target (comparar collider)
    - Retornar true si hay línea de visión directa, false si hay obstáculos
    - _Requirements: 4.2_
  
  - [x] 9.3 Implementar método AimAtTarget(Transform target)
    - Calcular dirección hacia el target desde firePoint
    - Agregar pequeño offset aleatorio basado en aimAccuracy
    - Rotar el transform del AI hacia la dirección calculada
    - Actualizar rotación de la cámara para apuntar al target
    - _Requirements: 4.5_
  
  - [x] 9.4 Implementar método CanShoot()
    - Verificar que currentTarget no sea null
    - Verificar que Time.time >= nextFireTime
    - Verificar que gunScript tenga munición (bulletsInTheGun > 0)
    - Verificar que HasLineOfSight(currentTarget) sea true
    - Retornar true solo si todas las condiciones se cumplen
    - _Requirements: 4.2, 4.4, 4.6_
  
  - [x] 9.5 Implementar método ShootAtTarget()
    - Verificar CanShoot()
    - Si puede disparar, llamar a AimAtTarget(currentTarget)
    - Llamar a gunScript.AI_Shoot() (método a crear en siguiente tarea)
    - Calcular nextFireTime = Time.time + Random.Range(fireRateMin, fireRateMax)
    - _Requirements: 4.2, 4.4_
  
  - [x] 9.6 Implementar Update() para combate continuo
    - Si currentTarget es válido, llamar a ShootAtTarget()
    - Si currentTarget es null o inactivo, buscar nuevo target
    - _Requirements: 4.2_
  
  - [x] 9.7 Agregar AI_Combat_System al prefab AI_Companion
    - Agregar componente al prefab
    - Asignar referencias en Start(): gunScript, firePoint
    - Configurar enemyLayer en el Inspector
    - currentTarget se asignará dinámicamente desde AI_Target_Selection_System
    - _Requirements: 4.1_

- [x] 10. Modificar Easy FPS System para soporte de IA
  - [x] 10.1 Agregar control de IA a GunScript.cs
    - Agregar campo público: bool isAIControlled = false
    - Crear método público AI_Shoot() que llame a ShootMethod() si isAIControlled y hay munición
    - Crear método público AI_Reload() que inicie Reload_Animation si isAIControlled
    - Validar que no esté recargando antes de disparar
    - _Requirements: 2.5, 4.2_
  
  - [x] 10.2 Modificar MouseLookScript.cs para deshabilitar input en IA
    - En Awake(), solo bloquear cursor si NO tiene componente AI_Behavior_Controller
    - En MouseInputMovement(), verificar si tiene AI_Behavior_Controller
    - Si es IA, no procesar Input.GetAxis("Mouse X/Y")
    - Permitir que la rotación sea controlada externamente por AI_Combat_System
    - _Requirements: 2.5_
  
  - [x] 10.3 Modificar PlayerMovementScript.cs para deshabilitar input en IA
    - En PlayerMovementLogic(), verificar si tiene componente AI_Behavior_Controller
    - Si es IA, no procesar Input.GetAxis("Horizontal/Vertical")
    - El movimiento será controlado completamente por NavMeshAgent
    - Mantener la física de Rigidbody para colisiones
    - _Requirements: 2.5, 3.1_

- [x] 11. Integrar todos los sistemas en AI_Behavior_Controller
  - [x] 11.1 Conectar AI_Movement_System con AI_Behavior_Controller
    - Obtener referencia a AI_Movement_System en Start()
    - En estado Following, llamar a movementSystem.FollowPlayer()
    - En estado Combat, llamar a movementSystem.MoveToCombaPosition(currentTarget)
    - En estado Retreating, calcular posición alejada y usar movementSystem
    - _Requirements: 3.2, 3.3_
  
  - [x] 11.2 Conectar AI_Target_Selection_System con AI_Behavior_Controller
    - Obtener referencia a AI_Target_Selection_System en Start()
    - En UpdateAIState(), obtener currentTarget desde targetSelectionSystem.GetCurrentTarget()
    - Usar currentTarget para decidir transiciones de estado
    - Pasar currentTarget a AI_Combat_System
    - _Requirements: 4.3, 5.2_
  
  - [x] 11.3 Conectar AI_Combat_System con AI_Behavior_Controller
    - Obtener referencia a AI_Combat_System en Start()
    - En estado Combat, pasar currentTarget a combatSystem
    - En estado Retreating, permitir que combatSystem dispare mientras retrocede
    - En otros estados, deshabilitar combate
    - _Requirements: 4.2, 7.4_
  
  - [x] 11.4 Implementar lógica de estado Retreating
    - Cuando currentHealth < lowHealthThreshold, activar estado Retreating
    - Calcular posición alejada de enemigos (dirección opuesta)
    - Usar movementSystem para moverse a posición segura
    - Continuar disparando si hay línea de visión
    - _Requirements: 7.4_

- [x] 12. Checkpoint - Verificar comportamiento completo del AI
  - Ensure all tests pass, ask the user if questions arise.

- [x] 13. Implementar sistema de salud para AI_Companion
  - [x] 13.1 Agregar detección de colisiones con zombies
    - Agregar método OnCollisionEnter() en AI_Behavior_Controller
    - Detectar colisiones con objetos en zombieLayer
    - Obtener componente de daño del zombie (si existe)
    - Llamar a TakeDamage() con el valor de daño
    - _Requirements: 7.2, 7.5_
  
  - [x] 13.2 Implementar desactivación cuando salud llega a 0
    - En TakeDamage(), verificar si currentHealth <= 0
    - Desactivar todos los componentes de IA
    - Desactivar NavMeshAgent
    - Reproducir animación de muerte (si existe)
    - Desactivar GameObject después de 2 segundos
    - _Requirements: 7.3_

- [x] 14. Implementar sistema de audio para AI_Companion
  - [x] 14.1 Configurar AudioSource para sonidos de disparo
    - Agregar AudioSource al prefab AI_Companion
    - Configurar spatialBlend = 1.0 (3D sound)
    - Configurar rolloffMode = Logarithmic
    - Configurar maxDistance basado en distancia a Player_One
    - _Requirements: 9.1, 9.5_
  
  - [x] 14.2 Reproducir sonidos de disparo en AI_Combat_System
    - En ShootAtTarget(), reproducir sonido de disparo desde gunScript
    - Verificar que el sonido se reproduzca correctamente
    - Ajustar volumen basado en distancia a Player_One
    - _Requirements: 9.1_
  
  - [ ]* 14.3 Agregar sonidos de pasos durante movimiento
    - Detectar cuando navAgent.velocity.magnitude > 0.1
    - Reproducir sonido de pasos con intervalo basado en velocidad
    - Usar AudioSource separado para pasos
    - _Requirements: 9.3_
  
  - [ ]* 14.4 Agregar sonido de impacto al recibir daño
    - En TakeDamage(), reproducir sonido de impacto
    - Usar AudioSource.PlayOneShot() para no interrumpir otros sonidos
    - _Requirements: 9.4_

- [ ] 15. Optimización de rendimiento
  - [ ] 15.1 Implementar actualización por intervalos en todos los sistemas
    - Verificar que AI_Behavior_Controller use updateInterval (0.2s)
    - Verificar que AI_Target_Selection_System use scanInterval (0.3s)
    - Verificar que AI_Combat_System no actualice cada frame innecesariamente
    - _Requirements: 8.1, 8.2_
  
  - [ ] 15.2 Configurar occlusion culling para ambas cámaras
    - En Split_Screen_Manager.ConfigureCameraViewports(), habilitar useOcclusionCulling
    - Configurar farClipPlane = 100f para ambas cámaras
    - _Requirements: 8.3_
  
  - [ ] 15.3 Optimizar detección de enemigos con OverlapSphereNonAlloc
    - En AI_Target_Selection_System.ScanForEnemies(), usar buffer de Collider[20]
    - Evitar allocaciones en cada frame
    - _Requirements: 8.2_
  
  - [ ] 15.4 Implementar cálculo asíncrono de NavMesh paths
    - En AI_Movement_System, crear método RequestPath(Vector3 destination)
    - Usar Coroutine para calcular path de forma asíncrona
    - Cachear resultados de paths recientes
    - _Requirements: 8.5_

- [x] 16. Testing e integración final
  - [ ]* 16.1 Crear unit tests para AI_Target_Selection_System
    - Test: Selección del zombie más cercano con múltiples enemigos
    - Test: Priorización de zombies cerca de Player_One (< 5 unidades)
    - Test: Cambio de objetivo cuando aparece mayor amenaza
    - Test: Ignorar zombies sin línea de visión
    - _Requirements: 5.2, 5.3, 5.4_
  
  - [ ]* 16.2 Crear unit tests para AI_Behavior_Controller
    - Test: Transición Following → Combat cuando detecta enemigo
    - Test: Transición Combat → Retreating cuando salud < 30
    - Test: Transición Retreating → Following cuando salud se recupera
    - Test: Estado Idle cuando playerOne es null
    - _Requirements: 3.2, 7.4_
  
  - [ ]* 16.3 Crear integration tests para Split_Screen_Manager
    - Test: Inicialización correcta de ambos jugadores
    - Test: Configuración de viewports (izquierdo 0-0.5, derecho 0.5-1.0)
    - Test: Solo un AudioListener activo (en Player_One)
    - Test: Ambas cámaras renderizan simultáneamente
    - _Requirements: 1.2, 1.3, 6.1, 6.2, 9.2_
  
  - [ ]* 16.4 Crear integration tests para AI + Easy FPS System
    - Test: AI dispara correctamente usando GunScript
    - Test: Balas del AI causan daño a zombies
    - Test: Sonidos de disparo se reproducen
    - Test: Recarga automática cuando se agota munición
    - _Requirements: 4.2, 9.1, 10.1_
  
  - [x] 16.5 Realizar pruebas manuales de escenarios completos
    - Escenario: Inicio de juego con split-screen activo
    - Escenario: AI siguiendo al jugador sin enemigos
    - Escenario: Combate cooperativo con múltiples zombies
    - Escenario: AI con salud baja retrocediendo
    - Escenario: AI derrotado (salud = 0)
    - _Requirements: Todos los requisitos de aceptación_

- [x] 17. Checkpoint final - Verificar sistema completo
  - Ensure all tests pass, ask the user if questions arise.

## Notes

- Las tareas marcadas con `*` son opcionales y pueden omitirse para un MVP más rápido
- Cada tarea referencia requisitos específicos para trazabilidad
- Los checkpoints aseguran validación incremental del progreso
- El sistema está diseñado para integrarse sin romper funcionalidad existente
- La configuración de NavMesh (tarea 1.1) es crítica y debe completarse primero
- Las modificaciones a Easy FPS System (tarea 10) son mínimas y no invasivas
- El sistema de audio (tarea 14) puede implementarse parcialmente para MVP
- Los tests unitarios e integración (tarea 16) son opcionales pero recomendados
