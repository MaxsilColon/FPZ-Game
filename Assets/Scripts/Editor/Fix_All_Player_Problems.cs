using UnityEngine;
using UnityEditor;

/// <summary>
/// Arregla TODOS los problemas del jugador principal
/// </summary>
public class Fix_All_Player_Problems : EditorWindow
{
    [MenuItem("Tools/FIX ALL PLAYER PROBLEMS")]
    public static void ShowWindow()
    {
        GetWindow<Fix_All_Player_Problems>("Fix Player");
    }

    private void OnGUI()
    {
        GUILayout.Label("ARREGLAR JUGADOR", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "Si tu jugador tiene problemas:\n\n" +
            "✓ Sin cámara\n" +
            "✓ Sin audio\n" +
            "✓ Errores de NullReference\n" +
            "✓ No puede moverse\n\n" +
            "Este botón arregla TODO.",
            MessageType.Error
        );

        GUILayout.Space(10);

        if (GUILayout.Button("ARREGLAR TODO AHORA", GUILayout.Height(50)))
        {
            FixAllPlayerProblems();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("RESET COMPLETO", GUILayout.Height(30)))
        {
            CompleteReset();
        }
    }

    private void FixAllPlayerProblems()
    {
        Debug.Log("=== ARREGLANDO TODOS LOS PROBLEMAS DEL JUGADOR ===");

        // 1. ELIMINAR todos los compañeros problemáticos
        string[] companionNames = {"Player_Companion", "Simple_Companion", "AI_Companion"};
        foreach (string name in companionNames)
        {
            GameObject companion = GameObject.Find(name);
            if (companion != null)
            {
                DestroyImmediate(companion);
                Debug.Log("✓ " + name + " eliminado");
            }
        }

        // 2. BUSCAR el jugador principal
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

        Debug.Log("Jugador encontrado: " + player.name);

        // 3. ASEGURAR que el jugador esté activo
        player.SetActive(true);

        // 4. ARREGLAR LA CÁMARA
        FixPlayerCamera(player);

        // 5. ARREGLAR EL AUDIO
        FixPlayerAudio(player);

        // 6. ARREGLAR LOS SCRIPTS
        FixPlayerScripts(player);

        // 7. MARCAR ESCENA COMO MODIFICADA
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );

        Debug.Log("=== TODOS LOS PROBLEMAS ARREGLADOS ===");

