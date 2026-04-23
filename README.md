# 🧟 FPZ Game - FPS Zombie Survival

Un intenso juego de supervivencia FPS contra zombies desarrollado en Unity con sistema avanzado de compañero AI.

## 🎮 Descripción

FPZ Game es un juego de disparos en primera persona donde el jugador debe sobrevivir oleadas de zombies en diferentes niveles industriales y urbanos. El juego incluye un sistema de compañero AI inteligente que ayuda al jugador en combate, creando una experiencia cooperativa única.

## ✨ Características Principales

### 🔫 Sistema de Combate Avanzado
- **Armas Realistas**: Múltiples tipos de armas con física de retroceso
- **Sistema de Munición**: Gestión realista de balas y recarga
- **Combate Cuerpo a Cuerpo**: Ataques melee para situaciones de emergencia
- **Efectos Visuales**: Muzzle flash, impactos de bala y efectos de sangre

### 🤖 Inteligencia Artificial
- **Zombies Inteligentes**: AI avanzada que persigue y ataca estratégicamente
- **Compañero AI**: Sistema de seguimiento y asistencia en combate
- **Navegación NavMesh**: Movimiento fluido y realista de todos los NPCs

### 🗺️ Entornos Diversos
- **Mapas Industriales**: Complejos industriales con múltiples niveles
- **Escenarios Urbanos**: Calles y edificios para combate táctico
- **Iluminación Dinámica**: Sistema de luces que crea atmósfera inmersiva

### 🎯 Sistemas de Juego
- **Sistema de Puntuación**: Tracking detallado de kills y rendimiento
- **Múltiples Niveles**: Progresión a través de diferentes escenarios
- **Menú Principal**: Interfaz completa con opciones y configuración

## 🎮 Controles

| Acción | Control |
|--------|---------|
| **Movimiento** | WASD |
| **Mirar** | Mouse |
| **Disparar** | Click Izquierdo |
| **Apuntar** | Click Derecho |
| **Recargar** | R |
| **Ataque Melee** | Q |
| **Correr** | Shift (mantener) |
| **Agacharse** | C |
| **Saltar** | Espacio |
| **Bloquear Cursor** | L |
| **Pausa** | ESC |

## 🛠️ Requisitos del Sistema

### Mínimos
- **OS**: Windows 10 (64-bit)
- **Procesador**: Intel i3-4340 / AMD FX-6300
- **Memoria**: 4 GB RAM
- **Gráficos**: DirectX 11 compatible
- **DirectX**: Versión 11
- **Almacenamiento**: 2 GB espacio disponible

### Recomendados
- **OS**: Windows 11 (64-bit)
- **Procesador**: Intel i5-8400 / AMD Ryzen 5 2600
- **Memoria**: 8 GB RAM
- **Gráficos**: GTX 1060 / RX 580
- **DirectX**: Versión 12
- **Almacenamiento**: 4 GB espacio disponible (SSD)

## 🚀 Instalación y Ejecución

### Para Desarrolladores
```bash
# Clonar el repositorio
git clone https://github.com/MaxsilColon/FPZ-Game.git

# Abrir en Unity
# 1. Abre Unity Hub
# 2. Selecciona "Add project from disk"
# 3. Navega a la carpeta del proyecto
# 4. Abre el proyecto con Unity 2022.3+
```

### Para Jugadores
1. Descarga el build desde la carpeta `Nueva carpeta/`
2. Ejecuta `FPZ.exe`
3. ¡Disfruta del juego!

## 🏗️ Arquitectura del Proyecto

```
Assets/
├── Easy FPS/           # Sistema base de FPS
├── Scripts/            # Scripts personalizados
├── Prefabs/           # Prefabs del juego
├── Scenes/            # Escenas del juego
├── Art/               # Assets de arte y audio
├── Materials/         # Materiales y texturas
└── Settings/          # Configuraciones de Unity
```

## 🔧 Herramientas de Desarrollo

El proyecto incluye herramientas de editor personalizadas:

- **Tools → FIX ALL PLAYER PROBLEMS** - Arregla problemas del jugador
- **Tools → CREATE PLAYER COMPANION** - Crea un compañero que te sigue
- **Tools → RESET EVERYTHING** - Resetea el juego al estado original

## 🎨 Assets Utilizados

- **Easy FPS System**: Sistema base de disparos
- **Female Zombie Pack**: Modelos de zombies femeninos
- **Monster Orc**: Enemigos tipo orco/troll
- **Industrial Assets**: Entornos industriales completos
- **Audio Assets**: Efectos de sonido y música ambiente

## 🔧 Características Técnicas

- **Engine**: Unity 2022.3 LTS
- **Rendering**: Universal Render Pipeline (URP)
- **Physics**: Unity Physics 3D
- **AI Navigation**: Unity NavMesh System
- **Audio**: Unity Audio System con efectos 3D
- **Input**: Unity Input System

## 🎯 Modos de Juego

1. **Supervivencia**: Sobrevive oleadas infinitas de zombies
2. **Campaña**: Progresa a través de niveles con objetivos
3. **Boss Battle**: Enfréntate a jefes únicos y poderosos

## 🏆 Logros y Puntuación

- Sistema de puntuación basado en precisión y velocidad
- Tracking de estadísticas detalladas
- Múltiples niveles de dificultad

## 🤝 Contribuciones

Este es un proyecto educativo. Las contribuciones son bienvenidas:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📝 Licencia

Este proyecto es de código abierto para fines educativos. Los assets de terceros mantienen sus licencias originales.

## 🙏 Créditos

- **Desarrollo**: MaxsilColon
- **Assets**: Varios creadores de Unity Asset Store
- **Testing**: Comunidad de desarrolladores
- **Inspiración**: Clásicos juegos de supervivencia zombie

---

**¡Gracias por jugar FPZ Game! 🎮🧟‍♂️**