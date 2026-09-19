# Simulador Balístico — Unity 6

Prototipo de simulador balístico desarrollado como Trabajo Práctico. El jugador controla un cañón fijo, regulando ángulo, fuerza y masa del proyectil para derribar una pared de cajas conectadas mediante Joints físicos.

##  Video de demostración:

[Ver en YouTube](https://youtu.be/l899Q2N9y5E)

##  Cómo probar:

1. Ajustá los sliders de la interfaz para configurar el disparo:
   - **Ángulo vertical**: qué tan alto apunta el cañón.
   - **Ángulo horizontal**: hacia qué lado apunta.
   - **Fuerza**: intensidad del impulso de disparo.
   - **Masa**: peso del proyectil.
2. Una línea de trayectoria muestra en tiempo real la parábola estimada y dónde impactaría la bala.
3. Presioná DISPARAR para lanzar el proyectil.
4. Tras el impacto, se muestra un panel con el reporte del tiro: tiempo de vuelo, punto de impacto, velocidad relativa, impulso de colisión, piezas derribadas y puntuación.
5. Usá el botón REGENERAR CAJAS para resetear la pared de objetivos sin reiniciar el Play.

##  Controles:

| Slider Ángulo Vertical | Eleva o baja el cañón |

| Slider Ángulo Horizontal | Gira el cañón a izquierda/derecha |

| Slider Fuerza | Define la potencia del disparo |

| Slider Masa | Define el peso del proyectil |

| Botón DISPARAR | Lanza el proyectil |

| Botón REGENERAR CAJAS | Reconstruye la pared de objetivos |

## Criterios de evaluación cubiertos

- **Controles de disparo en pantalla**: ángulo (vertical y horizontal) y fuerza con Sliders; masa seleccionable con Slider.
- **Disparo físico**: proyectil con `Rigidbody` y `Collider`, lanzado con `AddForce` (`ForceMode.Impulse`) según el ángulo y la masa configurados.
- **Escena de objetivos**: pared de cajas armada con `Rigidbody` + `FixedJoint`, ancladas a una base fija (`Kinematic`), estable desde el arranque.
- **Registro del resultado**: tiempo de vuelo, punto de impacto, velocidad relativa e impulso de colisión (calculados con datos nativos de `Collision` de Unity), y piezas derribadas por tiro.
- **Reporte final**: panel en pantalla con los datos del tiro y la puntuación obtenida, tras cada disparo.

## Versión de Unity

`Unity 6000.3.19f1`
