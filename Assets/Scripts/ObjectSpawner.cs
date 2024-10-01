using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject fallingObjectPrefab;
    public float spawnInterval = 1f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnFallingObject), 0f, spawnInterval);
    }

    void SpawnFallingObject()
    {
        float randomX = Random.Range(-8f, 8f); // Cambia los valores según el tamaño de tu pantalla
        Vector3 spawnPosition = new Vector3(randomX, 6f, 0); // Cambia 6f según la altura de tu pantalla
        Instantiate(fallingObjectPrefab, spawnPosition, Quaternion.identity);
    }
}
