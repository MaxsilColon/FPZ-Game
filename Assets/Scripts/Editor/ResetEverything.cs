using UnityEngine;
using UnityEditor;

/// <summary>
/// Herramienta para resetear todo y dejar la escena como estaba originalmente
/// </summary>
public class ResetEverything : EditorWindow
{
    [MenuItem("Tools/RESET EVERYTHING - Leave as Original")]
    public static void ShowWindow()
    {
        GetWindow<ResetEverything>("Reset All");
    }

    private void OnGUI()
    {
        GUILayout.Label("RESET EVERYTHING", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "Esta herramienta eliminará TODOS los cambios y dejará la escena como estaba originalmente.\n\n" +
            "- Eliminará AI Companion\n" +
            "- Eliminará SplitScreenManager\n" +
            "- Eliminará AI_Companion_Spawner\n" +
            "- Activará el Player original\n" +
            "- Resetea posición del Player",
            MessageType.Warning
        );

        GUILayout.Space(10);

        if (GUILayout.Button("RESET EVERYTHING NOW", GUILayout.Height(40)))
        {
            if (EditorUtility.DisplayDialog("Confirmar Reset", 
                "¿Estás seguro de que quieres resetear todo?\n\nEsto eliminará todos los cambios del AI Companion.", 
                "Sí, resetear", "Cancelar"))
            {
                ResetAll();
            }
        }
    }

    private void ResetAll()
    {
        Debug.Log("=== RESETEANDO TODO ===");

        // 1. Eliminar AI Companion
        GameObject aiCompanion = GameObject.Find("AI_Companion");
        if (aiCompanion != null)
        {
            DestroyImmediate(aiCompanion);
            Debug.Log("✓ AI Companion eliminado");
        }

        // 2. Eliminar cualquier objeto SplitScreenManager
        GameObject splitManagerObj = GameObject.Find("SplitScreenManager");
        if (splitManagerObj != null)
        {
            DestroyImmediate(splitManagerObj);
            Debug.Log("✓ SplitScreenManager eliminado");
        }

        // 3. Eliminar AI_Companion_Spawner (si existe)
        GameObject spawnerObj = GameObject.Find("AI_Companion_Spawner");
        if (spawnerObj != null)
        {
            DestroyImmediate(spawnerObj);
            Debug.Log("✓ AI_Companion_Spawner GameObject eliminado");
        }

        // 4. Eliminar spawn points creados
        GameObject spawnP1 = GameObject.Find("SpawnPoint_Player1");
        if (spawnP1 != null)
        {
            DestroyImmediate(spawnP1);
            Debug.Log("✓ SpawnPoint_Player1 eliminado");
        }

        GameObject spawnP2 = GameObject.Find("SpawnPoint_Player2_AI");
        if (spawnP2 != null)
        {
            DestroyImmediate(spawnP2);
            Debug.Log("✓ SpawnPoint_Player2_AI eliminado");
        }

        // 5. Buscar y activar Player original
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        if (player != null)
        {
            // Activar el player
            player.SetActive(true);
            
            // NO cambiar la posición - dejarla como está
            // Solo asegurar que esté un poco arriba del suelo
            Vector3 currentPos = player.transform.position;
            if (currentPos.y < 1f)
            {
                player.transform.position = new Vector3(currentPos.x, 2f, currentPos.z);
            }
            
            Debug.Log("✓ Player activado en posición: " + player.transform.position);
        }

        // 6. Marcar escena como modificada
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );

        Debug.Log("=== RESET COMPLETADO ===");
        Debug.Log("La escena ha sido reseteada a su estado original");

        EditorUtility.DisplayDialog("Reset Completado", 
            "Todo ha sido reseteado exitosamente.\n\n" +
            "La escena está ahora como estaba originalmente.\n" +
            "Presiona Play para probar el juego normal.", "OK");
    }
}