using UnityEngine;
using UnityEngine.Events;

public class Granada : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float _explosionTime = 3f;
    [SerializeField] private UnityEvent _onTriggerEnterEvent;
    [SerializeField] private UnityEvent _onExplodeEvent;
    [SerializeField] private GameObject _explosionPrefab;

    [Header("Explosión")]
    [SerializeField] private float _radioExplosion = 3f; // Radio del área de daño
    [SerializeField] private float _dañoFijo = 1f; // Daño constante al jugador
    [SerializeField] private LayerMask _layerJugador; // Capa del jugador

    [Header("Debug")]
    [SerializeField] private float _currentTimer;
    [SerializeField] private bool _isFrozen;

    private bool _isBeingDestroyed = false;

    private void Start()
    {
        _currentTimer = _explosionTime;
        _isFrozen = false;
        _isBeingDestroyed = false;
    }

    private void Update()
    {
        if (_isFrozen) return;

        _currentTimer -= Time.deltaTime;

        if (_currentTimer <= 0f)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Balcon"))
        {
            _onTriggerEnterEvent?.Invoke();
            FreezeTimer();
        }
    }

    private void Explode()
    {
        if (_isBeingDestroyed) return;
        _isBeingDestroyed = true;

        // Instanciar la explosión
        if (_explosionPrefab != null)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        }

        // Aplicar daño fijo en área
        AplicarDañoFijo();

        _onExplodeEvent?.Invoke();
        Destroy(gameObject);
    }

    private void AplicarDañoFijo()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radioExplosion, _layerJugador);

        foreach (Collider2D collider in colliders)
        {
            MovimientoJugador jugador = collider.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                jugador.TomarDaño(_dañoFijo); // Daño constante
            }
        }
    }

    public void FreezeTimer() => _isFrozen = true;
    public void UnfreezeTimer() => _isFrozen = false;

    public void ForceExplosion()
    {
        if (_isBeingDestroyed) return;
        _currentTimer = 0f;
        _isFrozen = false;
        Explode();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radioExplosion);
    }
}