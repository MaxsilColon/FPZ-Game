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
        // Buscar todos los materiales en el proyecto
        string[] guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Realistic Car Controller Pro" });
        
        int fixedCount = 0;
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            
            if (mat != null)
            {
                // Si el material está rosado (shader no encontrado o incompatible)
                if (mat.shader == null || mat.shader.name.Contains("Hidden") || !mat.shader.isSupported)
                {
                    // Cambiar a shader Standard
                    mat.shader = Shader.Find("Standard");
                    
                    // Configurar propiedades básicas
                    if (mat.HasProperty("_Color"))
                    {
                        // Si el color es magenta/rosado, cambiarlo a blanco
                        Color currentColor = mat.GetColor("_Color");
                        if (Mathf.Approximately(currentColor.r, 1f) && Mathf.Approximately(currentColor.g, 0f) && Mathf.Approximately(currentColor.b, 1f))
                        {
                            mat.SetColor("_Color", Color.white);
                        }
                    }
                    
                    EditorUtility.SetDirty(mat);
                    fixedCount++;
                    Debug.Log($"Fixed material: {mat.name}");
                }
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"✓ Fixed {fixedCount} materials!");
        EditorUtility.DisplayDialog("Success", $"Fixed {fixedCount} materials!", "OK");
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
