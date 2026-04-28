using UnityEngine;
using UnityEditor;

public class FixCarroRosado : EditorWindow
{
    [MenuItem("Tools/Arreglar Carro Rosado")]
    public static void ArreglarTodo()
    {
        Debug.Log("=== INICIANDO ARREGLO ===");
        
        int materialesProyecto = 0;
        int materialesEscena = 0;
        
        // PASO 1: Arreglar materiales en el PROYECTO
        Debug.Log("PASO 1: Arreglando materiales del proyecto...");
        string[] guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Realistic Car Controller Pro" });
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            
            if (mat != null)
            {
                mat.shader = Shader.Find("Standard");
                
                string nombreMaterial = mat.name.ToLower();
                
                if (nombreMaterial.Contains("body") || nombreMaterial.Contains("skyline_body"))
                {
                    mat.SetColor("_Color", Color.black);
                    mat.SetFloat("_Metallic", 0.8f);
                    mat.SetFloat("_Glossiness", 0.9f);
                }
                else if (nombreMaterial.Contains("wheel"))
                {
                    mat.SetColor("_Color", new Color(0.2f, 0.2f, 0.2f));
                    mat.SetFloat("_Metallic", 0.3f);
                    mat.SetFloat("_Glossiness", 0.4f);
                }
                else if (nombreMaterial.Contains("dev"))
                {
                    mat.SetColor("_Color", new Color(0.6f, 0.6f, 0.6f));
                    mat.SetFloat("_Metallic", 0.0f);
                    mat.SetFloat("_Glossiness", 0.2f);
                }
                else
                {
                    mat.SetColor("_Color", Color.white);
                    mat.SetFloat("_Metallic", 0.2f);
                    mat.SetFloat("_Glossiness", 0.5f);
                }
                
                EditorUtility.SetDirty(mat);
                materialesProyecto++;
            }
        }
        
        // PASO 2: Arreglar materiales en la ESCENA ACTUAL
        Debug.Log("PASO 2: Arreglando objetos en la escena...");
        GameObject[] todosLosObjetos = Object.FindObjectsOfType<GameObject>();
        
        foreach (GameObject obj in todosLosObjetos)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>(true);
            
            foreach (Renderer rend in renderers)
            {
                if (rend.sharedMaterials != null)
                {
                    foreach (Material mat in rend.sharedMaterials)
                    {
                        if (mat != null)
                        {
                            mat.shader = Shader.Find("Standard");
                            
                            string nombreObj = obj.name.ToLower();
                            string nombreMat = mat.name.ToLower();
                            
                            // Identificar si es carro o suelo
                            if (nombreObj.Contains("body") || nombreMat.Contains("body") || 
                                nombreObj.Contains("skyline") || nombreMat.Contains("skyline"))
                            {
                                mat.SetColor("_Color", Color.black);
                                mat.SetFloat("_Metallic", 0.8f);
                                mat.SetFloat("_Glossiness", 0.9f);
                                Debug.Log($"CARRO NEGRO: {obj.name} - {mat.name}");
                            }
                            else if (nombreObj.Contains("wheel") || nombreMat.Contains("wheel"))
                            {
                                mat.SetColor("_Color", new Color(0.2f, 0.2f, 0.2f));
                                mat.SetFloat("_Metallic", 0.3f);
                                mat.SetFloat("_Glossiness", 0.4f);
                                Debug.Log($"RUEDA: {obj.name} - {mat.name}");
                            }
                            else if (nombreObj.Contains("plane") || nombreObj.Contains("ground") || 
                                     nombreObj.Contains("floor") || nombreMat.Contains("dev"))
                            {
                                mat.SetColor("_Color", new Color(0.6f, 0.6f, 0.6f));
                                mat.SetFloat("_Metallic", 0.0f);
                                mat.SetFloat("_Glossiness", 0.2f);
                                Debug.Log($"SUELO GRIS: {obj.name} - {mat.name}");
                            }
                            
                            EditorUtility.SetDirty(mat);
                            EditorUtility.SetDirty(rend);
                            materialesEscena++;
                        }
                    }
                }
            }
        }
        
        // PASO 3: Arreglar cámaras
        Debug.Log("PASO 3: Arreglando cámaras...");
        Camera[] camaras = Object.FindObjectsOfType<Camera>();
        foreach (Camera cam in camaras)
        {
            cam.backgroundColor = new Color(0.5f, 0.7f, 1f);
            cam.clearFlags = CameraClearFlags.SolidColor;
            EditorUtility.SetDirty(cam);
        }
        
        // Guardar todo
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );
        
        string mensaje = $"LISTO!\n\nMateriales proyecto: {materialesProyecto}\nMateriales escena: {materialesEscena}\nCámaras: {camaras.Length}\n\nCarro: NEGRO\nSuelo: GRIS\nCielo: AZUL";
        Debug.Log(mensaje);
        EditorUtility.DisplayDialog("Arreglado!", mensaje, "OK");
    }
}
