# Instrucciones: Agregar Split_Screen_Manager a zombie.unity

## Tarea 2.5: Configuración del Split Screen Manager en la Escena

Este documento proporciona instrucciones paso a paso para agregar y configurar el componente `Split_Screen_Manager` en la escena `zombie.unity`.

---

## Requisitos Previos

Antes de comenzar, asegúrate de que:
- ✅ El script `Split_Screen_Manager.cs` existe en `Assets/Scripts/SplitScreen/Managers/`
- ✅ Los prefabs de jugador están creados (Player_One y AI_Companion)
- ✅ La escena `zombie.unity` está abierta en Unity Editor

---

## Pasos de Configuración

### Paso 1: Abrir la Escena zombie.unity

1. En Unity Editor, navega a `Assets/Zombie/`
2. Haz doble clic en `zombie.unity` para abrirla
3. Verifica que la escena se cargó correctamente en la vista Scene

### Paso 2: Crear el GameObject SplitScreenManager

1. En la ventana **Hierarchy**, haz clic derecho en un área vacía
2. Selecciona **Create Empty**
3. Renombra el GameObject a: `SplitScreenManager`
4. Asegúrate de que la posición del Transform sea (0, 0, 0)

### Paso 3: Agregar el Componente Split_Screen_Manager

1. Con el GameObject `SplitScreenManager` seleccionado en la Hierarchy
2. En la ventana **Inspector**, haz clic en **Add Component**
3. Busca: `Split_Screen_Manager`
4. Selecciona el script para agregarlo al GameObject

### Paso 4: Configurar Referencias de Prefabs

En el Inspector, verás las siguientes secciones que necesitas configurar:

#### **Player References**

1. **Player One Prefab:**
   - Arrastra el prefab `Player` desde `Assets/Prefabs/Player.prefab`
   - O haz clic en el círculo y selecciona el prefab desde el selector

2. **AI Companion Prefab:**
   - **IMPORTANTE:** El prefab `AI_Companion` se creará en la tarea 4.1
   - Por ahora, **deja este campo vacío** (None)
   - Volverás a asignar este campo después de completar la tarea 4.1

3. **Player One Spawn Point:**
   - En la Hierarchy, busca o crea un GameObject vacío llamado `PlayerSpawnPoint`
   - Posiciónalo en las coordenadas deseadas (ejemplo: X=0, Y=1, Z=0)
   - Arrastra este GameObject al campo `Player One Spawn Point`

4. **AI Companion Spawn Point:**
   - En la Hierarchy, busca o crea un GameObject vacío llamado `AISpawnPoint`
   - Posiciónalo cerca del PlayerSpawnPoint (ejemplo: X=2, Y=1, Z=0)
   - Arrastra este GameObject al campo `AI Companion Spawn Point`

#### **Camera Configuration**

1. **Field Of View:** Dejar en `17.4` (valor por defecto)
2. **Culling Mask:** Dejar en `Everything` (valor por defecto: -1)

#### **Spawn Settings**

1. **Min Spawn Distance:** Dejar en `2` (distancia mínima entre jugadores)
2. **AI Spawn Offset:** Dejar en `(2, 0, 0)` (offset adicional para el AI)

### Paso 5: Modificar el Script Split_Screen_Manager (Agregar Start)

El componente necesita llamar a `InitializeSplitScreen()` en el método `Start()`. Abre el archivo:

`Assets/Scripts/SplitScreen/Managers/Split_Screen_Manager.cs`

Y agrega el siguiente método después de las variables privadas:

```csharp
void Start()
{
    InitializeSplitScreen();
}
```

El método Start completo debería verse así:

```csharp
/// <summary>
/// Inicializa el sistema al cargar la escena.
/// </summary>
void Start()
{
    InitializeSplitScreen();
}
```

### Paso 6: Crear Spawn Points (Si no existen)

Si los spawn points no existen en la escena:

1. **Crear PlayerSpawnPoint:**
   - Hierarchy → Clic derecho → Create Empty
   - Renombrar a: `PlayerSpawnPoint`
   - Transform Position: `(0, 1, 0)` (ajustar según tu escena)
   - Transform Rotation: `(0, 0, 0)`

2. **Crear AISpawnPoint:**
   - Hierarchy → Clic derecho → Create Empty
   - Renombrar a: `AISpawnPoint`
   - Transform Position: `(2, 1, 0)` (ajustar según tu escena)
   - Transform Rotation: `(0, 0, 0)`

**IMPORTANTE:** Asegúrate de que ambos spawn points estén sobre el NavMesh si ya está configurado.

### Paso 7: Verificar la Configuración

Antes de guardar, verifica que:

