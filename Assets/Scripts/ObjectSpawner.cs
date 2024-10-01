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
        float randomX = Random.Range(-8f, 8f); // Cambia los valores seg�n el tama�o de tu pantalla
        Vector3 spawnPosition = new Vector3(randomX, 6f, 0); // Cambia 6f seg�n la altura de tu pantalla
        Instantiate(fallingObjectPrefab, spawnPosition, Quaternion.identity);
    }
}
