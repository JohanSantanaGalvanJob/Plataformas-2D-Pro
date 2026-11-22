using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float motionVelocity = 1f;

    [SerializeField] float jumpForce = 1f;

    [SerializeField] float lengthLine = 0f;
    bool jump = true;

    bool activeMap = false;

    public int coinsCollected = 0;

    private int totalCoins = 0;

    private int hitsLeft = 3;

    public TextMeshProUGUI txtCoins;
    public TextMeshProUGUI txtHits;
    [SerializeField] ParticleSystem jumpParticles;
    [SerializeField] float offsetRaycast = 0f;
    private bool mushroomActivated = false;
    public Sprite newSprite;

    [SerializeField] private GameObject[] mushrooms;

    private void Start() {
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        updateCoinText();
        updateHitText();
    }
    private void Update() {
        float xMotion = Input.GetAxisRaw("Horizontal");
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (xMotion < 0) sr.flipX = true;
        else if (xMotion > 0) sr.flipX = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 speedVector = new Vector2(xMotion * motionVelocity, rb.linearVelocityY);
        GetComponent<Rigidbody2D>().linearVelocity = speedVector;

        if (Input.GetKeyDown(KeyCode.Space) && jump) {
            jump = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpParticles.Play();
            AudioManager.instance.PlaySFX("Jump");
        }

        if (Input.GetKeyDown(KeyCode.M) && !activeMap) {
            SceneManager.LoadScene("MapScene", LoadSceneMode.Additive);
            activeMap = true;
        } else if (Input.GetKeyDown(KeyCode.M) && activeMap) {
            SceneManager.UnloadSceneAsync("MapScene");
            activeMap = false;
        }

        // Obtenemos el tamaño vertical del collider para configurar la coordenada de generación
        float colliderSizeY = gameObject.GetComponent<Collider2D>().bounds.size.y + offsetRaycast;
        Vector2 raycastOrigin = new Vector2(gameObject.transform.position.x, transform.position.y - (colliderSizeY / 2));
        // Dibujamos una línea de color negro hacia abajo que parta desde la posición calculada anteriormente
        // y que termine en donde indique el segundo parámetro del método DrawLine()
        Debug.DrawLine(raycastOrigin, raycastOrigin - (new Vector2(0f, lengthLine)), Color.black);

        RaycastHit2D raycastHit2D = Physics2D.Raycast(raycastOrigin, Vector2.down, lengthLine);
        // Preguntamos si el RayCast está colisionando con el Tilemap de colisiones
        // asegurándonos de que dicho Tilemap tiene una etiqueta correctamente configurada
        if (raycastHit2D.collider != null) {
            if (GetComponent<Rigidbody2D>().linearVelocityX != 0) SetAnimation("running");
            else SetAnimation("idle"); // Si está sobre una superficie pero no se mueve
            if (raycastHit2D.collider.gameObject.CompareTag("Ground")) {
                jump = true;
            }
            jump = true;
        } else {
            jump = false;
            SetAnimation("jumping");
        }

        if(hitsLeft == 0) {
            SCManager.instance.LoadScene("DieScene");
        }
    }

    void SetAnimation(string name) {

        // Obtenemos todos los parámetros del Animator
        AnimatorControllerParameter[] parametros = GetComponent<Animator>().parameters;

        // Recorremos todos los parámetros y los ponemos a false
        foreach (var item in parametros) GetComponent<Animator>().SetBool(item.name, false);

        // Activamos el pasado por parámetro
        GetComponent<Animator>().SetBool(name, true);

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision != null) {
            if (collision.collider.CompareTag("Kill")) {
                AudioManager.instance.PlaySFX("Lose");
                SCManager.instance.LoadScene("DieScene");
            }

            if (collision.collider.CompareTag("Damage")) {
                AudioManager.instance.PlaySFX("Hit");
                hitsLeft--;
                updateHitText();
            }

            if (collision.collider.CompareTag("ActivationBlock") && mushroomActivated == false) {
                mushroomActivated = true;
                GameObject activationBlockObject = GameObject.FindGameObjectWithTag("ActivationBlock");
                SpriteRenderer sprite = activationBlockObject.GetComponent<SpriteRenderer>();
                sprite.sprite = newSprite;
                foreach (GameObject mushroom in mushrooms) {
                    mushroom.SetActive(true);
                }

            }

            if (collision.collider.CompareTag("Win")) {
                SCManager.instance.LoadScene("VictoryScene");

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision != null) {

            if (collision.CompareTag("Coin")) {
                AudioManager.instance.PlaySFX("CollectCoin");
                coinsCollected++;
                updateCoinText();
                Destroy(collision.gameObject);
            }

            if (collision.CompareTag("Key")) {
                AudioManager.instance.PlaySFX("CollectKey");
                GameObject keyBlockObject = GameObject.FindGameObjectWithTag("KeyBlock");
                Destroy(keyBlockObject);
                Destroy(collision.gameObject);

            }

            if (collision.CompareTag("Heart") && hitsLeft < 3) {
                AudioManager.instance.PlaySFX("Heart");
                hitsLeft++;
                updateHitText();
                Destroy(collision.gameObject);
            }

        }
    }

    private void updateCoinText() {
        txtCoins.text = coinsCollected.ToString() + "/" + totalCoins.ToString() + " Coins";
    }

    private void updateHitText() {
        txtHits.text = hitsLeft.ToString() + " Hits";
    }

}