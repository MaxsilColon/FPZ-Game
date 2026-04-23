using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Componentes y referencias
    private CharacterController controller;

    // Variables de movimiento
    public float speed = 10f; // Velocidad de movimiento horizontal

    // Variables de gravedad y salto
    public float gravity = -19.62f; // Valor de gravedad (cercano a la Tierra)
    public float jumpHeight = 3f; // Altura máxima de salto

    private Vector3 moveDirection; // Almacena el vector de movimiento TOTAL (horizontal + vertical)

    // Variables de Ground Check
    public Transform groundCheck; // Objeto vacío posicionado en los pies del jugador
    public float groundDistance = 0.4f; // Radio para el Physics.CheckSphere
    public LayerMask groundMask; // Capa que define lo que es 'suelo'

    public bool isGrounded; // Indica si el jugador está tocando el suelo

    // Variables para verificar movimiento
    private Vector3 lastPosition;
    public bool isMoving;
    public bool isStanding;


    void Start()
    {
        // Obtener la referencia al CharacterController
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    void Update()
    {
        // 1. COMPROBACIÓN DE SUELO (Ground Check)
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // **ERROR CORREGIDO AQUI: La velocidad se reinicia si está en el suelo.**
        if (isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = -2f; // Reseteamos la velocidad vertical
        }

        // 2. MOVIMIENTO HORIZONTAL
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculamos el vector de movimiento horizontal (transform.right y transform.forward usan la dirección del jugador)
        Vector3 horizontalMove = transform.right * x + transform.forward * z;

        // Asignamos la velocidad horizontal (mantenemos la velocidad vertical)
        moveDirection.x = horizontalMove.x * speed;
        moveDirection.z = horizontalMove.z * speed;


        // 3. SALTO
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Aplicamos la fórmula para calcular la velocidad de salto
            moveDirection.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 4. APLICAR GRAVEDAD
        // La gravedad se aplica sobre la componente Y (velocidad vertical)
        moveDirection.y += gravity * Time.deltaTime;

        // 5. APLICAR EL MOVIMIENTO TOTAL
        // **ERROR CORREGIDO: Aplicamos el movimiento horizontal y vertical en una sola llamada.**
        controller.Move(moveDirection * Time.deltaTime);

        // 6. COMPROBAR ESTADO DE MOVIMIENTO
        // Comprobamos si hay movimiento horizontal significativo
        Vector3 flatVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        isMoving = flatVelocity.magnitude > 0.1f;
        isStanding = !isMoving && isGrounded;
    }
}