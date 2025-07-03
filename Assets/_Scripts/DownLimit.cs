using UnityEngine;

public class DownLimit : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] float waterSpeed;
    [SerializeField] bool isWaterRising;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHead"))
        {
            PlayerController controller = collision.GetComponentInParent<PlayerController>();
            controller.enabled = false; // Disable player movement
            controller.DetachMonkeyHands(); // Detach monkey hands

            controller.playerRb.gravityScale = 0.1f;
            controller.playerRb.linearDamping = 100f;

            UIManager.Instance.SetGameOver(true); // Show game over UI
        }
    }

    private void Update()
    {
        if (isWaterRising)
        {
            transform.localScale += new Vector3(0, waterSpeed * Time.deltaTime, 0); // Agrandar en el eje Y
        }
    }
}
