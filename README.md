# Mazmorras y Serpientes 🐍🏰

**Mazmorras y Serpientes** es un videojuego 2D para PC de exploración y combate con mecánicas *roguelike*. El núcleo del proyecto destaca por su sistema de generación procedural infinita, donde cada nivel escala en complejidad, enemigos y recursos conforme el jugador avanza de manera continua.

Este repositorio contiene el código fuente, la arquitectura modular del sistema y los recursos utilizados en el Proyecto de Fin de Grado.

---

## 🚀 Características Clave

- **Generación Procedural por Semillas:** Algoritmo que crea dinámicamente un laberinto infinito de habitaciones interconectadas de manera reproducible mediante una *seed*.
- **Persistencia Segura (JSON + AES):** Sistema de guardado y carga automática local en archivos JSON estructurados con cifrado simétrico AES de 256 bits para evitar adulteraciones externas.
- **Combate en Tiempo Real y RPG:** Mecánicas fluidas de ataque (estocadas con lanza/barridos con espada), bloqueo, efectos alterados (veneno, inmunidad) y escalado de estadísticas tanto para el jugador como para las entidades enemigas.
- **Bestiario con IA Específica:** Tres clases de enemigos con patrones autónomos diferenciados:
  - **Serpiente:** Persecución activa y envenenamiento temporal.
  - **Golem de Roca:** Tanque de alta resistencia inmune al daño en fase de movimiento.
  - **Espíritu:** Enemigo a distancia con patrones de teletransporte defensivo reactivo.
- **Gestión Avanzada de Inventarios:** Sistema HUD interactivo (*drag and drop*, división de pilas con modificadores como `Ctrl` / `Z`) con transferencia bidireccional entre el inventario del jugador y el cofre permanente.
- **Economía del Juego:** Un mercader centralizado para el intercambio de monedas/gemas recolectadas por consumibles tácticos (pociones de velocidad, daño, defensa o curación).

---

## 🛠️ Stack Tecnológico

- **Motor Gráfico:** Unity
- **Lenguaje:** C# (Programación Orientada a Objetos y patrones modulares)
- **IDE:** Visual Studio Code
- **Control de Versiones:** Git

---

## 📐 Arquitectura del Software

El desarrollo sigue un enfoque **modular basado en componentes**, aislando la lógica del motor de los controladores de juego:

- **`GestorPartidas.cs` (Core):** Administra el ciclo de vida del estado de juego (crear, cargar, borrar y subir nivel) sincronizando el almacenamiento de datos.
- **`MazmorraController.cs` & `HabitacionController.cs`:** Lógica matemática de construcción procedural de mapas, posicionamiento anti-solapamiento de cofres/enemigos y distribución de la llave del nivel en base a la dificultad.
- **`JugadorController.cs` & `EnemigoController.cs`:** Controladores de físicas 2D, máquinas de estados para IA y disparadores de eventos para la UI/HUD de vida.
- **`CifradoAES.cs`:** Utilidad estática criptográfica que procesa las strings del serializador `JsonUtility` mediante CBC y SHA-256.

### Modelo de Datos (JSON Estructurado)

El modelo lógico emula una base de datos documental (Tercera Forma Normal) encapsulando las entidades dependientes en un único documento raíz. Ejemplo de estructura:

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

---

## 📖 Más Información

Para conocer todos los detalles técnicos, metodologías de desarrollo, diagramas de flujo y el manual de usuario completo, puedes consultar el archivo [DOCUMENTO_X.pdf](DOCUMENTO_X.pdf) ubicado en la raíz de este repositorio.
