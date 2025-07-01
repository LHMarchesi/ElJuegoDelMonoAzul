using UnityEngine;

public class DownLimit : MonoBehaviour
{
    [SerializeField]Transform spawnPoint;
    [SerializeField]GameObject Player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            Instantiate(Player, spawnPoint.position, Quaternion.identity);
        }
    }
}
