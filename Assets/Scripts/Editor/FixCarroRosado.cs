using UnityEngine;
using UnityEditor;

public class FixCarroRosado : EditorWindow
{
    [MenuItem("Tools/Arreglar Carro Rosado")]
    public static void ArreglarTodo()
    {
        Debug.Log("=== INICIANDO ARREGLO ===");
        
        int materialesArreglados = 0;
        
        // Buscar TODOS los materiales en la carpeta del juego de carreras
        string[] guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Realistic Car Controller Pro" });
        
        Debug.Log($"Encontrados {guids.Length} materiales para revisar");
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            
            if (mat != null)
            {
                // Cambiar SIEMPRE a shader Standard
                mat.shader = Shader.Find("Standard");
                
                string nombreMaterial = mat.name.ToLower();
                
                // Determinar color según el nombre
                if (nombreMaterial.Contains("body") || nombreMaterial.Contains("skyline_body"))
                {
                    // CARRO - Negro brillante
                    mat.SetColor("_Color", Color.black);
                    mat.SetFloat("_Metallic", 0.8f);
                    mat.SetFloat("_Glossiness", 0.9f);
                    Debug.Log($"CARRO NEGRO: {mat.name}");
                }
                else if (nombreMaterial.Contains("wheel"))
                {
                    // RUEDAS - Negro mate
                    mat.SetColor("_Color", new Color(0.2f, 0.2f, 0.2f));
                    mat.SetFloat("_Metallic", 0.3f);
                    mat.SetFloat("_Glossiness", 0.4f);
                    Debug.Log($"RUEDA: {mat.name}");
                }
                else if (nombreMaterial.Contains("dev"))
                {
                    // SUELO - Gris claro
                    mat.SetColor("_Color", new Color(0.6f, 0.6f, 0.6f));
                    mat.SetFloat("_Metallic", 0.0f);
                    mat.SetFloat("_Glossiness", 0.2f);
                    Debug.Log($"SUELO GRIS: {mat.name}");
                }
                else
                {
                    // OTROS - Blanco
                    mat.SetColor("_Color", Color.white);
                    mat.SetFloat("_Metallic", 0.2f);
                    mat.SetFloat("_Glossiness", 0.5f);
                    Debug.Log($"OTRO: {mat.name}");
                }
                
                EditorUtility.SetDirty(mat);
                materialesArreglados++;
            }
        }
        
        // Arreglar cámaras
        Camera[] camaras = Object.FindObjectsOfType<Camera>();
        foreach (Camera cam in camaras)
        {
            cam.backgroundColor = new Color(0.5f, 0.7f, 1f); // Azul cielo
            cam.clearFlags = CameraClearFlags.SolidColor;
            EditorUtility.SetDirty(cam);
        }
        
        // Guardar todo
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        string mensaje = $"LISTO!\n\nMateriales arreglados: {materialesArreglados}\nCámaras: {camaras.Length}\n\nCarro: NEGRO\nSuelo: GRIS\nCielo: AZUL";
        Debug.Log(mensaje);
        EditorUtility.DisplayDialog("Arreglado!", mensaje, "OK");
    }
}
