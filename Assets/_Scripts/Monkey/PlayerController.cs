using System;
using System.Collections.Generic;
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
    [SerializeField] private float airControl;
    [SerializeField] private MonkeyHand[] monkeyHands;
    private bool isPlayerAttached;

    public Action OnWin;
    [SerializeField] private List<AudioClip> JumpSounds;

    private void Awake()
    {
        OnWin += Desactivate;
    }

    void FixedUpdate()
    {
        if (enableInputs)
        {
            MoveSideWays();
            Jump();

            if (playerRb.linearVelocityY < 0 && !isPlayerAttached)
            {
                Vector2 downforce = Vector2.down * extraDownForce;
                playerRb.AddForce(downforce, ForceMode2D.Force);

                // Debug para visualizar la fuerza de caída
                Debug.DrawLine(playerRb.position, playerRb.position + downforce * 0.1f, Color.blue);
            }
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

        float moveInput = Input.GetAxis("Horizontal");

        if (isPlayerAttached)
        {
            float horizontalSpeed = moveInput * moveForce;
            float upwardBoost = Mathf.Abs(moveInput) * jumpUpBoost;

            Vector2 targetVelocity = new Vector2(horizontalSpeed, playerRb.linearVelocityY + upwardBoost);
            playerRb.linearVelocity = Vector2.Lerp(playerRb.linearVelocity, targetVelocity, Time.fixedDeltaTime * 10f);

            Debug.DrawLine(playerRb.position, playerRb.position + new Vector2(playerRb.linearVelocity.x, 0), Color.cyan);
            Debug.DrawLine(playerRb.position, playerRb.position + new Vector2(0, upwardBoost), Color.red);
        }
        else
        {
            // Control en el aire limitado: solo aplicar fuerza si se está moviendo
            float airControlForce = moveInput * moveForce * airControl;
            playerRb.AddForce(Vector2.right * airControlForce, ForceMode2D.Force);
        }
    }

    private void Jump()
    {
        float horizontalVelocity = Mathf.Abs(playerRb.linearVelocityX);

        if (Input.GetButton("Jump") && isPlayerAttached)
        {

            SoundManager.Instance.PlaySFX(JumpSounds[UnityEngine.Random.Range(0, JumpSounds.Count)]);
            DetachMonkeyHands();

            // Impulso vertical constante
            Vector2 upwardForce = Vector2.up * jumpForce;

            // Impulso lateral basado en velocidad horizontal 
            float lateralMomentum = Mathf.Clamp(playerRb.linearVelocityX, -1f, 1f); // no depende de magnitud
            Vector2 lateralForce = Vector2.right * lateralMomentum * lateralBoost;

            // Combinar sin que el lateral domine
            Vector2 totalJump = upwardForce + lateralForce;
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

    private void Desactivate()
    {
        DetachMonkeyHands();
        enableInputs = false;
        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 position) { playerRb.MovePosition(position); }

    private void OnDisable()
    {
        OnWin -= Desactivate; 
    }
}
