
using UnityEngine;

public class SpringController : MonoBehaviour {
    [SerializeField] float jumpForce = 15f;

    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();

            if (rb != null) {
                // Reiniciamos la velocidad vertical
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

                // Aplicamos el rebote hacia arriba
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                // Reproducimos la animación del muelle
                animator.SetTrigger("HasToJump");
            }
        }
    }
}
