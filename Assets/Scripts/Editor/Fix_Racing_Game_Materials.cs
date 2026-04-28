using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Fix_Racing_Game_Materials : EditorWindow
{
    [MenuItem("Tools/Fix Racing Game Materials")]
    public static void ShowWindow()
    {
        GetWindow<Fix_Racing_Game_Materials>("Fix Racing Materials");
    }

    void OnGUI()
    {
        GUILayout.Label("Fix Racing Game Pink Materials", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox("Este script arreglará los materiales rosados del juego de carreras cambiando los shaders a Standard (Built-in).", MessageType.Info);
        
        GUILayout.Space(10);

        if (GUILayout.Button("Fix All Materials", GUILayout.Height(40)))
        {
            FixAllMaterials();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Fix Camera Background", GUILayout.Height(40)))
        {
            FixCameraBackground();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Fix Everything", GUILayout.Height(50)))
        {
            FixAllMaterials();
            FixCameraBackground();
            Debug.Log("✓ Todo arreglado!");
        }
    }

    static void FixAllMaterials()
    {
        // Buscar todos los materiales en el proyecto de carreras
        string[] guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Realistic Car Controller Pro" });
        
        int fixedCount = 0;
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            
            if (mat != null)
            {
                bool needsFix = false;
                
                // Verificar si necesita arreglo
                if (mat.shader == null || !mat.shader.isSupported || mat.shader.name.Contains("Hidden"))
                {
                    needsFix = true;
                }
                
                // Verificar si el color es magenta/rosado
                if (mat.HasProperty("_Color"))
                {
                    Color currentColor = mat.GetColor("_Color");
                    if (Mathf.Approximately(currentColor.r, 1f) && Mathf.Approximately(currentColor.g, 0f) && Mathf.Approximately(currentColor.b, 1f))
                    {
                        needsFix = true;
                    }
                }
                
                if (needsFix)
                {
                    // Cambiar a shader Standard
                    mat.shader = Shader.Find("Standard");
                    
                    // Determinar color basado en el nombre del material
                    string matName = mat.name.ToLower();
                    
                    if (matName.Contains("body") || matName.Contains("skyline_body"))
                    {
                        // Carrocería del auto - negro brillante
                        mat.SetColor("_Color", Color.black);
                        mat.SetFloat("_Metallic", 0.8f);
                        mat.SetFloat("_Glossiness", 0.9f);
                    }
                    else if (matName.Contains("wheel") || matName.Contains("tire"))
                    {
                        // Ruedas - negro mate
                        mat.SetColor("_Color", new Color(0.1f, 0.1f, 0.1f));
                        mat.SetFloat("_Metallic", 0.2f);
                        mat.SetFloat("_Glossiness", 0.4f);
                    }
                    else if (matName.Contains("dev") || matName.Contains("ground") || matName.Contains("road"))
                    {
                        // Suelo - gris claro
                        mat.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f));
                        mat.SetFloat("_Metallic", 0.1f);
                        mat.SetFloat("_Glossiness", 0.3f);
                    }
                    else
                    {
                        // Otros - blanco por defecto
                        mat.SetColor("_Color", Color.white);
                        mat.SetFloat("_Metallic", 0.3f);
                        mat.SetFloat("_Glossiness", 0.5f);
                    }
                    
                    EditorUtility.SetDirty(mat);
                    fixedCount++;
                    Debug.Log($"Fixed material: {mat.name} at {path}");
                }
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"✓ Fixed {fixedCount} materials!");
        EditorUtility.DisplayDialog("Success", $"Fixed {fixedCount} materials!\n\nAuto: Negro brillante\nSuelo: Gris claro", "OK");
    }

    static void FixCameraBackground()
    {
        // Buscar todas las cámaras en la escena
        Camera[] cameras = FindObjectsOfType<Camera>();
        
        foreach (Camera cam in cameras)
        {
            // Cambiar el color de fondo a un color cielo
            cam.backgroundColor = new Color(0.5f, 0.7f, 1f); // Azul cielo
            cam.clearFlags = CameraClearFlags.SolidColor;
            
            EditorUtility.SetDirty(cam);
            Debug.Log($"Fixed camera: {cam.name}");
        }
        
        Debug.Log($"✓ Fixed {cameras.Length} cameras!");
    }
}
