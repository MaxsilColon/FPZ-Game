using UnityEngine;
using UnityEditor;

public class Setup_Racing_Colors : EditorWindow
{
    private Color carColor = Color.black; // Negro por defecto
    private Color groundColor = new Color(0.5f, 0.5f, 0.5f); // Gris claro para contraste con negro
    private Color skyColor = new Color(0.5f, 0.7f, 1f); // Azul cielo

    [MenuItem("Tools/Setup Racing Colors")]
    public static void ShowWindow()
    {
        GetWindow<Setup_Racing_Colors>("Racing Colors");
    }

    void OnGUI()
    {
        GUILayout.Label("Configure Racing Game Colors", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox("Configura los colores del auto, pista y cielo para que haya diferencia visual.", MessageType.Info);
        
        GUILayout.Space(10);

        // Color del auto
        GUILayout.Label("Car Color:", EditorStyles.boldLabel);
        carColor = EditorGUILayout.ColorField("Car Body Color", carColor);
        
        GUILayout.Space(10);

        // Color del suelo/pista
        GUILayout.Label("Ground/Track Color:", EditorStyles.boldLabel);
        groundColor = EditorGUILayout.ColorField("Ground Color", groundColor);
        
        GUILayout.Space(10);

        // Color del cielo
        GUILayout.Label("Sky/Background Color:", EditorStyles.boldLabel);
        skyColor = EditorGUILayout.ColorField("Sky Color", skyColor);
        
        GUILayout.Space(20);

        // Botones de presets
        GUILayout.Label("Quick Presets:", EditorStyles.boldLabel);
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Black Car ★", GUILayout.Height(35)))
        {
            carColor = Color.black;
            groundColor = new Color(0.5f, 0.5f, 0.5f);
            skyColor = new Color(0.5f, 0.7f, 1f);
        }
        if (GUILayout.Button("Red Car"))
        {
            carColor = Color.red;
            groundColor = new Color(0.3f, 0.3f, 0.3f);
            skyColor = new Color(0.5f, 0.7f, 1f);
        }
        if (GUILayout.Button("Blue Car"))
        {
            carColor = Color.blue;
            groundColor = new Color(0.3f, 0.3f, 0.3f);
            skyColor = new Color(0.5f, 0.7f, 1f);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Yellow Car"))
        {
            carColor = Color.yellow;
            groundColor = new Color(0.3f, 0.3f, 0.3f);
            skyColor = new Color(0.5f, 0.7f, 1f);
        }
        if (GUILayout.Button("Green Car"))
        {
            carColor = Color.green;
            groundColor = new Color(0.3f, 0.3f, 0.3f);
            skyColor = new Color(0.5f, 0.7f, 1f);
        }
        if (GUILayout.Button("White Car"))
        {
            carColor = Color.white;
            groundColor = new Color(0.2f, 0.2f, 0.2f);
            skyColor = new Color(0.5f, 0.7f, 1f);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        if (GUILayout.Button("Apply Colors to Scene", GUILayout.Height(50)))
        {
            ApplyColors();
        }
    }

    void ApplyColors()
    {
        int carsFixed = 0;
        int groundsFixed = 0;
        int camerasFixed = 0;

        // Buscar todos los GameObjects en la escena
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Buscar autos (objetos con "car", "vehicle" en el nombre)
            if (obj.name.ToLower().Contains("car") || 
                obj.name.ToLower().Contains("vehicle") || 
                obj.name.ToLower().Contains("prototype"))
            {
                Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
                foreach (Renderer rend in renderers)
                {
                    // Evitar ruedas y partes específicas
                    if (!rend.gameObject.name.ToLower().Contains("wheel") &&
                        !rend.gameObject.name.ToLower().Contains("tire"))
                    {
                        foreach (Material mat in rend.sharedMaterials)
                        {
                            if (mat != null)
                            {
                                mat.shader = Shader.Find("Standard");
                                mat.SetColor("_Color", carColor);
                                mat.SetFloat("_Metallic", 0.8f); // Más metálico para negro brillante
                                mat.SetFloat("_Glossiness", 0.9f); // Más brillante
                                EditorUtility.SetDirty(mat);
                                carsFixed++;
                            }
                        }
                    }
                }
            }

            // Buscar pistas/suelo (objetos con "ground", "road", "track", "plane" en el nombre)
            if (obj.name.ToLower().Contains("ground") || 
                obj.name.ToLower().Contains("road") || 
                obj.name.ToLower().Contains("track") ||
                obj.name.ToLower().Contains("plane") ||
                obj.name.ToLower().Contains("floor"))
            {
                Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
                foreach (Renderer rend in renderers)
                {
                    foreach (Material mat in rend.sharedMaterials)
                    {
                        if (mat != null)
                        {
                            mat.shader = Shader.Find("Standard");
                            mat.SetColor("_Color", groundColor);
                            mat.SetFloat("_Metallic", 0.1f);
                            mat.SetFloat("_Glossiness", 0.3f);
                            EditorUtility.SetDirty(mat);
                            groundsFixed++;
                        }
                    }
                }
            }
        }

        // Arreglar cámaras
        Camera[] cameras = FindObjectsOfType<Camera>();
        foreach (Camera cam in cameras)
        {
            cam.backgroundColor = skyColor;
            cam.clearFlags = CameraClearFlags.SolidColor;
            EditorUtility.SetDirty(cam);
            camerasFixed++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        string message = $"✓ Applied colors!\n\nCar materials: {carsFixed}\nGround materials: {groundsFixed}\nCameras: {camerasFixed}";
        Debug.Log(message);
        EditorUtility.DisplayDialog("Success", message, "OK");
    }
}
