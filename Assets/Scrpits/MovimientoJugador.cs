using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MovimientoJugador : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Vector2 input;

    [Header("UI Vida")]
    [SerializeField] private TextMeshProUGUI textoVida;

    [Header("Vida")]
    [SerializeField] public float vida;
    [SerializeField] private GameObject efectoMuerte;

    [Header("Movimiento")]
    private float movimientoHorizontal = 0f;
    [SerializeField] private float velocidadDeMovimiento;
    [Range(0, 0.3f)][SerializeField] private float suavizadoDeMovimiento;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;
    private bool movimientoPausado = false;

    [Header("Salto")]
    [SerializeField] private float fuerzaDeSalto;
    [SerializeField] private LayerMask queEsSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionesCaja;
    [SerializeField] private bool enSuelo;
    private bool salto = false;
    [Header("Escaleras")]
    [SerializeField] private float velocidadEscalera = 3f;
    private bool enEscalera = false;

    [Header("Animación")]
    private Animator animator;

    [Header("UI Dash")]
    [SerializeField] private TextMeshProUGUI textoDashUI;

    [Header("Dash")]
    [SerializeField] private float fuerzaDash = 15f;
    [SerializeField] private float duracionDash = 0.2f;
    [SerializeField] private float tiempoEntreDashes = 1f;
    private bool puedeHacerDash = true;
    private bool estaHaciendoDash = false;
    private float tiempoDashRestante = 0f;
    private float direccionDash = 1f;
    private bool estaMuerto = false;

    private BoxCollider2D boxCollider;
    private Vector2 colliderOriginalSize;
    private Vector2 colliderOriginalOffset;
    [SerializeField] private Vector2 colliderDashSize = new Vector2(1.5f, 0.5f);
    [SerializeField] private Vector2 colliderDashOffset = new Vector2(0f, -0.25f);

    [SerializeField] private GameOverManager gameOverManager;

    public event EventHandler MuerteJugador;
    private InventarioMano inventarioMano;


    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ActualizarTextoVida();
        boxCollider = GetComponent<BoxCollider2D>();
        colliderOriginalSize = boxCollider.size;
        colliderOriginalOffset = boxCollider.offset;
        inventarioMano = GetComponent<InventarioMano>();
        textoDashUI.text = "Dash Listo";

    }

    private void Update()
    {
        if (movimientoPausado) return;

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        movimientoHorizontal = input.x * velocidadDeMovimiento;
        animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));

        if (Input.GetButtonDown("Jump"))
        {
            salto = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && puedeHacerDash && !estaHaciendoDash)
        {
            IniciarDash();
        }
        if (Input.GetKeyDown(KeyCode.F) && inventarioMano != null && inventarioMano.objetoActual == InventarioMano.ObjetoEnMano.Ladrillo)
        {
            AtacarConLadrillo();
        }

    }

    private void FixedUpdate()
    {
        if (movimientoPausado) return;

        // Dash activo
        if (estaHaciendoDash)
        {
            rb2D.velocity = new Vector2(direccionDash * fuerzaDash, 0f);
            tiempoDashRestante -= Time.fixedDeltaTime;

            if (tiempoDashRestante <= 0f)
            {
                estaHaciendoDash = false;
            }

            return; // Ignora movimiento normal mientras dura el dash
        }

        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);
        if (enEscalera)
        {
            rb2D.gravityScale = 0;
            rb2D.velocity = new Vector2(rb2D.velocity.x, input.y * velocidadEscalera);
        }
        else
        {
            rb2D.gravityScale = 3; // O el valor original
        }
        Mover(movimientoHorizontal * Time.fixedDeltaTime, salto);
        salto = false;
        
        if (tiempoDashRestante <= 0f)
        {
            estaHaciendoDash = false;
            TerminarDash();
        }
    }

    private void Mover(float mover, bool saltar)
    {
        Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);

        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }

        if (enSuelo && saltar)
        {
            enSuelo = false;
            rb2D.AddForce(new Vector2(0f, fuerzaDeSalto));
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void IniciarDash()
    {
        estaHaciendoDash = true;
        puedeHacerDash = false;
        tiempoDashRestante = duracionDash;

        direccionDash = mirandoDerecha ? 1f : -1f;

        animator.SetTrigger("Dash");

        // Ajustar tamaño del collider
        boxCollider.size = colliderDashSize;

        // Ajustar offset para que la base del collider quede igual
        float deltaOffsetY = (colliderOriginalSize.y - colliderDashSize.y) / 2f;
        boxCollider.offset = new Vector2(colliderOriginalOffset.x, colliderOriginalOffset.y - deltaOffsetY);

        StartCoroutine(CooldownDash());
        textoDashUI.text = "Reiniciando Dash...";
    }

    private void TerminarDash()
    {
        boxCollider.size = colliderOriginalSize;
        boxCollider.offset = colliderOriginalOffset;
    }

    private IEnumerator CooldownDash()
    {
        float tiempoRestante = tiempoEntreDashes;
        while (tiempoRestante > 0)
        {
            textoDashUI.text = "Reiniciando..." + tiempoRestante.ToString("F1") + "s";
            yield return new WaitForSeconds(0.1f);
            tiempoRestante -= 0.1f;
        }

        puedeHacerDash = true;
        textoDashUI.text = "Dash Listo";
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }

    public void TomarDaño(float daño)
    {
        if (estaMuerto) return; 

        vida -= daño;
        ActualizarTextoVida();

        if (vida <= 0)
        {
            estaMuerto = true;
            MuerteJugador?.Invoke(this, EventArgs.Empty);
            Muerte();
        }
    }

    private void ActualizarTextoVida()
    {
        textoVida.text = "Vidas: " + Mathf.Max(vida, 0).ToString("0");
    }

    private void Muerte()
    {
        Instantiate(efectoMuerte, transform.position, Quaternion.identity);
        gameOverManager.ActivarGameOver(); // Llamar al Game Over
        Destroy(gameObject);
    }
    public void HabilitarJugador()
    {
        movimientoPausado = false;
    }


    public void PausarMovimiento()
    {
        movimientoPausado = true;
        rb2D.velocity = new Vector2(0f, rb2D.velocity.y);
    }
    private void AtacarConLadrillo()
    {
        Vector2 direccionAtaque = mirandoDerecha ? Vector2.right : Vector2.left;
        Vector2 origen = (Vector2)transform.position + direccionAtaque * 0.8f; // Un poco al frente del jugador
        float radio = 2f;

        Collider2D hit = Physics2D.OverlapCircle(origen, radio, LayerMask.GetMask("Enemy"));

        if (hit != null)
        {
            BossVida bossVida = hit.GetComponent<BossVida>();
            if (bossVida != null)
            {
                bossVida.TomarDaño(1); // Daño arbitrario
                UnityEngine.Debug.Log("¡Boss golpeado con ladrillo!");
                inventarioMano.UsarObjeto(); // Solo se usa si daña al boss
            }
            else
            {
                UnityEngine.Debug.Log("Golpeó a algo que no es el boss.");
            }
        }
        else
        {
            UnityEngine.Debug.Log("Golpeó al aire.");
        }

        // Visualización opcional
        UnityEngine.Debug.DrawLine(transform.position, origen, Color.red, 0.5f);
    }
    private void OnDrawGizmosSelected()
    {
        if (inventarioMano != null && inventarioMano.objetoActual == InventarioMano.ObjetoEnMano.Ladrillo)
        {
            Vector2 direccion = mirandoDerecha ? Vector2.right : Vector2.left;
            Vector2 origen = (Vector2)transform.position + direccion * 0.8f;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(origen, 0.6f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Escaleras"))
        {
            enEscalera = true;
        }

        if (other.CompareTag("Gun"))
        {
            //Debug.Log("Pistola recogida. Cambiando de nivel...");
            other.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (other.CompareTag("Ladrillo"))
        {
            if (inventarioMano.objetoActual == InventarioMano.ObjetoEnMano.Nada)
            {
                inventarioMano.TomarLadrillo();
                other.gameObject.SetActive(false); // Solo desaparece si fue tomado
            }
            else
            {
                UnityEngine.Debug.Log("Ya tienes un objeto en la mano. No puedes tomar otro ladrillo.");
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Escaleras"))
        {
            enEscalera = false;
        }
    }
}
