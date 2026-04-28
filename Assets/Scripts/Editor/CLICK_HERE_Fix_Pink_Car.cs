using UnityEngine;
using UnityEditor;

public class CLICK_HERE_Fix_Pink_Car : EditorWindow
{
    [MenuItem("Tools/★ CLICK HERE - FIX PINK CAR ★", priority = 0)]
    public static void ShowWindow()
    {
        if (EditorUtility.DisplayDialog(
            "Fix Pink Car & Ground", 
            "This will fix all pink materials in the racing game.\n\nCar will be BLACK\nGround will be GRAY\n\nContinue?", 
            "YES - FIX IT!", 
            "Cancel"))
        {
            FixEverything();
        }
    }

    static void FixEverything()
    {
        int fixedCount = 0;

        // 1. Fix ALL materials in the Realistic Car Controller Pro folder
        Debug.Log("=== FIXING MATERIALS ===");
        string[] materialPaths = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Realistic Car Controller Pro" });
        
        foreach (string guid in materialPaths)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            
            if (mat != null)
            {
                // Force Standard shader
                mat.shader = Shader.Find("Standard");
                
                string name = mat.name.ToLower();
                
                // Car body materials
                if (name.Contains("body") || name.Contains("skyline"))
                {
                    mat.SetColor("_Color", Color.black);
                    mat.SetFloat("_Metallic", 0.8f);
                    mat.SetFloat("_Glossiness", 0.9f);
                    Debug.Log($"✓ CAR BODY: {mat.name} → BLACK");
                }
                // Wheel materials
                else if (name.Contains("wheel"))
                {
                    mat.SetColor("_Color", new Color(0.15f, 0.15f, 0.15f));
                    mat.SetFloat("_Metallic", 0.3f);
                    mat.SetFloat("_Glossiness", 0.5f);
                    Debug.Log($"✓ WHEEL: {mat.name} → DARK GRAY");
                }
                // Ground/Dev materials
                else if (name.Contains("dev"))
                {
                    mat.SetColor("_Color", new Color(0.6f, 0.6f, 0.6f));
                    mat.SetFloat("_Metallic", 0.1f);
                    mat.SetFloat("_Glossiness", 0.3f);
                    Debug.Log($"✓ GROUND: {mat.name} → LIGHT GRAY");
                }
                // Everything else
                else
                {
                    mat.SetColor("_Color", Color.white);
                    mat.SetFloat("_Metallic", 0.3f);
                    mat.SetFloat("_Glossiness", 0.5f);
                    Debug.Log($"✓ OTHER: {mat.name} → WHITE");
                }
                
                EditorUtility.SetDirty(mat);
                fixedCount++;
            }
        }

        // 2. Fix camera background
        Camera[] cameras = Object.FindObjectsOfType<Camera>();
        foreach (Camera cam in cameras)
        {
            cam.backgroundColor = new Color(0.5f, 0.7f, 1f); // Sky blue
            cam.clearFlags = CameraClearFlags.SolidColor;
            EditorUtility.SetDirty(cam);
            Debug.Log($"✓ CAMERA: {cam.name} → SKY BLUE");
        }

        // 3. Save everything
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // 4. Show result
        string result = $"✓✓✓ DONE! ✓✓✓\n\nFixed {fixedCount} materials\nFixed {cameras.Length} cameras\n\nCar: BLACK\nGround: GRAY\nSky: BLUE";
        Debug.Log(result);
        EditorUtility.DisplayDialog("SUCCESS!", result, "OK");
    }
}
