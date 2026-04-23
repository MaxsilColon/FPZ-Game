using UnityEngine;

public class MoverDiana : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [Tooltip("La distancia máxima que la diana se moverá desde su posición inicial (en el Eje Z).")]
    public float moveDistance = 5.0f;

    [Tooltip("La velocidad a la que la diana se mueve.")]
    public float speed = 2.0f;

    private Vector3 startPosition;

    void Start()
    {
        // Guardamos la posición inicial de la diana al inicio del juego.
        startPosition = transform.position;
    }

    void Update()
    {
        // 1. Calcular el desplazamiento usando Mathf.PingPong
        // Mathf.PingPong crea un valor que oscila suavemente entre 0 y el valor límite (moveDistance).
        // Time.time * speed hace que el valor de entrada cambie con el tiempo.
        float offsetZ = Mathf.PingPong(Time.time * speed, moveDistance);

        // 2. Aplicar la nueva posición

        // Creamos la nueva posición:
        // La posición X e Y se mantienen fijas.
        // La posición Z es la inicial (startPosition.z) más el desplazamiento calculado (offsetZ).
        Vector3 newPosition = new Vector3(
            startPosition.x + offsetZ,
            startPosition.y ,
            startPosition.z 
        );

        // Aplicamos la nueva posición al objeto.
        transform.position = newPosition;
    }
}