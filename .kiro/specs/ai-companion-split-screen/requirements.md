# Requirements Document

## Introduction

Este documento define los requisitos para agregar un segundo jugador controlado por IA con pantalla dividida (split-screen) en el juego FPS de zombies de Unity. El jugador IA actuará como compañero del jugador principal, ayudándolo a combatir zombies en la misma escena con su propia vista de cámara independiente.

## Glossary

- **Split_Screen_Manager**: Sistema que gestiona la división de la pantalla y las cámaras de ambos jugadores
- **AI_Companion**: Segundo jugador controlado por inteligencia artificial que ayuda al jugador principal
- **Player_One**: Jugador principal controlado por el usuario mediante teclado/mouse
- **Viewport**: Área rectangular de la pantalla asignada a una cámara específica
- **Easy_FPS_System**: Sistema de disparo existente en el proyecto utilizado por ambos jugadores
- **Zombie_Target**: Enemigo zombie (Female Zombie o Monster_Orc) que puede ser atacado por ambos jugadores
- **Navigation_System**: Sistema de navegación de Unity (NavMesh) utilizado para el movimiento del AI_Companion
- **Combat_Range**: Distancia efectiva desde la cual el AI_Companion puede disparar a los zombies

## Requirements

### Requirement 1: Split-Screen Display

**User Story:** Como jugador, quiero ver dos pantallas simultáneas (izquierda y derecha), para que pueda ver mi vista y la vista del compañero IA al mismo tiempo.

#### Acceptance Criteria

1. THE Split_Screen_Manager SHALL divide la pantalla en dos viewports verticales de igual tamaño
2. THE Split_Screen_Manager SHALL asignar el viewport izquierdo (x: 0, width: 0.5) a Player_One
3. THE Split_Screen_Manager SHALL asignar el viewport derecho (x: 0.5, width: 0.5) a AI_Companion
4. WHEN la escena zombie.unity se carga, THE Split_Screen_Manager SHALL activar ambas cámaras simultáneamente
5. THE Split_Screen_Manager SHALL mantener la relación de aspecto correcta en cada viewport

### Requirement 2: AI Companion Instantiation

**User Story:** Como jugador, quiero que un segundo jugador IA aparezca en la escena, para que me acompañe durante el juego.

#### Acceptance Criteria

1. WHEN la escena zombie.unity se carga, THE Split_Screen_Manager SHALL instanciar el AI_Companion en una posición cercana a Player_One
2. THE AI_Companion SHALL utilizar el mismo prefab de jugador con el Easy_FPS_System configurado
3. THE AI_Companion SHALL tener su propia cámara independiente asignada al viewport derecho
4. THE Split_Screen_Manager SHALL posicionar al AI_Companion a una distancia mínima de 2 unidades de Player_One
5. THE AI_Companion SHALL tener todos los componentes necesarios para disparar utilizando Easy_FPS_System

### Requirement 3: AI Companion Movement

**User Story:** Como jugador, quiero que el compañero IA se mueva de forma autónoma, para que pueda ayudarme activamente en diferentes áreas del mapa.

#### Acceptance Criteria

1. THE AI_Companion SHALL utilizar el Navigation_System para moverse por el entorno
2. WHEN no hay Zombie_Target visible, THE AI_Companion SHALL seguir a Player_One manteniendo una distancia de 3-5 unidades
3. WHEN un Zombie_Target está dentro de Combat_Range, THE AI_Companion SHALL moverse hacia una posición de combate óptima
4. THE AI_Companion SHALL evitar obstáculos utilizando el Navigation_System
5. WHILE se mueve, THE AI_Companion SHALL actualizar su rotación para mirar hacia su objetivo o dirección de movimiento
6. THE AI_Companion SHALL tener una velocidad de movimiento de 3.5 unidades por segundo

### Requirement 4: AI Companion Combat Behavior

**User Story:** Como jugador, quiero que el compañero IA dispare automáticamente a los zombies, para que me ayude a eliminar enemigos.

#### Acceptance Criteria

1. THE AI_Companion SHALL detectar Zombie_Target dentro de un rango de 15 unidades
2. WHEN un Zombie_Target está dentro de Combat_Range y tiene línea de visión, THE AI_Companion SHALL disparar utilizando Easy_FPS_System
3. THE AI_Companion SHALL priorizar el Zombie_Target más cercano cuando hay múltiples enemigos
4. THE AI_Companion SHALL disparar con intervalos de 0.5-1.0 segundos entre disparos
5. WHILE dispara, THE AI_Companion SHALL mantener su mira apuntando hacia el Zombie_Target
6. IF no hay Zombie_Target dentro de Combat_Range, THEN THE AI_Companion SHALL dejar de disparar

### Requirement 5: AI Companion Target Selection

