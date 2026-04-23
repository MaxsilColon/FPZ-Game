using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Ajustes de Disparo")]
    public float damage = 10f;          // Daño infligido (se pasa al script Bullet)
    public float fireRate = 1f;         // Tasa de disparo (disparos por segundo)
    private float nextTimeToFire = 0f;  // Control de cooldown

    [Header("Proyectil Físico")]
    // ¡Arrastra el Bullet_Prefab a esta ranura en el Inspector!
    public GameObject bulletPrefab;
    public float launchForce = 50f;     // Fuerza con la que se lanza la bala

    [Header("Referencias")]
    // ¡Arrastra tu Main Camera aquí en el Inspector!
    // Se usa para obtener la posición y dirección de disparo.
    public Camera fpsCam;

    void Update()
    {
        // Verificar si se presiona el botón de disparo (ratón izquierdo) 
        // y si ha pasado suficiente tiempo desde el último disparo
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            // Resetear el cooldown
            nextTimeToFire = Time.time + 1f / fireRate;

            // Llamar a la función de disparo de proyectil
            Shoot();
        }
    }

    void Shoot()
    {
        // --- LÓGICA DE INSTANCIACIÓN DE BALA FÍSICA ---

        // 1. Crear una instancia de la bala (Instantiate)
        // Se crea en la posición y rotación de la cámara (de donde apunta)
        GameObject bulletInstance = Instantiate(bulletPrefab, fpsCam.transform.position, fpsCam.transform.rotation);

        // 2. Obtener el Rigidbody y aplicarle la fuerza para mover la bala
        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Lanza la bala hacia adelante con respecto a la vista de la cámara
            rb.AddForce(fpsCam.transform.forward * launchForce, ForceMode.Impulse);
        }

        // 3. Transferir el daño del arma al script Bullet
        // Esto asegura que si cambias el daño en el arma, la bala se actualice
        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = damage;
        }

        // --- EFECTOS VISUALES Y SONIDO (Añadir aquí) ---
        // Ejemplo: Toca un sonido de disparo o activa un Particle System de flash
    }
}