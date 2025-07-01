using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveForce;
    [SerializeField] private float jumpImpulse;
    [SerializeField] private float lateralBoost;
    [SerializeField] private MonkeyHand[] monkeyHands;
    [SerializeField] Rigidbody2D playerRb;
    private bool isPlayerAttached;

    void FixedUpdate()
    {
        MoveSideWays();
        Jump();
    }

    private void Update()
    {
        for (int i = 0; i < monkeyHands.Length; i++)
        {
            if (monkeyHands[i].IsAttached)
            {
                isPlayerAttached = true;
                break; // Exit the loop early if at least one hand is attached
            }
        }
        Debug.Log($"Attached: {isPlayerAttached}, VelX: {playerRb.linearVelocityX}, JumpKey: {Input.GetButtonDown("Jump")}");
    }

    private void MoveSideWays()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 targetVelocity = new Vector2(moveInput * moveForce, playerRb.linearVelocityY);
        playerRb.linearVelocity = Vector2.Lerp(playerRb.linearVelocity, targetVelocity, Time.fixedDeltaTime * 10f);
        Debug.DrawLine(playerRb.position, playerRb.position + new Vector2(playerRb.linearVelocity.x, 0), Color.cyan);
    }

    private void Jump()
    {
        float horizontalVelocity = Mathf.Abs(playerRb.linearVelocityX);

        if (Input.GetButton("Jump") && isPlayerAttached)
        {

            foreach (MonkeyHand hand in monkeyHands)
            {
                if (hand.IsAttached)
                    hand.Detach();
            }
            // Detach all hands if any are attached
            isPlayerAttached = false; // Reset the attachment state

            Vector2 upwardForce = Vector2.up * jumpImpulse;

            float lateral = Mathf.Clamp(playerRb.linearVelocity.x, -1f, 1f) * lateralBoost;

            Vector2 totalJump = upwardForce + new Vector2(lateral, 0f);

            playerRb.AddForce(totalJump, ForceMode2D.Impulse);

            Debug.DrawLine(playerRb.position, playerRb.position + totalJump, Color.green, 1f);
        }
    }

    public void SetPosition(Vector3 position) { playerRb.MovePosition(position); }

  }
