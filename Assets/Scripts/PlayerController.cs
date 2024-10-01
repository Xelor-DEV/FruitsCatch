using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeedMultiplier = 1.8f; // Multiplicador de velocidad al correr
    public float screenLimit = 8f; // Límite de movimiento en la pantalla
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Detectar si se mantiene presionada la tecla de correr
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= runSpeedMultiplier;
        }

        float move = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + new Vector3(move, 0, 0);
        newPosition.x = Mathf.Clamp(newPosition.x, -screenLimit, screenLimit);
        transform.position = newPosition;

        // Voltear el sprite según la dirección de movimiento
        if (move > 0)
        {
            spriteRenderer.flipX = false; // Mirando a la derecha
        }
        else if (move < 0)
        {
            spriteRenderer.flipX = true; // Mirando a la izquierda
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FallingObject"))
        {
            Destroy(other.gameObject);
            GameManager.instance.AddScore(Random.Range(10, 30));
        }
    }
}
