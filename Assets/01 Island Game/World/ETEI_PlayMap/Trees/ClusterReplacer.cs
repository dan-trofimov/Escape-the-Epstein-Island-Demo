using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class ClusterReplacer : EditorWindow
{
    public GameObject mergedMeshObject;
    public GameObject[] prefabsToPlace;

    // SETTINGS
    public float mergeRadius = 2.0f; // Points within 2m = 1 Tree
    public float treeScale = 13.5f;  // Your tested scale
    public float scaleVariance = 1.0f;

    [MenuItem("Tools/Direct Tree Replacer")]
    public static void ShowWindow() => GetWindow<ClusterReplacer>("Direct Replacer");

    void OnGUI()
    {
        GUILayout.Label("Direct Distance Replacer", EditorStyles.boldLabel);

        mergedMeshObject = (GameObject)EditorGUILayout.ObjectField("Merged Log Object", mergedMeshObject, typeof(GameObject), true);

        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        EditorGUILayout.PropertyField(so.FindProperty("prefabsToPlace"), true);
        so.ApplyModifiedProperties();

        GUILayout.Space(10);
        mergeRadius = EditorGUILayout.FloatField("Merge Radius", mergeRadius);
        treeScale = EditorGUILayout.FloatField("Tree Scale", treeScale);

        if (GUILayout.Button("CALCULATE & REPLACE"))
        {
            if (mergedMeshObject != null && prefabsToPlace.Length > 0) ExecuteReplacement();
        }
    }

    void ExecuteReplacement()
    {
        Mesh mesh = mergedMeshObject.GetComponent<MeshFilter>().sharedMesh;
        int[] tris = mesh.triangles;
        Vector3[] verts = mesh.vertices;
        Transform tr = mergedMeshObject.transform;

        // 1. Get Center of every single triangle in World Space
        List<Vector3> faceCenters = new List<Vector3>();
        for (int i = 0; i < tris.Length; i += 3)
        {
            Vector3 v1 = verts[tris[i]];
            Vector3 v2 = verts[tris[i + 1]];
            Vector3 v3 = verts[tris[i + 2]];
            Vector3 centroid = (v1 + v2 + v3) / 3.0f;
            faceCenters.Add(tr.TransformPoint(centroid));
        }

        Debug.Log($"Found {faceCenters.Count} total faces. Clustering...");

        // 2. Group them by Distance
        List<Vector3> finalSpawnPoints = new List<Vector3>();

        while (faceCenters.Count > 0)
        {
            // Pick the first point as a seed
            Vector3 seed = faceCenters[0];
            List<Vector3> cluster = new List<Vector3>();

            // Find everything close to it (brute force for reliability)
            for (int i = faceCenters.Count - 1; i >= 0; i--)
            {
                // Ignore height difference (Y) so we grab the whole trunk
                float dist = Vector2.Distance(new Vector2(seed.x, seed.z), new Vector2(faceCenters[i].x, faceCenters[i].z));

                if (dist < mergeRadius)
                {
                    cluster.Add(faceCenters[i]);
                    faceCenters.RemoveAt(i);
                }
            }

            // Calculate the true center of this tree trunk
            Vector3 spawnPos = Vector3.zero;
            foreach (Vector3 p in cluster) spawnPos += p;
            spawnPos /= cluster.Count;

            // Set Y to the bottom of the cluster
            spawnPos.y = cluster.Min(p => p.y);

            finalSpawnPoints.Add(spawnPos);
        }

        // 3. Plant
        GameObject parent = new GameObject("Palms_Direct_Final");
        foreach (Vector3 pos in finalSpawnPoints)
        {
            GameObject prefab = prefabsToPlace[Random.Range(0, prefabsToPlace.Length)];
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            instance.transform.position = pos;
            instance.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            instance.transform.localScale = Vector3.one * (treeScale + Random.Range(-scaleVariance, scaleVariance));

            instance.transform.SetParent(parent.transform);
        }

        mergedMeshObject.SetActive(false);
        Debug.Log($"SUCCESS: Replaced {tris.Length / 3} triangles with {finalSpawnPoints.Count} trees.");
    }
}