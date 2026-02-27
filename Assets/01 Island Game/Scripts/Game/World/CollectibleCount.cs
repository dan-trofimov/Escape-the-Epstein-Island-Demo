using UnityEngine;

public class CollectibleCount : MonoBehaviour
{
    TMPro.TMP_Text text;
    public int count;
    public int collectibleTotal;

    void Awake()
    {
        collectibleTotal = 0;
        text = GetComponent<TMPro.TMP_Text>();
    }

    void Start() => UpdateCount();

    void OnEnable() => Collectible.OnCollected += OnCollectibleCollected;
    void OnDisable() => Collectible.OnCollected -= OnCollectibleCollected;

        void OnCollectibleCollected()
    {
        count++;
        UpdateCount();
    }

    void UpdateCount()
    {
        
        collectibleTotal = LevelManager.Instance.totalCollectibles;
        text.text = $"{count} - {collectibleTotal}";
    }
}
