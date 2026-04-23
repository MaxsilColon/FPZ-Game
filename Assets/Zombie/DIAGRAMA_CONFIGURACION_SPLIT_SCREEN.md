# Diagrama de Configuración: Split Screen Manager

## Estructura de la Escena zombie.unity

```
zombie.unity
│
├── Directional Light
├── Main Camera (se deshabilitará automáticamente)
├── Player (existente, se puede eliminar si está en la escena)
│
├── SplitScreenManager (NUEVO - Tarea 2.5)
│   └── Componente: Split_Screen_Manager
│       ├── Player One Prefab: → Assets/Prefabs/Player.prefab
│       ├── AI Companion Prefab: → (vacío por ahora, tarea 4.1)
│       ├── Player One Spawn Point: → PlayerSpawnPoint
│       ├── AI Companion Spawn Point: → AISpawnPoint
│       ├── Field Of View: 17.4
│       ├── Culling Mask: Everything
│       ├── Min Spawn Distance: 2
│       └── AI Spawn Offset: (2, 0, 0)
│
├── PlayerSpawnPoint (NUEVO - Tarea 2.5)
│   └── Transform: Position (0, 1, 0)
│
├── AISpawnPoint (NUEVO - Tarea 2.5)
│   └── Transform: Position (2, 1, 0)
│
├── Zombies (existentes)
├── Environment (existente)
└── NavMesh (configurado en tarea 1.1)
```

---

## Flujo de Inicialización

```
1. Escena zombie.unity se carga
   ↓
2. SplitScreenManager.Start() se ejecuta
   ↓
3. InitializeSplitScreen() es llamado
   ↓
4. Validación de prefabs
   ├── ✅ playerOnePrefab existe → Continuar
   └── ❌ aiCompanionPrefab es null → ERROR (esperado en tarea 2.5)
   ↓
5. SpawnPlayers() (solo cuando ambos prefabs existen)
   ├── Instanciar Player_One en PlayerSpawnPoint
   └── Instanciar AI_Companion en AISpawnPoint
   ↓
6. ConfigureCameraViewports()
   ├── Player_One Camera → Viewport izquierdo (0, 0, 0.5, 1)
   └── AI_Companion Camera → Viewport derecho (0.5, 0, 0.5, 1)
   ↓
7. Sistema inicializado ✅
```

---

## Configuración Visual del Inspector

```
┌─────────────────────────────────────────────────────────┐
│ SplitScreenManager (GameObject)                         │
├─────────────────────────────────────────────────────────┤
│ Transform                                               │
│   Position: (0, 0, 0)                                   │
│   Rotation: (0, 0, 0)                                   │
│   Scale: (1, 1, 1)                                      │
├─────────────────────────────────────────────────────────┤
│ Split_Screen_Manager (Script)                           │
│                                                         │
│ ▼ Player References                                     │
│   Player One Prefab:        [Player]                    │
│   AI Companion Prefab:      [None] ⚠️                   │
│   Player One Spawn Point:   [PlayerSpawnPoint]          │
│   AI Companion Spawn Point: [AISpawnPoint]              │
│                                                         │
│ ▼ Camera Configuration                                  │
│   Field Of View:  17.4                                  │
│   Culling Mask:   Everything                            │
│                                                         │
│ ▼ Spawn Settings                                        │
│   Min Spawn Distance: 2                                 │
│   AI Spawn Offset:    X: 2  Y: 0  Z: 0                  │
└─────────────────────────────────────────────────────────┘
```

---

## Layout de Pantalla Dividida

```
┌─────────────────────────────────────────────────────────┐
│                    PANTALLA COMPLETA                    │
├──────────────────────────┬──────────────────────────────┤
│                          │                              │
│    VIEWPORT IZQUIERDO    │    VIEWPORT DERECHO          │
│                          │                              │
│    Player_One Camera     │    AI_Companion Camera       │
│                          │                              │
│    Rect: (0, 0, 0.5, 1)  │    Rect: (0.5, 0, 0.5, 1)    │
│    Depth: 0              │    Depth: 1                  │
│    FOV: 17.4             │    FOV: 17.4                 │
│    Audio Listener: ✅     │    Audio Listener: ❌         │
│                          │                              │
│    [Vista del jugador]   │    [Vista del AI]            │
│                          │                              │
└──────────────────────────┴──────────────────────────────┘
```

