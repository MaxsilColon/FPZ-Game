# Instrucciones para Configurar NavMesh en zombie.unity

## Tarea 1.1: Configurar NavMesh en la escena zombie.unity

Esta guía te ayudará a configurar el sistema de navegación NavMesh necesario para el compañero IA.

### Requisitos
- Unity Editor abierto
- Escena `Assets/Zombie/zombie.unity` cargada

---

## Paso 1: Abrir la Escena

1. En Unity, ve a **Project** → **Assets** → **Zombie**
2. Haz doble clic en **zombie.unity** para abrir la escena

---

## Paso 2: Marcar Superficies Caminables como "Navigation Static"

### ¿Qué superficies marcar?

Debes marcar todos los objetos por los que el AI puede caminar:
- Suelos (floors)
- Plataformas
- Rampas
- Terrenos

### Cómo marcar los objetos:

1. En la ventana **Hierarchy**, selecciona el objeto del suelo principal (probablemente llamado "Ground", "Floor", "Plane" o similar)

2. En la ventana **Inspector** (panel derecho), busca la sección **Static** en la parte superior

3. Haz clic en el dropdown de **Static** y marca la opción **Navigation Static**

4. Si el objeto tiene hijos (child objects), Unity preguntará:
   - "Do you want to enable the static flags for all the child objects as well?"
   - Selecciona **Yes, change children** si todos los hijos también son superficies caminables

5. Repite este proceso para TODOS los objetos que deben ser caminables:
   - Suelos
   - Plataformas
   - Escaleras
   - Cualquier superficie horizontal donde el AI pueda moverse

### Objetos que NO deben ser Navigation Static:
- Paredes (a menos que quieras que el AI camine por ellas)
- Objetos decorativos
- Enemigos (zombies)
- Jugadores
- Objetos móviles

---

## Paso 3: Abrir la Ventana de Navigation

1. En el menú superior de Unity, ve a **Window** → **AI** → **Navigation**

2. Se abrirá la ventana **Navigation** (generalmente se acopla en el panel derecho)

---

## Paso 4: Configurar Parámetros de Baking

En la ventana **Navigation**, ve a la pestaña **Bake**:

### Configuración Requerida:

```
Agent Radius: 0.5
Agent Height: 2.0
Max Slope: 45
Step Height: 0.4 (valor por defecto, ajustar si es necesario)
```

### Cómo configurar:

1. En la pestaña **Bake**, busca la sección **Baked Agent Size**

2. Configura los siguientes valores:
   - **Agent Radius**: `0.5`
   - **Agent Height**: `2.0`

3. En la sección **Generated Off Mesh Links**, configura:
   - **Max Slope**: `45`

### Explicación de los parámetros:

- **Agent Radius (0.5)**: El radio del cilindro que representa al AI. Determina qué tan cerca puede estar de las paredes.
- **Agent Height (2.0)**: La altura del AI. Asegura que pueda pasar por debajo de objetos.
- **Max Slope (45)**: La pendiente máxima (en grados) que el AI puede subir.

---

## Paso 5: Bake el NavMesh

1. En la ventana **Navigation**, pestaña **Bake**, haz clic en el botón **Bake** en la parte inferior

2. Unity procesará la escena y generará el NavMesh (esto puede tomar unos segundos)

3. Cuando termine, verás una capa azul semi-transparente sobre las superficies caminables en la vista **Scene**

---

## Paso 6: Verificar la Cobertura del NavMesh

### Visualización:

1. En la vista **Scene**, deberías ver áreas azules que representan el NavMesh

2. Las áreas azules indican dónde el AI puede caminar

3. Si hay huecos o áreas faltantes:
   - Verifica que esos objetos estén marcados como **Navigation Static**
   - Ajusta el **Agent Radius** si es necesario (un radio más pequeño permite más cobertura)

### Verificación de Cobertura:

Asegúrate de que el NavMesh cubra:
- ✅ Todas las áreas principales de juego
- ✅ Rutas entre diferentes secciones
- ✅ Áreas donde aparecen zombies
- ✅ Spawn points del jugador y AI

### Áreas que NO deben tener NavMesh:
- ❌ Paredes
- ❌ Objetos elevados inaccesibles
- ❌ Áreas fuera del mapa de juego

---

## Paso 7: Ajustes Opcionales (si es necesario)

### Si el NavMesh no cubre suficiente área:

1. **Reducir Agent Radius**: Prueba con `0.4` o `0.3`
2. **Aumentar Max Slope**: Prueba con `50` o `60` si hay rampas pronunciadas
3. **Verificar objetos**: Asegúrate de que todos los suelos estén marcados como Navigation Static

### Si el NavMesh cubre demasiado:

1. **Aumentar Agent Radius**: Prueba con `0.6` o `0.7`
2. **Marcar obstáculos**: Objetos que deben bloquear el NavMesh pueden marcarse como **Navigation Static** con **Not Walkable**

---

## Paso 8: Guardar la Escena

1. Ve a **File** → **Save** (o presiona `Ctrl+S` / `Cmd+S`)

2. El NavMesh se guardará automáticamente con la escena

---

## Verificación Final

### Checklist:

- [ ] Escena zombie.unity abierta
- [ ] Superficies caminables marcadas como "Navigation Static"
- [ ] Ventana Navigation abierta
- [ ] Parámetros configurados:
  - [ ] Agent Radius: 0.5
  - [ ] Agent Height: 2.0
  - [ ] Max Slope: 45
- [ ] NavMesh bakeado (botón "Bake" presionado)
- [ ] Áreas azules visibles en la vista Scene
- [ ] Cobertura verificada en áreas principales de juego
- [ ] Escena guardada

---

## Solución de Problemas

### Problema: No veo áreas azules después de bakear

**Solución:**
- Verifica que al menos un objeto esté marcado como "Navigation Static"
- Asegúrate de estar en la vista **Scene** (no **Game**)
- En la ventana Navigation, ve a la pestaña **Object** y verifica que los objetos aparezcan en la lista

### Problema: El NavMesh tiene huecos

**Solución:**
- Marca más objetos como "Navigation Static"
- Reduce el **Agent Radius** a `0.4` o `0.3`
- Verifica que no haya gaps entre los objetos del suelo

### Problema: El NavMesh cubre áreas donde no debería

**Solución:**
- Selecciona los objetos problemáticos
- En Inspector → Static, desmarca "Navigation Static"
- O marca como "Not Walkable" en la ventana Navigation → Object

---

## Próximos Pasos

Una vez completada esta configuración, el NavMesh estará listo para ser utilizado por el AI_Companion en las siguientes tareas del proyecto.

**Siguiente tarea:** 1.2 - Crear estructura de carpetas para scripts del sistema

---

## Referencias

- **Requisitos relacionados:** 3.1, 3.4, 8.5
- **Documentación de Unity:** [NavMesh Building Components](https://docs.unity3d.com/Manual/nav-BuildingNavMesh.html)
