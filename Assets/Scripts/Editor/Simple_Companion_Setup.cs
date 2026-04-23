using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

/// <summary>
/// Setup súper simple para crear un compañero que te siga
/// </summary>
public class Simple_Companion_Setup : EditorWindow
{
    [MenuItem("Tools/SIMPLE COMPANION - Just Follow Me")]
    public static void ShowWindow()
    {
        GetWindow<Simple_Companion_Setup>("Simple Companion");
    }

    private void OnGUI()
    {
        GUILayout.Label("COMPAÑERO SIMPLE", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "Esto creará un compañero simple que:\n\n" +
            "✓ Te sigue a donde vayas\n" +
            "✓ No tiene cámara (no interfiere)\n" +
            "✓ No hace nada más\n" +
            "✓ Súper simple, sin errores\n\n" +
            "Solo presiona el botón y listo.",
            MessageType.Info
        );

        GUILayout.Space(10);

        if (GUILayout.Button("CREAR COMPAÑERO SIMPLE", GUILayout.Height(50)))
        {
            CreateSimpleCompanion();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("ELIMINAR COMPAÑERO", GUILayout.Height(30)))
        {
            RemoveCompanion();
        }
    }

    private void CreateSimpleCompanion()
    {
        Debug.Log("=== CREANDO COMPAÑERO SIMPLE ===");

        // Eliminar compañero anterior si existe
        RemoveCompanion();

        // Buscar jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        if (player == null)
        {
            EditorUtility.DisplayDialog("Error", "No se encontró el jugador en la escena.", "OK");
            return;
        }

        // Cargar prefab del jugador
        GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
        if (playerPrefab == null)
        {
            EditorUtility.DisplayDialog("Error", "No se encontró Player.prefab en Assets/Prefabs/", "OK");
            return;
        }

        // Crear compañero desde el prefab del jugador
        Vector3 spawnPos = player.transform.position + player.transform.right * 2f;
        GameObject companion = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;
        companion.name = "Simple_Companion";
        companion.transform.position = spawnPos;
        companion.transform.rotation = player.transform.rotation;

        // Agregar NavMeshAgent
        NavMeshAgent navAgent = companion.GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            navAgent = companion.AddComponent<NavMeshAgent>();
        }
        navAgent.speed = 3.5f;
        navAgent.stoppingDistance = 3f;

        // Agregar script de seguimiento
        Simple_Companion companionScript = companion.AddComponent<Simple_Companion>();

        // Eliminar cámara del compañero
        Camera companionCamera = companion.GetComponentInChildren<Camera>();
        if (companionCamera != null)
        {
            companionCamera.gameObject.SetActive(false);
        }

        // Eliminar AudioListener del compañero
        AudioListener audioListener = companion.GetComponentInChildren<AudioListener>();
        if (audioListener != null)
        {
            audioListener.enabled = false;
        }

        // Marcar escena como modificada
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );

        Debug.Log("✓ Compañero simple creado exitosamente");

        EditorUtility.DisplayDialog("¡ÉXITO!", 
            "Compañero simple creado.\n\n" +
            "✓ Te seguirá a donde vayas\n" +
            "✓ Sin cámaras, sin problemas\n" +
            "✓ Súper simple\n\n" +
            "Presiona Play para probarlo.", "OK");
    }

    private void RemoveCompanion()
    {
        GameObject companion = GameObject.Find("Simple_Companion");
        if (companion != null)
        {
            DestroyImmediate(companion);
            Debug.Log("✓ Compañero anterior eliminado");
        }
    }
}