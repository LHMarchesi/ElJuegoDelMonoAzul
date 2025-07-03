using System;
using UnityEngine;

public class MonkeyHand : MonoBehaviour
{
    public static MonkeyHand CurrentlyAttachedHand { get; private set; }

    [SerializeField] private float attractionForce;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float attachCooldownDuration; // o el valor que prefieras
    [SerializeField] private LayerMask platformMask;
    [SerializeField] private bool DrawGizmos;

    private Rigidbody2D handRb;
    private Transform targetPlatform;
    private Vector2 attachPosition;
    private float attachCooldownTimer = 0f;

    public bool IsAttached => isAttached;
    private bool isAttached;

    private void Awake()
    {
        handRb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (attachCooldownTimer > 0)
            attachCooldownTimer -= Time.fixedDeltaTime;

        if (isAttached)
        {
            handRb.linearVelocity = Vector2.zero;
            handRb.position = attachPosition;
        }
        else if (attachCooldownTimer <= 0f)
        {
            // Atracción magnética (antes de pegarse)
            Collider2D platform = Physics2D.OverlapCircle(transform.position, detectionRadius, platformMask);
            if (platform && MonkeyHand.CurrentlyAttachedHand == null)
            {
                Vector2 dir = ((Vector2)platform.transform.position - (Vector2)transform.position).normalized;
                handRb.AddForce(dir * attractionForce,ForceMode2D.Impulse);
            }
        }


    }
    private void Update()
    {
        if (attachCooldownTimer > 0)
            attachCooldownTimer -= Time.deltaTime;
    }
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttached || attachCooldownTimer > 0f || CurrentlyAttachedHand != null)
            return;

        if (((1 << other.gameObject.layer) & platformMask) != 0)
        {
            isAttached = true;
            attachPosition = transform.position;
            CurrentlyAttachedHand = this;
        }
    }

    public void Detach()
    {
        isAttached = false;
        attachCooldownTimer = attachCooldownDuration;

        if (CurrentlyAttachedHand == this)
            CurrentlyAttachedHand = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (DrawGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            // Si hay atracción, dibujá una flecha hacia la plataforma
            if (!Application.isPlaying || isAttached || targetPlatform == null)
                return;

            Vector3 direction = (targetPlatform.position - transform.position).normalized;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, transform.position + direction * detectionRadius);
        }
    }
}