---

## Posicionamiento de Spawn Points

Vista superior de la escena:

```
                    ↑ Z
                    │
                    │
        ┌───────────┼───────────┐
        │           │           │
        │           │           │
        │     P1    │    AI     │
        │     ●─────┼────●      │
        │   (0,1,0) │  (2,1,0)  │
        │           │           │
        │           │           │
        └───────────┼───────────┘
                    │
    ────────────────┼──────────────→ X
                    │
                    │

P1 = PlayerSpawnPoint
AI = AISpawnPoint
Distancia mínima: 2 unidades
```

---

## Estados de Configuración

### Estado Actual (Tarea 2.5)

```
✅ Script Split_Screen_Manager.cs creado
✅ Método Start() agregado
✅ GameObject SplitScreenManager en escena
✅ Componente Split_Screen_Manager agregado
✅ Player One Prefab asignado
⚠️ AI Companion Prefab vacío (esperado)
✅ Spawn Points creados y asignados
```

### Estado Completo (Después de Tarea 4.1)

```
✅ Script Split_Screen_Manager.cs creado
✅ Método Start() agregado
✅ GameObject SplitScreenManager en escena
✅ Componente Split_Screen_Manager agregado
✅ Player One Prefab asignado
✅ AI Companion Prefab asignado
✅ Spawn Points creados y asignados
✅ Sistema funcional con split-screen activo
```

---

## Checklist de Verificación

### Antes de Guardar la Escena

- [ ] GameObject `SplitScreenManager` existe en Hierarchy
- [ ] Componente `Split_Screen_Manager` está agregado
- [ ] `Player One Prefab` apunta a `Assets/Prefabs/Player.prefab`
- [ ] `AI Companion Prefab` está vacío (None) - OK por ahora
- [ ] GameObject `PlayerSpawnPoint` existe en posición (0, 1, 0)
- [ ] GameObject `AISpawnPoint` existe en posición (2, 1, 0)
- [ ] Ambos spawn points están asignados en el componente
- [ ] Escena guardada (Ctrl+S / Cmd+S)

### Después de Presionar Play

- [ ] Console muestra error: "aiCompanionPrefab es null" - Esperado ✅
- [ ] No hay otros errores críticos
- [ ] El GameObject SplitScreenManager está activo

### Después de Completar Tarea 4.1

- [ ] `AI Companion Prefab` asignado a `Assets/Prefabs/AI_Companion.prefab`
- [ ] Console muestra: "Sistema de split-screen inicializado correctamente"
- [ ] Pantalla dividida verticalmente visible
- [ ] Player_One aparece en lado izquierdo
- [ ] AI_Companion aparece en lado derecho
- [ ] Ambas cámaras funcionan correctamente

---

## Referencias Rápidas

| Elemento | Ubicación | Valor |
|----------|-----------|-------|
| Script Manager | `Assets/Scripts/SplitScreen/Managers/Split_Screen_Manager.cs` | - |
| Prefab Player | `Assets/Prefabs/Player.prefab` | Asignado |
| Prefab AI | `Assets/Prefabs/AI_Companion.prefab` | Pendiente (tarea 4.1) |
| Escena | `Assets/Zombie/zombie.unity` | Modificar |
| Instrucciones | `Assets/Zombie/INSTRUCCIONES_SPLIT_SCREEN_MANAGER.md` | Leer |
| Resumen | `Assets/Zombie/RESUMEN_TAREA_2.5.md` | Leer |

---

**Última actualización:** Tarea 2.5
**Próxima tarea:** 3. Checkpoint - Verificar configuración de split-screen
