
using UnityEngine;

public class RopeController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D playerRb;
    private Collider2D ropeCollider;

    private float originalGravity;
    private RigidbodyConstraints2D originalConstraints;

    private bool isAttached = false;
    private bool canAttach = true;
    private float horizontal;

    void Start() {
        ropeCollider = GetComponent<Collider2D>();
    }

    void Update() {
        if (!isAttached) return;

        // Soltar la cuerda al pulsar Espacio
        if (Input.GetKeyDown(KeyCode.Space)) {
            DetachPlayer();
            return;
        }

        // Movimiento izquierda y derecha
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate() {
        if (!isAttached || playerRb == null) return;

        float newX = playerRb.position.x +
                     horizontal * moveSpeed * Time.fixedDeltaTime;

        // Límites de la cuerda
        float minX = ropeCollider.bounds.min.x;
        float maxX = ropeCollider.bounds.max.x;

        // Evitar salir de la cuerda
        newX = Mathf.Clamp(newX, minX, maxX);

        // Mover al jugador horizontalmente
        playerRb.MovePosition(new Vector2(
            newX,
            playerRb.position.y
        ));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && canAttach) {
            AttachPlayer(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            canAttach = true;
        }
    }

    void AttachPlayer(Collider2D player) {
        if (isAttached) return;

        playerRb = player.attachedRigidbody;

        if (playerRb == null) return;

        // Guardar física original
        originalGravity = playerRb.gravityScale;
        originalConstraints = playerRb.constraints;

        // Detener al jugador
        playerRb.linearVelocity = Vector2.zero;
        playerRb.gravityScale = 0f;

        // Bloquear movimiento vertical
        playerRb.constraints =
            RigidbodyConstraints2D.FreezePositionY |
            RigidbodyConstraints2D.FreezeRotation;

        horizontal = 0f;
        isAttached = true;
    }

    void DetachPlayer() {
        if (playerRb == null) return;

        // Restaurar física del jugador
        playerRb.constraints = originalConstraints;
        playerRb.gravityScale = originalGravity;

        // Evitar que se enganche inmediatamente
        canAttach = false;
        isAttached = false;
        horizontal = 0f;

        playerRb = null;
    }
}
