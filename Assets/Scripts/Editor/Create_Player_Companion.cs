using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

/// <summary>
/// Crea un compañero que se ve igual al jugador pero sin problemas
/// </summary>
public class Create_Player_Companion : EditorWindow
{
    [MenuItem("Tools/CREATE PLAYER COMPANION")]
    public static void ShowWindow()
    {
        GetWindow<Create_Player_Companion>("Player Companion");
    }

    private void OnGUI()
    {
        GUILayout.Label("COMPAÑERO JUGADOR", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "Esto creará un compañero que:\n\n" +
            "✓ Se ve IGUAL que tu jugador\n" +
            "✓ Te sigue a donde vayas\n" +
            "✓ NO tiene cámara (no interfiere)\n" +
            "✓ NO puede disparar ni moverse solo\n" +
            "✓ Es solo visual y te acompaña\n\n" +
            "¡Como tener un gemelo que te sigue!",
            MessageType.Info
        );

        GUILayout.Space(10);

        if (GUILayout.Button("CREAR COMPAÑERO JUGADOR", GUILayout.Height(50)))
        {
            CreatePlayerCompanion();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("ELIMINAR COMPAÑERO", GUILayout.Height(30)))
        {
            RemoveCompanion();
        }
    }

    private void CreatePlayerCompanion()
    {
        Debug.Log("=== CREANDO COMPAÑERO JUGADOR ===");

        // Eliminar compañero anterior
        RemoveCompanion();

        // Buscar jugador principal
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        if (player == null)
        {
            EditorUtility.DisplayDialog("Error", "No se encontró el jugador principal.", "OK");
            return;
        }

        // DUPLICAR EL JUGADOR COMPLETO
        GameObject companion = Instantiate(player);
        companion.name = "Player_Companion";
        companion.transform.position = player.transform.position + player.transform.right * 3f;
        companion.transform.rotation = player.transform.rotation;

        // LIMPIAR COMPONENTES PROBLEMÁTICOS

        // 1. ELIMINAR todas las cámaras del compañero
        Camera[] companionCameras = companion.GetComponentsInChildren<Camera>(true);
        foreach (Camera cam in companionCameras)
        {
            DestroyImmediate(cam.gameObject);
            Debug.Log("✓ Cámara eliminada: " + cam.name);
        }

        // 2. ELIMINAR todos los AudioListeners del compañero
        AudioListener[] audioListeners = companion.GetComponentsInChildren<AudioListener>(true);
        foreach (AudioListener audio in audioListeners)
        {
            DestroyImmediate(audio);
            Debug.Log("✓ AudioListener eliminado");
        }

        // 3. DESACTIVAR scripts de control del compañero
        MouseLookScript mouseScript = companion.GetComponent<MouseLookScript>();
        if (mouseScript != null)
        {
            mouseScript.enabled = false;
        }

        PlayerMovementScript movementScript = companion.GetComponent<PlayerMovementScript>();
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        // 4. DESACTIVAR scripts de armas del compañero
        GunScript[] gunScripts = companion.GetComponentsInChildren<GunScript>(true);
        foreach (GunScript gun in gunScripts)
        {
            gun.enabled = false;
        }

        // 5. DESACTIVAR otros scripts de gameplay
        MonoBehaviour[] allScripts = companion.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (MonoBehaviour script in allScripts)
        {
            string scriptType = script.GetType().Name;
            
            // Mantener solo componentes esenciales
            if (scriptType != "Simple_Companion" && 
                scriptType != "Transform" &&
                scriptType != "Rigidbody" &&
                !scriptType.Contains("Collider") &&
                !scriptType.Contains("Renderer") &&
                !scriptType.Contains("MeshFilter") &&
                scriptType != "NavMeshAgent")
            {
                script.enabled = false;
            }
        }

        // 6. Cambiar tag para evitar conflictos
        companion.tag = "Companion";

        // 7. AGREGAR NavMeshAgent para seguimiento
        NavMeshAgent navAgent = companion.GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            navAgent = companion.AddComponent<NavMeshAgent>();
        }
        
        // Configurar NavMeshAgent
        navAgent.speed = 3.5f;
        navAgent.stoppingDistance = 2.5f;
        navAgent.acceleration = 8f;
        navAgent.angularSpeed = 120f;
        navAgent.radius = 0.5f;
        navAgent.height = 1.8f;

        // 8. AGREGAR script de seguimiento
        Simple_Companion companionScript = companion.AddComponent<Simple_Companion>();

        // 9. Asegurar que el Rigidbody no interfiera con NavMesh
        Rigidbody companionRb = companion.GetComponent<Rigidbody>();
        if (companionRb != null)
        {
            companionRb.isKinematic = true; // Para que NavMesh controle el movimiento
        }

        // 10. Marcar escena como modificada
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );

        Debug.Log("✓ Compañero jugador creado exitosamente");

        EditorUtility.DisplayDialog("¡COMPAÑERO CREADO!", 
            "¡Compañero jugador creado exitosamente!\n\n" +
            "✓ Se ve IGUAL que tu jugador\n" +
            "✓ Te seguirá a donde vayas\n" +
            "✓ Sin cámaras ni problemas\n" +
            "✓ Tu juego original intacto\n\n" +
            "¡Presiona Play para ver a tu gemelo!", "¡Genial!");
    }

    private void RemoveCompanion()
    {
        // Buscar y eliminar compañeros
        string[] companionNames = {"Player_Companion", "Simple_Companion"};
        
        foreach (string name in companionNames)
        {
            GameObject companion = GameObject.Find(name);
            if (companion != null)
            {
                DestroyImmediate(companion);
                Debug.Log("✓ " + name + " eliminado");
            }
        }
    }
}