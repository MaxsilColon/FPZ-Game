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

        int carsFixed = 0;
        int groundsFixed = 0;
        int camerasFixed = 0;

        // Buscar todos los GameObjects en la escena
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Buscar autos (objetos con "car", "vehicle", "skyline" en el nombre)
            if (obj.name.ToLower().Contains("car") || 
                obj.name.ToLower().Contains("vehicle") || 
                obj.name.ToLower().Contains("prototype") ||
                obj.name.ToLower().Contains("skyline"))
            {
                Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
                foreach (Renderer rend in renderers)
                {
                    // Evitar ruedas y partes específicas
                    if (!rend.gameObject.name.ToLower().Contains("wheel") &&
                        !rend.gameObject.name.ToLower().Contains("tire") &&
                        !rend.gameObject.name.ToLower().Contains("rim"))
                    {
                        foreach (Material mat in rend.sharedMaterials)
                        {
                            if (mat != null)
                            {
                                // Asegurar que use shader Standard
                                mat.shader = Shader.Find("Standard");
                                
                                // Color negro brillante
                                mat.SetColor("_Color", carColor);
                                mat.SetFloat("_Metallic", 0.8f); // Muy metálico
                                mat.SetFloat("_Glossiness", 0.9f); // Muy brillante
                                
                                EditorUtility.SetDirty(mat);
                                carsFixed++;
                            }
                        }
                    }
                }
            }

            // Buscar pistas/suelo
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

        string message = $"✓ Auto negro configurado!\n\nMateriales del auto: {carsFixed}\nMateriales del suelo: {groundsFixed}\nCámaras: {camerasFixed}";
        Debug.Log(message);
        
        if (carsFixed > 0 || groundsFixed > 0 || camerasFixed > 0)
        {
            EditorUtility.DisplayDialog("Auto Negro Configurado", message, "OK");
        }
    }
}
