# Mazmorras y Serpientes 🐍🏰

[![Unity](https://img.shields.io/badge/Unity-2022.3%2B-blue?logo=unity)](https://unity.com/)
[![Language](https://img.shields.io/badge/Language-C%23-green?logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

[cite_start]**Mazmorras y Serpientes** es un videojuego 2D para PC de exploración y combate con mecánicas *roguelike*[cite: 8, 94]. [cite_start]El núcleo del proyecto destaca por su sistema de generación procedural infinita, donde cada nivel escala en complejidad, enemigos y recursos conforme el jugador avanza de manera continua[cite: 6, 9].

[cite_start]Este repositorio contiene el código fuente, la arquitectura modular del sistema y los recursos utilizados en el Proyecto de Fin de Grado[cite: 1, 20].

---

## 🚀 Características Clave

- [cite_start]**Generación Procedural por Semillas:** Algoritmo que crea dinámicamente un laberinto infinito de habitaciones interconectadas de manera reproducible mediante una *seed*[cite: 13, 157].
- [cite_start]**Persistencia Segura (JSON + AES):** Sistema de guardado y carga automática local en archivos JSON estructurados con cifrado simétrico AES de 256 bits para evitar adulteraciones externas[cite: 10, 51, 53].
- [cite_start]**Combate en Tiempo Real y RPG:** Mecánicas fluidas de ataque (estocadas con lanza/barridos con espada), bloqueo, efectos alterados (veneno, inmunidad) y escalado de estadísticas tanto para el jugador como para las entidades enemigas[cite: 10, 16, 140].
- **Bestiario con IA Específica:** Tres clases de enemigos con patrones autónomos diferenciados:
  - [cite_start]**Serpiente:** Persecución activa y envenenamiento temporal[cite: 26].
  - [cite_start]**Golem de Roca:** Tanque de alta resistencia inmune al daño en fase de movimiento[cite: 41, 42].
  - [cite_start]**Espíritu:** Enemigo a distancia con patrones de teletransporte defensivo reactivo[cite: 39].
- [cite_start]**Gestión Avanzada de Inventarios:** Sistema HUD interactivo (*drag and drop*, división de pilas con modificadores como `Ctrl` / `Z`) con transferencia bidireccional entre el inventario del jugador y el cofre permanente[cite: 32, 157, 213, 214].
- [cite_start]**Economía del Juego:** Un mercader centralizado para el intercambio de monedas/gemas recolectadas por consumibles tácticos (pociones de velocidad, daño, defensa o curación)[cite: 10, 204].

---

## 🛠️ Stack Tecnológico

- [cite_start]**Motor Gráfico:** Unity [cite: 77]
- [cite_start]**Lenguaje:** C# (Programación Orientada a Objetos y patrones modulares) [cite: 76, 79]
- [cite_start]**IDE:** Visual Studio Code [cite: 83]
- [cite_start]**Control de Versiones:** Git [cite: 80]

---

## 📐 Arquitectura del Software

[cite_start]El desarrollo sigue un enfoque **modular basado en componentes**, aislando la lógica del motor de los controladores de juego[cite: 20, 79]:

- [cite_start]**`GestorPartidas.cs` (Core):** Administra el ciclo de vida del estado de juego (crear, cargar, borrar y subir nivel) sincronizando el almacenamiento de datos[cite: 128].
- [cite_start]**`MazmorraController.cs` & `HabitacionController.cs`:** Lógica matemática de construcción procedural de mapas, posicionamiento anti-solapamiento de cofres/enemigos y distribución de la llave del nivel en base a la dificultad[cite: 148, 152, 155].
- [cite_start]**`JugadorController.cs` & `EnemigoController.cs`:** Controladores de físicas 2D, máquinas de estados para IA y disparadores de eventos para la UI/HUD de vida[cite: 134, 138, 141].
- [cite_start]**`CifradoAES.cs`:** Utilidad estática criptográfica que procesa las strings del serializador `JsonUtility` mediante CBC y SHA-256[cite: 120, 121, 430].

### Modelo de Datos (JSON Estructurado)
[cite_start]El modelo lógico emula una base de datos documental (Tercera Forma Normal) encapsulando las entidades dependientes en un único documento raíz[cite: 61, 67]:
```json
{
  "partidas": [
    {
      "id": 1,
      "nombre": "Partida_01",
      "nivel": 3,
      "semilla": 4829103,
      "dificultad": "Normal",
      "inventarioJugador": [ { "iditem": 5, "cantidad": 10, "slotindex": 0 } ],
      "inventarioTaquilla": []
    }
  ]
}
