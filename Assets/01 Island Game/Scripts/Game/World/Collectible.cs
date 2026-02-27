using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public static event Action OnCollected;
    public static int total;

    void Awake() => total++; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollected?.Invoke();
            Destroy(gameObject);
        }
    }
}
