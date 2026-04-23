using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    [Tooltip("Furthest distance bullet will look for target")]
    public float maxDistance = 1000000;

    // La variable que almacenará el resultado del Raycast
    RaycastHit hit;

    [Tooltip("Prefab of wall damange hit. The object needs 'LevelPart' tag to create decal on it.")]
    public GameObject decalHitWall;

    [Tooltip("Decal will need to be sligtly infront of the wall so it doesnt cause rendeing problems so for best feel put from 0.01-0.1.")]
    public float floatInfrontOfWall;

    [Tooltip("Blood prefab particle this bullet will create upoon hitting enemy")]
    public GameObject bloodEffect;

    [Tooltip("Put Weapon layer and Player layer to ignore bullet raycast.")]
    public LayerMask ignoreLayer;

    // **** VARIABLE DE DAÑO AÑADIDA ****
    [Tooltip("Damage this bullet inflicts (set by GunScript).")]
    [HideInInspector]
    public float damageAmount = 10f;
    // **********************************

    /*
    * Uppon bullet creation with this script attatched,
    * bullet creates a raycast which searches for corresponding tags.
    * If raycast finds somethig it will create a decal of corresponding tag.
    */
    void Update()
    {
        // El Raycast se lanza desde la posición actual de la bala, hacia adelante.
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, ~ignoreLayer))
        {
            // 1. Verificar si golpeamos un Target (con el tag "Target")
            if (hit.transform.tag == "Target")
            {
                // Intenta obtener el script Target del objeto golpeado
                Target target = hit.transform.GetComponent<Target>();

                if (target != null)
                {
                    // Llama a la función TakeDamage() del script Target, transfiriendo el daño.
                    target.TakeDamage(damageAmount);
                }

                // Crea el efecto de sangre/impacto (si tienes la variable bloodEffect asignada)
                if (bloodEffect)
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));

                // Destruye la bala después del impacto
                Destroy(gameObject);
            }

            // 2. Lógica para impactar Paredes (con el tag "LevelPart")
            else if (hit.transform.tag == "LevelPart")
            {
                // Crea el decal (agujero de bala)
                if (decalHitWall)
                {
                    Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
                }
                Destroy(gameObject);
            }

            // 3. Si golpea cualquier otra cosa, destrúyete (evita que la bala continúe infinitamente)
            else
            {
                Destroy(gameObject);
            }
        }

        // Destruye la bala después de un tiempo si no golpea nada.
        // Esto previene que la bala permanezca en el juego indefinidamente si falla el Raycast.
        Destroy(gameObject, 0.1f);
    }
}