**User Story:** Como jugador, quiero que el compañero IA seleccione objetivos de forma inteligente, para que la colaboración sea efectiva.

#### Acceptance Criteria

1. THE AI_Companion SHALL escanear Zombie_Target cada 0.3 segundos
2. WHEN múltiples Zombie_Target están presentes, THE AI_Companion SHALL priorizar el más cercano a Player_One
3. WHEN un Zombie_Target está a menos de 5 unidades de Player_One, THE AI_Companion SHALL priorizar ese objetivo independientemente de la distancia
4. THE AI_Companion SHALL cambiar de objetivo si un nuevo Zombie_Target representa mayor amenaza
5. THE AI_Companion SHALL ignorar Zombie_Target que ya están siendo atacados por Player_One si hay otras amenazas disponibles

### Requirement 6: Camera Configuration

**User Story:** Como jugador, quiero que ambas cámaras funcionen correctamente, para que pueda ver claramente ambas perspectivas.

#### Acceptance Criteria

1. THE Split_Screen_Manager SHALL configurar la cámara de Player_One con viewport (x: 0, y: 0, width: 0.5, height: 1)
2. THE Split_Screen_Manager SHALL configurar la cámara de AI_Companion con viewport (x: 0.5, y: 0, width: 0.5, height: 1)
3. WHEN la escena se carga, THE Split_Screen_Manager SHALL asignar depth 0 a la cámara de Player_One
4. WHEN la escena se carga, THE Split_Screen_Manager SHALL asignar depth 1 a la cámara de AI_Companion
5. THE Split_Screen_Manager SHALL mantener el mismo field of view (17.4) para ambas cámaras
6. THE Split_Screen_Manager SHALL configurar ambas cámaras con el mismo culling mask

### Requirement 7: AI Companion Health and State

**User Story:** Como jugador, quiero que el compañero IA pueda recibir daño y ser derrotado, para que el juego mantenga el desafío.

#### Acceptance Criteria

1. THE AI_Companion SHALL tener un sistema de salud con 100 puntos iniciales
2. WHEN el AI_Companion recibe daño de un Zombie_Target, THE AI_Companion SHALL reducir su salud por la cantidad de daño recibido
3. WHEN la salud del AI_Companion llega a 0, THE AI_Companion SHALL desactivarse y dejar de funcionar
4. WHILE la salud del AI_Companion está por debajo de 30, THE AI_Companion SHALL priorizar mantener distancia de los Zombie_Target
5. THE AI_Companion SHALL utilizar el mismo sistema de colisión y daño que Player_One

### Requirement 8: Performance Optimization

**User Story:** Como jugador, quiero que el juego mantenga un rendimiento fluido con dos cámaras, para que la experiencia de juego no se vea afectada.

#### Acceptance Criteria

1. THE Split_Screen_Manager SHALL mantener un framerate mínimo de 30 FPS con ambas cámaras activas
2. THE AI_Companion SHALL actualizar su lógica de decisión cada 0.2 segundos en lugar de cada frame
3. THE Split_Screen_Manager SHALL utilizar occlusion culling para ambas cámaras
4. THE AI_Companion SHALL utilizar el mismo nivel de detalle (LOD) que Player_One para los modelos renderizados
5. THE Navigation_System SHALL calcular rutas para AI_Companion de forma asíncrona

### Requirement 9: Audio Management

**User Story:** Como jugador, quiero escuchar los sonidos de ambos jugadores correctamente, para tener una experiencia inmersiva.

#### Acceptance Criteria

1. THE AI_Companion SHALL reproducir sonidos de disparo cuando utiliza Easy_FPS_System
2. THE Split_Screen_Manager SHALL configurar Audio Listener solo en la cámara de Player_One
3. THE AI_Companion SHALL reproducir sonidos de pasos durante el movimiento
4. WHEN el AI_Companion recibe daño, THE AI_Companion SHALL reproducir sonido de impacto
5. THE Split_Screen_Manager SHALL ajustar el volumen de los sonidos del AI_Companion basado en la distancia a Player_One

### Requirement 10: Scene Integration

**User Story:** Como desarrollador, quiero que el sistema de split-screen se integre con la escena existente, para que no rompa la funcionalidad actual.

#### Acceptance Criteria

1. THE Split_Screen_Manager SHALL preservar la funcionalidad existente de Player_One
2. THE Split_Screen_Manager SHALL ser compatible con el sistema de spawn de zombies existente
3. WHEN la escena zombie.unity se carga, THE Split_Screen_Manager SHALL inicializarse antes que otros sistemas de juego
4. THE Split_Screen_Manager SHALL permitir que ambos jugadores interactúen con los mismos Zombie_Target
5. THE Split_Screen_Manager SHALL mantener la compatibilidad con el sistema de iluminación existente (Directional Light y Point Light)
