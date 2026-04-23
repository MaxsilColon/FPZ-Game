using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Visualizador de NavMesh para debugging
/// Adjunta este script a cualquier GameObject en la escena para ver el NavMesh en tiempo de ejecución
/// </summary>
public class NavMeshDebugVisualizer : MonoBehaviour
{
    [Header("Visualization Settings")]
    [Tooltip("Mostrar el NavMesh en la vista Scene")]
    public bool showNavMesh = true;
    
    [Tooltip("Color del NavMesh")]
    public Color navMeshColor = new Color(0f, 0.75f, 1f, 0.5f);
    
    [Tooltip("Mostrar bordes del NavMesh")]
    public bool showNavMeshEdges = true;
    
    [Tooltip("Color de los bordes")]
    public Color edgeColor = new Color(0f, 0.5f, 1f, 1f);
    
    [Header("Test Settings")]
    [Tooltip("Posición de prueba para verificar si está en NavMesh")]
    public Transform testPosition;
    
    [Tooltip("Radio de búsqueda para NavMesh.SamplePosition")]
    public float sampleRadius = 5f;
    
    private NavMeshTriangulation triangulation;
    private bool navMeshExists = false;
    
    private void Start()
    {
        ValidateNavMesh();
    }
    
    private void ValidateNavMesh()
    {
        triangulation = NavMesh.CalculateTriangulation();
        navMeshExists = triangulation.vertices.Length > 0;
        
        if (navMeshExists)
        {
            Debug.Log($"[NavMeshDebugVisualizer] NavMesh found with {triangulation.vertices.Length} vertices");
        }
        else
        {
            Debug.LogWarning("[NavMeshDebugVisualizer] No NavMesh found! Please bake NavMesh in the Navigation window.");
        }
    }
    
    private void OnDrawGizmos()
    {
        if (!showNavMesh || !navMeshExists)
            return;
        
        // Draw NavMesh triangles
        Gizmos.color = navMeshColor;
        for (int i = 0; i < triangulation.indices.Length; i += 3)
        {
            Vector3 v1 = triangulation.vertices[triangulation.indices[i]];
            Vector3 v2 = triangulation.vertices[triangulation.indices[i + 1]];
            Vector3 v3 = triangulation.vertices[triangulation.indices[i + 2]];
            
            // Draw filled triangle
            DrawTriangle(v1, v2, v3, navMeshColor);
            
            // Draw edges
            if (showNavMeshEdges)
            {
                Gizmos.color = edgeColor;
                Gizmos.DrawLine(v1, v2);
                Gizmos.DrawLine(v2, v3);
                Gizmos.DrawLine(v3, v1);
            }
        }
        
        // Test position if provided
        if (testPosition != null)
        {
            TestPosition(testPosition.position);
        }
    }
    
    private void DrawTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(v1, v2);
        Gizmos.DrawLine(v2, v3);
        Gizmos.DrawLine(v3, v1);
    }
    
    private void TestPosition(Vector3 position)
    {
        NavMeshHit hit;
        bool isOnNavMesh = NavMesh.SamplePosition(position, out hit, sampleRadius, NavMesh.AllAreas);
        
        if (isOnNavMesh)
        {
            // Position is on NavMesh - draw green sphere
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(position, 0.3f);
            
            // Draw line to nearest NavMesh point
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(position, hit.position);
            Gizmos.DrawSphere(hit.position, 0.2f);
        }
        else
        {
            // Position is NOT on NavMesh - draw red sphere
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(position, 0.3f);
            Gizmos.DrawWireSphere(position, sampleRadius);
        }
    }
    
    /// <summary>
    /// Verifica si una posición está en el NavMesh
    /// </summary>
    public bool IsPositionOnNavMesh(Vector3 position, float maxDistance = 1f)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, maxDistance, NavMesh.AllAreas);
    }
    
    /// <summary>
    /// Obtiene el punto más cercano en el NavMesh
    /// </summary>
    public Vector3 GetNearestNavMeshPosition(Vector3 position, float maxDistance = 5f)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, maxDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return position;
    }
    
    /// <summary>
    /// Verifica si hay un path válido entre dos posiciones
    /// </summary>
    public bool HasValidPath(Vector3 start, Vector3 end)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }
    
#if UNITY_EDITOR
    [ContextMenu("Refresh NavMesh Data")]
    private void RefreshNavMeshData()
    {
        ValidateNavMesh();
        Debug.Log("[NavMeshDebugVisualizer] NavMesh data refreshed");
    }
    
    [ContextMenu("Print NavMesh Info")]
    private void PrintNavMeshInfo()
    {
        ValidateNavMesh();
        
        if (navMeshExists)
        {
            Debug.Log("=== NavMesh Information ===");
            Debug.Log($"Vertices: {triangulation.vertices.Length}");
            Debug.Log($"Triangles: {triangulation.indices.Length / 3}");
            Debug.Log($"Areas: {triangulation.areas.Length}");
            
            // Calculate total area
            float totalArea = 0f;
            for (int i = 0; i < triangulation.indices.Length; i += 3)
            {
                Vector3 v1 = triangulation.vertices[triangulation.indices[i]];
                Vector3 v2 = triangulation.vertices[triangulation.indices[i + 1]];
                Vector3 v3 = triangulation.vertices[triangulation.indices[i + 2]];
                totalArea += CalculateTriangleArea(v1, v2, v3);
            }
            
            Debug.Log($"Total NavMesh Area: {totalArea:F2} square units");
            Debug.Log("========================");
        }
        else
        {
            Debug.LogWarning("No NavMesh data available");
        }
    }
    
    private float CalculateTriangleArea(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        Vector3 side1 = v2 - v1;
        Vector3 side2 = v3 - v1;
        return Vector3.Cross(side1, side2).magnitude * 0.5f;
    }
#endif
}
