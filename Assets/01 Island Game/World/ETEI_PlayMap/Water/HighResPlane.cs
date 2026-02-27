using UnityEngine;
using UnityEditor;

public class HighResPlane : MonoBehaviour
{
    [MenuItem("GameObject/3D Object/High-Res Water Plane")]
    static void Create()
    {
        // 1. Create the GameObject
        GameObject go = new GameObject("HighResWater");
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();

        // 2. Settings (Hardcoded size)
        float width = 25000f;  // X Axis
        float length = 20000f; // Z Axis
        int res = 250;        // 250x250 grid (62,500 vertices) - Max safe limit for a single mesh

        // 3. Generate Data
        Mesh m = new Mesh();
        m.name = "WaterMesh";
        m.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // Allows larger meshes if needed

        Vector3[] verts = new Vector3[(res + 1) * (res + 1)];
        Vector2[] uvs = new Vector2[verts.Length];
        int[] tris = new int[res * res * 6];

        // 4. Build Vertices (Math handles the scaling here)
        for (int y = 0; y <= res; y++)
        {
            for (int x = 0; x <= res; x++)
            {
                int i = y * (res + 1) + x;

                // Calculate position based on the hardcoded width/length
                float xPos = (float)x / res * width - (width / 2f);
                float zPos = (float)y / res * length - (length / 2f);

                verts[i] = new Vector3(xPos, 0, zPos);
                uvs[i] = new Vector2((float)x / res, (float)y / res);
            }
        }

        // 5. Build Triangles
        int t = 0;
        for (int y = 0; y < res; y++)
        {
            for (int x = 0; x < res; x++)
            {
                int i = y * (res + 1) + x;
                tris[t++] = i;
                tris[t++] = i + res + 1;
                tris[t++] = i + 1;
                tris[t++] = i + 1;
                tris[t++] = i + res + 1;
                tris[t++] = i + res + 2;
            }
        }

        // 6. Finalize
        m.vertices = verts;
        m.triangles = tris;
        m.uv = uvs;
        m.RecalculateNormals();
        m.RecalculateTangents();
        m.RecalculateBounds();

        mf.mesh = m;

        // Tries to apply the standard Lit shader, but you will swap this for Water anyway
        mr.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        Selection.activeGameObject = go;
    }
}