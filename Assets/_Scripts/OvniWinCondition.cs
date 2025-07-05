using System.Collections;
using UnityEngine;

public class OvniWinCondition : MonoBehaviour
{
    //    [SerializeField] Sprite newSprite;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("PlayerHead"))
        {
            //       SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            //      spriteRenderer.sprite = newSprite;

            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("TriggerWin");

            foreach (Transform child in collision.transform)
            {
                child.gameObject.SetActive(false);
            }

            collision.gameObject.SetActive(false);
           

            StartCoroutine(DelayedWin());
        }
    }

    private IEnumerator DelayedWin()
    {
        yield return new WaitForSeconds(3f);
        UIManager.Instance.SetWin(true);
    }
}
