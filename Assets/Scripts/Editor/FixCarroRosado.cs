using UnityEngine;
using UnityEditor;

public class FixCarroRosado : EditorWindow
{
    [MenuItem("Tools/Arreglar Carro Rosado")]
    public static void ArreglarTodo()
    {
        Debug.Log("=== ARREGLANDO TODO AGRESIVAMENTE ===");
        
        int materialesArreglados = 0;
        
        // PASO 1: Arreglar TODOS los materiales del proyecto
        Debug.Log("PASO 1: Buscando TODOS los materiales...");
        string[] guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Realistic Car Controller Pro" });
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            
            if (mat != null)
            {
                // FORZAR shader Standard
                mat.shader = Shader.Find("Standard");
                
                // Verificar si es rosado
                bool esRosado = false;
                if (mat.HasProperty("_Color"))
                {
                    Color color = mat.GetColor("_Color");
                    // Si es magenta (rosado)
                    if (color.r > 0.9f && color.g < 0.1f && color.b > 0.9f)
                    {
                        esRosado = true;
                    }
                }
                
                string nombre = mat.name.ToLower();
                
                // Aplicar colores
                if (nombre.Contains("body") || nombre.Contains("skyline"))
                {
                    mat.SetColor("_Color", Color.black);
                    mat.SetFloat("_Metallic", 0.8f);
                    mat.SetFloat("_Glossiness", 0.9f);
                    Debug.Log($"✓ CARRO NEGRO: {mat.name}");
                }
                else if (nombre.Contains("wheel"))
                {
                    mat.SetColor("_Color", new Color(0.2f, 0.2f, 0.2f));
                    mat.SetFloat("_Metallic", 0.3f);
                    mat.SetFloat("_Glossiness", 0.4f);
                    Debug.Log($"✓ RUEDA: {mat.name}");
                }
                else if (nombre.Contains("dev") || esRosado)
                {
                    // Si es "dev" o es rosado, hacerlo gris
                    mat.SetColor("_Color", new Color(0.6f, 0.6f, 0.6f));
                    mat.SetFloat("_Metallic", 0.0f);
                    mat.SetFloat("_Glossiness", 0.2f);
                    Debug.Log($"✓ SUELO/ROSADO → GRIS: {mat.name}");
                }
                else
                {
                    mat.SetColor("_Color", Color.white);
                    mat.SetFloat("_Metallic", 0.2f);
                    mat.SetFloat("_Glossiness", 0.5f);
                }
                
                EditorUtility.SetDirty(mat);
                materialesArreglados++;
            }
        }
        
        // PASO 2: Arreglar TODOS los objetos en la escena
        Debug.Log("PASO 2: Arreglando objetos en escena...");
        GameObject[] todos = Object.FindObjectsOfType<GameObject>();
        int objetosArreglados = 0;
        
        foreach (GameObject obj in todos)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null && rend.sharedMaterial != null)
            {
                Material mat = rend.sharedMaterial;
                mat.shader = Shader.Find("Standard");
                
                string nombreObj = obj.name.ToLower();
                string nombreMat = mat.name.ToLower();
                
                // Verificar si es rosado
                bool esRosado = false;
                if (mat.HasProperty("_Color"))
                {
                    Color color = mat.GetColor("_Color");
                    if (color.r > 0.9f && color.g < 0.1f && color.b > 0.9f)
                    {
                        esRosado = true;
                    }
                }
                
                if (nombreObj.Contains("body") || nombreMat.Contains("body") || 
                    nombreObj.Contains("skyline") || nombreMat.Contains("skyline"))
                {
                    mat.SetColor("_Color", Color.black);
                    mat.SetFloat("_Metallic", 0.8f);
                    mat.SetFloat("_Glossiness", 0.9f);
                    Debug.Log($"✓ OBJETO CARRO: {obj.name}");
                    objetosArreglados++;
                }
                else if (nombreObj.Contains("plane") || nombreObj.Contains("ground") || 
                         nombreMat.Contains("dev") || esRosado)
                {
                    mat.SetColor("_Color", new Color(0.6f, 0.6f, 0.6f));
                    mat.SetFloat("_Metallic", 0.0f);
                    mat.SetFloat("_Glossiness", 0.2f);
                    Debug.Log($"✓ OBJETO SUELO: {obj.name}");
                    objetosArreglados++;
                }
                
                EditorUtility.SetDirty(mat);
                EditorUtility.SetDirty(rend);
            }
        }
        
        // PASO 3: Cámaras
        Camera[] camaras = Object.FindObjectsOfType<Camera>();
        foreach (Camera cam in camaras)
        {
            cam.backgroundColor = new Color(0.5f, 0.7f, 1f);
            cam.clearFlags = CameraClearFlags.SolidColor;
            EditorUtility.SetDirty(cam);
        }
        
        // Guardar
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );
        
        string mensaje = $"¡TERMINADO!\n\nMateriales: {materialesArreglados}\nObjetos: {objetosArreglados}\nCámaras: {camaras.Length}\n\nRevisa la consola para detalles";
        Debug.Log(mensaje);
        EditorUtility.DisplayDialog("¡Listo!", mensaje, "OK");
    }
}
