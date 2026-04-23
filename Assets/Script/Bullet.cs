using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Variables de la bala (se definen en WeaponController)
    public float damage = 10f;

    // Unity llama a esta función cuando la bala colisiona con otro objeto
    void OnCollisionEnter(Collision collision)
    {
        // 1. Verificar si golpeamos un objetivo (Target)
        Target target = collision.gameObject.GetComponent<Target>();

        // Si encontramos el script Target, le infligimos daño.
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        // 2. Destruir la bala después de golpear cualquier cosa (pared, objetivo, etc.)
        Destroy(gameObject);
    }

    // Opcional: Destruir la bala después de un tiempo si no golpea nada
    void Start()
    {
        // Destruir la bala después de 5 segundos para que no vuele infinitamente
        Destroy(gameObject, 5f);
    }
}