
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int totalCollectibles;

    void Awake()
    {
        Instance = this;
        // FindObjectsByType is safe and only looks at the CURRENT scene
        totalCollectibles = Object.FindObjectsByType<Collectible>(FindObjectsSortMode.None).Length;
    }
}