- ✅ El GameObject `SplitScreenManager` existe en la Hierarchy
- ✅ El componente `Split_Screen_Manager` está agregado
- ✅ El campo `Player One Prefab` está asignado
- ⚠️ El campo `AI Companion Prefab` puede estar vacío (se asignará en tarea 4.1)
- ✅ Los campos `Player One Spawn Point` y `AI Companion Spawn Point` están asignados
- ✅ Los spawn points tienen posiciones válidas en la escena
- ✅ El método `Start()` fue agregado al script

### Paso 8: Guardar la Escena

1. En Unity Editor, presiona **Ctrl+S** (Windows) o **Cmd+S** (Mac)
2. O ve a **File → Save** en el menú superior
3. Verifica que el archivo `zombie.unity` se guardó correctamente

---

## Validación

**NOTA IMPORTANTE:** Como el prefab AI_Companion aún no existe (se creará en tarea 4.1), verás un error en la consola al presionar Play. Esto es esperado y normal en este punto.

Para verificar que la configuración básica está correcta:

1. **Presiona Play** en Unity Editor
2. Observa la **Console** (Window → General → Console)
3. **Deberías ver un error esperado:**
   ```
   [Split_Screen_Manager] aiCompanionPrefab es null. No se puede inicializar el sistema.
   ```
   Esto es normal y se resolverá cuando completes la tarea 4.1.

4. **Validación completa (después de tarea 4.1):**
   Una vez que asignes el prefab AI_Companion, deberías ver:
   ```
   [Split_Screen_Manager] Player_One instanciado en (x, y, z)
   [Split_Screen_Manager] AI_Companion instanciado en (x, y, z)
   [Split_Screen_Manager] Cámara de Player_One configurada: viewport izquierdo (0, 0, 0.5, 1), depth 0
   [Split_Screen_Manager] Cámara de AI_Companion configurada: viewport derecho (0.5, 0, 0.5, 1), depth 1
   [Split_Screen_Manager] Sistema de split-screen inicializado correctamente.
   ```

5. **Verifica visualmente (después de tarea 4.1):**
   - La pantalla debe estar dividida verticalmente en dos
   - El lado izquierdo muestra la vista de Player_One
   - El lado derecho muestra la vista de AI_Companion

---

## Solución de Problemas

### Error: "playerOnePrefab es null"
- **Causa:** El prefab Player no está asignado
- **Solución:** Asigna el prefab `Player.prefab` en el campo `Player One Prefab`

### Error: "aiCompanionPrefab es null"
- **Causa:** El prefab AI_Companion no está asignado
- **Solución:** **Esto es esperado en este punto.** El prefab AI_Companion se creará en la tarea 4.1. Ignora este error por ahora y continúa con las siguientes tareas. Volverás a asignar el prefab cuando esté disponible.

### Error: "playerOneSpawnPoint es null"
- **Causa:** El spawn point no está asignado
- **Solución:** Crea y asigna los GameObjects de spawn points según el Paso 6

### Error: "No se encontró cámara en Player_One"
- **Causa:** El prefab Player no tiene una cámara como hijo
- **Solución:** Verifica que el prefab Player tenga un GameObject hijo con componente Camera

### La pantalla no se divide
- **Causa:** El método `Start()` no se está ejecutando
- **Solución:** Verifica que agregaste el método `Start()` al script según el Paso 5

### Los jugadores no aparecen
- **Causa:** Los spawn points están en posiciones inválidas
- **Solución:** Ajusta las posiciones de los spawn points a áreas accesibles de la escena

---

## Notas Adicionales

- **NavMesh:** Si ya configuraste el NavMesh en la escena, asegúrate de que los spawn points estén sobre áreas navegables
- **Prefab AI_Companion:** Se creará en la tarea 4.1. Esta tarea configura la infraestructura básica
- **Testing parcial:** Puedes verificar que el componente está correctamente agregado, pero la funcionalidad completa requiere el prefab AI_Companion
- **Próximos pasos:** Después de completar la tarea 4.1 (crear prefab AI_Companion), regresa al Inspector del SplitScreenManager y asigna el nuevo prefab
- **Script de validación (opcional):** Existe un script `Split_Screen_Manager_Validator.cs` que puedes agregar a la escena para validar automáticamente la configuración. Para usarlo:
  1. Crea un GameObject vacío llamado "Validator"
  2. Agrega el componente `Split_Screen_Manager_Validator`
  3. Presiona Play y revisa la consola para ver un reporte detallado de la configuración

---

## Referencias

- **Requisitos:** 1.4 (Split-Screen Display), 10.3 (Scene Integration)
- **Design Document:** `.kiro/specs/ai-companion-split-screen/design.md`
- **Script:** `Assets/Scripts/SplitScreen/Managers/Split_Screen_Manager.cs`

---

**Fecha de creación:** 2024
**Tarea:** 2.5 - Agregar Split_Screen_Manager a zombie.unity