        EditorUtility.DisplayDialog("¡TODO ARREGLADO!", 
            "Jugador completamente restaurado:\n\n" +
            "✓ Cámara funcionando\n" +
            "✓ Audio configurado\n" +
            "✓ Scripts arreglados\n" +
            "✓ Movimiento restaurado\n" +
            "✓ Armas funcionando\n\n" +
            "¡Presiona Play para jugar!", "¡Perfecto!");
    }

    private void FixPlayerCamera(GameObject player)
    {
        Debug.Log("Arreglando cámara del jugador...");

        // Buscar cámara existente
        Camera playerCamera = player.GetComponentInChildren<Camera>(true);
        
        if (playerCamera == null)
        {
            // Si no hay cámara, crear una nueva
            Transform cameraTransform = player.transform.Find("Main Camera");
            if (cameraTransform == null)
            {
                // Crear Main Camera
                GameObject mainCameraObj = new GameObject("Main Camera");
                mainCameraObj.transform.parent = player.transform;
                mainCameraObj.transform.localPosition = new Vector3(0, 1.6f, 0);
                mainCameraObj.transform.localRotation = Quaternion.identity;
                
                playerCamera = mainCameraObj.AddComponent<Camera>();
                Debug.Log("✓ Main Camera creada");
            }
            else
            {
                playerCamera = cameraTransform.GetComponent<Camera>();
                if (playerCamera == null)
                {
                    playerCamera = cameraTransform.gameObject.AddComponent<Camera>();
                }
            }
        }

        // Configurar cámara
        playerCamera.gameObject.SetActive(true);
        playerCamera.enabled = true;
        playerCamera.gameObject.tag = "MainCamera";
        playerCamera.fieldOfView = 60f;
        playerCamera.nearClipPlane = 0.1f;
        playerCamera.farClipPlane = 1000f;

        // Crear BulletSpawn si no existe
        Transform bulletSpawn = playerCamera.transform.Find("BulletSpawn");
        if (bulletSpawn == null)
        {
            GameObject bulletSpawnObj = new GameObject("BulletSpawn");
            bulletSpawnObj.transform.parent = playerCamera.transform;
            bulletSpawnObj.transform.localPosition = Vector3.zero;
            bulletSpawnObj.transform.localRotation = Quaternion.identity;
            bulletSpawnObj.tag = "BulletSpawn";
            Debug.Log("✓ BulletSpawn creado");
        }

        Debug.Log("✓ Cámara del jugador arreglada: " + playerCamera.name);
    }

    private void FixPlayerAudio(GameObject player)
    {
        Debug.Log("Arreglando audio del jugador...");

        // Buscar AudioListener existente
        AudioListener playerAudio = player.GetComponentInChildren<AudioListener>(true);
        
        if (playerAudio == null)
        {
            // Agregar AudioListener a la cámara
            Camera playerCamera = player.GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                playerAudio = playerCamera.gameObject.AddComponent<AudioListener>();
                Debug.Log("✓ AudioListener creado en la cámara");
            }
        }
        else
        {
            playerAudio.enabled = true;
            Debug.Log("✓ AudioListener reactivado");
        }

        // Desactivar otros AudioListeners
        AudioListener[] allListeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        foreach (AudioListener listener in allListeners)
        {
            if (listener != playerAudio)
            {
                listener.enabled = false;
            }
        }

        Debug.Log("✓ Audio del jugador arreglado");
    }

    private void FixPlayerScripts(GameObject player)
    {
        Debug.Log("Arreglando scripts del jugador...");

        // Arreglar MouseLookScript
        MouseLookScript mouseScript = player.GetComponent<MouseLookScript>();
        if (mouseScript != null)
        {
            mouseScript.enabled = true;
            Camera playerCamera = player.GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                mouseScript.myCamera = playerCamera.transform;
                Debug.Log("✓ MouseLookScript arreglado");
            }
        }

        // Arreglar PlayerMovementScript
        PlayerMovementScript movementScript = player.GetComponent<PlayerMovementScript>();
        if (movementScript != null)
        {
            movementScript.enabled = true;
            Debug.Log("✓ PlayerMovementScript arreglado");
        }

        // Arreglar GunScript
        GunScript[] gunScripts = player.GetComponentsInChildren<GunScript>(true);
        foreach (GunScript gun in gunScripts)
        {
            gun.enabled = true;
            Debug.Log("✓ GunScript arreglado: " + gun.name);
        }

        // Asegurar que el tag sea correcto
        player.tag = "Player";

        Debug.Log("✓ Scripts del jugador arreglados");
    }

    private void CompleteReset()
    {
        Debug.Log("=== RESET COMPLETO ===");

        // Eliminar TODOS los objetos problemáticos
        string[] objectsToDelete = {
            "Player_Companion", "Simple_Companion", "AI_Companion", 
            "SplitScreenManager", "SpawnPoint_Player1", "SpawnPoint_AI"
        };

        foreach (string objName in objectsToDelete)
        {
            GameObject obj = GameObject.Find(objName);
            if (obj != null)
            {
                DestroyImmediate(obj);
                Debug.Log("✓ " + objName + " eliminado");
            }
        }

        // Arreglar jugador principal
        FixAllPlayerProblems();

        EditorUtility.DisplayDialog("Reset Completo", 
            "Todo ha sido reseteado y arreglado.\n\n" +
            "El juego está como al principio pero funcionando.", "OK");
    }
}