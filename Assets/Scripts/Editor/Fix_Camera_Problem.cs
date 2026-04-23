using UnityEngine;
using UnityEditor;

/// <summary>
/// Arregla el problema de la cámara desaparecida
/// </summary>
public class Fix_Camera_Problem : EditorWindow
{
    [MenuItem("Tools/FIX CAMERA PROBLEM")]
    public static void ShowWindow()
    {
        GetWindow<Fix_Camera_Problem>("Fix Camera");
    }

    private void OnGUI()
    {
        GUILayout.Label("ARREGLAR CÁMARA", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "Si tu cámara desapareció después de crear el compañero:\n\n" +
            "✓ Esto reactivará tu cámara principal\n" +
            "✓ Eliminará el compañero problemático\n" +
            "✓ Restaurará el juego original\n\n" +
            "Presiona el botón para arreglar.",
            MessageType.Warning
        );

        GUILayout.Space(10);

        if (GUILayout.Button("ARREGLAR CÁMARA AHORA", GUILayout.Height(50)))
        {
            FixCameraProblem();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("CREAR COMPAÑERO CORRECTO", GUILayout.Height(40)))
        {
            CreateCorrectCompanion();
        }
    }

    private void FixCameraProblem()
    {
        Debug.Log("=== ARREGLANDO PROBLEMA DE CÁMARA ===");

        // 1. Eliminar compañero problemático
        GameObject companion = GameObject.Find("Simple_Companion");
        if (companion != null)
        {
            DestroyImmediate(companion);
            Debug.Log("✓ Compañero problemático eliminado");
        }

        // 2. Buscar jugador principal
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        if (player != null)
        {
            // 3. Asegurar que el jugador esté activo
            player.SetActive(true);

            // 4. Buscar y reactivar la cámara principal
            Camera playerCamera = player.GetComponentInChildren<Camera>(true); // incluir inactivos
            if (playerCamera != null)
            {
                playerCamera.gameObject.SetActive(true);
                playerCamera.enabled = true;
                
                // Asegurar que tenga el tag correcto
                if (playerCamera.gameObject.tag != "MainCamera")
                {
                    playerCamera.gameObject.tag = "MainCamera";
                }
                
                Debug.Log("✓ Cámara principal reactivada: " + playerCamera.name);
            }

            // 5. Asegurar AudioListener
            AudioListener playerAudio = player.GetComponentInChildren<AudioListener>(true);
            if (playerAudio != null)
            {
                playerAudio.enabled = true;
                Debug.Log("✓ AudioListener reactivado");
            }

            // 6. Actualizar MouseLookScript
            MouseLookScript mls = player.GetComponent<MouseLookScript>();
            if (mls != null && playerCamera != null)
            {
                mls.myCamera = playerCamera.transform;
                Debug.Log("✓ MouseLookScript actualizado");
            }
        }

        // 7. Marcar escena como modificada
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );

        Debug.Log("=== PROBLEMA DE CÁMARA ARREGLADO ===");

        EditorUtility.DisplayDialog("¡ARREGLADO!", 
            "Problema de cámara solucionado.\n\n" +
            "✓ Cámara principal reactivada\n" +
            "✓ Compañero problemático eliminado\n" +
            "✓ Juego restaurado\n\n" +
            "Presiona Play para probar.", "OK");
    }

    private void CreateCorrectCompanion()
    {
        Debug.Log("=== CREANDO COMPAÑERO CORRECTO ===");

        // Eliminar compañero anterior
        GameObject oldCompanion = GameObject.Find("Simple_Companion");
        if (oldCompanion != null)
        {
            DestroyImmediate(oldCompanion);
        }

        // Buscar jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        if (player == null)
        {
            EditorUtility.DisplayDialog("Error", "No se encontró el jugador.", "OK");
            return;
        }

        // Crear compañero simple SIN usar prefab (para evitar problemas)
        GameObject companion = new GameObject("Simple_Companion");
        companion.transform.position = player.transform.position + player.transform.right * 3f;
        companion.transform.rotation = player.transform.rotation;

        // Agregar modelo visual simple (cubo)
        GameObject visualCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        visualCube.transform.parent = companion.transform;
        visualCube.transform.localPosition = Vector3.zero;
        visualCube.transform.localScale = new Vector3(0.8f, 1.8f, 0.8f);
        
        // Cambiar color para distinguirlo
        Renderer cubeRenderer = visualCube.GetComponent<Renderer>();
        if (cubeRenderer != null)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = Color.green;
            cubeRenderer.material = mat;
        }

        // Agregar NavMeshAgent
        UnityEngine.AI.NavMeshAgent navAgent = companion.AddComponent<UnityEngine.AI.NavMeshAgent>();
        navAgent.speed = 3.5f;
        navAgent.stoppingDistance = 3f;
        navAgent.height = 1.8f;
        navAgent.radius = 0.4f;

        // Agregar script de seguimiento
        Simple_Companion companionScript = companion.AddComponent<Simple_Companion>();

        Debug.Log("✓ Compañero correcto creado (cubo verde)");

        EditorUtility.DisplayDialog("¡COMPAÑERO CREADO!", 
            "Compañero simple creado correctamente.\n\n" +
            "✓ Cubo verde que te sigue\n" +
            "✓ Sin cámaras, sin problemas\n" +
            "✓ Tu cámara principal intacta\n\n" +
            "Presiona Play para probarlo.", "OK");
    }
}