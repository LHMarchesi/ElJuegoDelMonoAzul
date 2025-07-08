using UnityEngine;

public class OvniTrigger : MonoBehaviour
{
    [SerializeField] GameObject Ovni;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHead"))
        {
            Animator animator = Ovni.GetComponent<Animator>();
            animator.SetTrigger("StartAnimation");
            Destroy(gameObject);
        }
    }
}
