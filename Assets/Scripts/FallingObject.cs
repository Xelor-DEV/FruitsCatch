using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float fallSpeed = 2.5f;

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Grass")
        {
            GameManager.instance.LoseLife();
            Destroy(gameObject);
        }
    }
}
