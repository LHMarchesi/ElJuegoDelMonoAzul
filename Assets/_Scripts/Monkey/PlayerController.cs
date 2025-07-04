using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool enableInputs;
    public Rigidbody2D playerRb;
    [SerializeField] private float moveForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpUpBoost;
    [SerializeField] private float lateralBoost;
    [SerializeField] private float extraDownForce;
    [SerializeField] private MonkeyHand[] monkeyHands;
    private bool isPlayerAttached;

    void FixedUpdate()
    {
        if (enableInputs)
        {
            MoveSideWays();
            Jump();
            Vector2 downforce = Vector2.down * extraDownForce;
            playerRb.AddForce(downforce, ForceMode2D.Force);

        }
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
    }

    private void MoveSideWays()
    {
        // Movimiento lateral base
        float moveInput = Input.GetAxis("Horizontal");
        float horizontalSpeed = moveInput * moveForce;

        // impulso vertical proporcional al movimiento horizontal
        float upwardBoost = Mathf.Abs(moveInput) * jumpUpBoost;

        Vector2 targetVelocity = new Vector2(horizontalSpeed, playerRb.linearVelocityY + upwardBoost);

        playerRb.linearVelocity = Vector2.Lerp(playerRb.linearVelocity, targetVelocity, Time.fixedDeltaTime * 10f);

        // Debug
        Debug.DrawLine(playerRb.position, playerRb.position + new Vector2(playerRb.linearVelocity.x, 0), Color.cyan);
        Debug.DrawLine(playerRb.position, playerRb.position + new Vector2(0, upwardBoost), Color.red);
    }

    private void Jump()
    {
        float horizontalVelocity = Mathf.Abs(playerRb.linearVelocityX);

        if (Input.GetButton("Jump") && isPlayerAttached)
        {
            DetachMonkeyHands();

            Vector2 upwardForce = Vector2.up * jumpForce;
            float lateral = Mathf.Clamp(playerRb.linearVelocity.x, -1f, 1f) * lateralBoost;
            
            Vector2 totalJump = upwardForce + new Vector2(lateral, 0f);
            playerRb.AddForce(totalJump, ForceMode2D.Impulse);

            Debug.DrawLine(playerRb.position, playerRb.position + totalJump, Color.green, 1f); //Debug line for jump force
        }
    }

    public void DetachMonkeyHands()
    {
        foreach (MonkeyHand hand in monkeyHands)
        {
            if (hand.IsAttached)
                hand.Detach();
        }
        // Detach all hands if any are attached
        isPlayerAttached = false; // Reset the attachment state

    }

    public void SetPosition(Vector3 position) { playerRb.MovePosition(position); }
}
