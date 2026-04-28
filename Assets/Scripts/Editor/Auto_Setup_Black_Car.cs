using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class Auto_Setup_Black_Car
{
    static Auto_Setup_Black_Car()
    {
        EditorSceneManager.sceneOpened += OnSceneOpened;
    }

    static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
    {
        // Si es la escena de carreras, aplicar auto negro automáticamente
        if (scene.name.Contains("RCCP") || scene.name.Contains("Racing") || scene.name.Contains("Car"))
        {
            EditorApplication.delayCall += () =>
            {
                SetupBlackCar();
            };
        }
    }

    [MenuItem("Tools/Setup Black Car NOW")]
    public static void SetupBlackCarMenu()
    {
        SetupBlackCar();
    }

    static void SetupBlackCar()
    {
        Color carColor = Color.black;
        Color groundColor = new Color(0.5f, 0.5f, 0.5f); // Gris claro para contraste
        Color skyColor = new Color(0.5f, 0.7f, 1f); // Azul cielo

        int materialsFixed = 0;
        int camerasFixed = 0;

        // PRIMERO: Arreglar TODOS los materiales rosados en el proyecto
        string[] guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Realistic Car Controller Pro" });
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            
            if (mat != null)
            {
                // Si el material está rosado o tiene shader inválido
                if (mat.shader == null || !mat.shader.isSupported || mat.shader.name.Contains("Hidden"))
                {
                    mat.shader = Shader.Find("Standard");
                    
                    // Determinar si es parte del auto o del suelo por el nombre
                    string matName = mat.name.ToLower();
                    
                    if (matName.Contains("body") || matName.Contains("skyline") || matName.Contains("car"))
                    {
                        // Material del auto - negro brillante
                        mat.SetColor("_Color", carColor);
                        mat.SetFloat("_Metallic", 0.8f);
                        mat.SetFloat("_Glossiness", 0.9f);
                    }
                    else if (matName.Contains("wheel") || matName.Contains("tire") || matName.Contains("rim"))
                    {
                        // Ruedas - negro mate
                        mat.SetColor("_Color", new Color(0.1f, 0.1f, 0.1f));
                        mat.SetFloat("_Metallic", 0.2f);
                        mat.SetFloat("_Glossiness", 0.4f);
                    }
                    else
                    {
                        // Suelo/otros - gris
                        mat.SetColor("_Color", groundColor);
                        mat.SetFloat("_Metallic", 0.1f);
                        mat.SetFloat("_Glossiness", 0.3f);
                    }
                    
                    EditorUtility.SetDirty(mat);
                    materialsFixed++;
                    Debug.Log($"Fixed material: {mat.name}");
                }
            }
        }

        // SEGUNDO: Buscar objetos en la escena y aplicar colores
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                foreach (Material mat in rend.sharedMaterials)
                {
                    if (mat != null)
                    {
                        // Asegurar shader Standard
                        if (mat.shader == null || !mat.shader.isSupported)
                        {
                            mat.shader = Shader.Find("Standard");
                        }
                        
                        string objName = rend.gameObject.name.ToLower();
                        string matName = mat.name.ToLower();
                        
                        // Identificar si es auto o suelo
                        if (objName.Contains("body") || objName.Contains("skyline") || 
                            matName.Contains("body") || matName.Contains("skyline"))
                        {
                            mat.SetColor("_Color", carColor);
                            mat.SetFloat("_Metallic", 0.8f);
                            mat.SetFloat("_Glossiness", 0.9f);
                            EditorUtility.SetDirty(mat);
                        }
                        else if (objName.Contains("ground") || objName.Contains("plane") || 
                                 objName.Contains("floor") || matName.Contains("dev"))
                        {
                            mat.SetColor("_Color", groundColor);
                            mat.SetFloat("_Metallic", 0.1f);
                            mat.SetFloat("_Glossiness", 0.3f);
                            EditorUtility.SetDirty(mat);
                        }
                    }
                }
            }
        }

        // Arreglar cámaras
        Camera[] cameras = Object.FindObjectsOfType<Camera>();
        foreach (Camera cam in cameras)
        {
            cam.backgroundColor = skyColor;
            cam.clearFlags = CameraClearFlags.SolidColor;
            EditorUtility.SetDirty(cam);
            camerasFixed++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        string message = $"✓ Auto negro configurado!\n\nMateriales arreglados: {materialsFixed}\nCámaras: {camerasFixed}";
        Debug.Log(message);
        
        if (materialsFixed > 0 || camerasFixed > 0)
        {
            EditorUtility.DisplayDialog("Auto Negro Configurado", message, "OK");
        }
    }
}
