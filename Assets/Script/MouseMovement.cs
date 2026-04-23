using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    // REFERENCIA: ¡Arrastra la Main Camera a esta ranura en el Inspector!
    public Transform playerCamera;

    // Ajustes públicos
    public float mouseSensitivity = 100f;

    // Variables internas para la rotación vertical
    private float rotationX = 0f;

    void Start()
    {
        // Oculta y bloquea el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. Obtener la entrada del ratón
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 2. Rotación Vertical (Arriba/Abajo)
        // Acumula y limita la rotación para evitar que el jugador mire detrás de sí mismo (360º)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Aplica la rotación vertical a la CÁMARA (Eje X)
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // 3. Rotación Horizontal (Izquierda/Derecha)
        // Aplica la rotación horizontal al CUERPO del jugador (Eje Y)
        transform.Rotate(Vector3.up * mouseX);
    }
}