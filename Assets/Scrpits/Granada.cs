using UnityEngine;
using UnityEngine.Events;

public class Granada : MonoBehaviour
{
    [Header("Configuraci�n")]
    [SerializeField] private float _explosionTime = 3f;
    [SerializeField] private UnityEvent _onTriggerEnterEvent;
    [SerializeField] private UnityEvent _onExplodeEvent;
    [SerializeField] private GameObject _explosionPrefab;

    [Header("Explosi�n")]
    [SerializeField] private float _radioExplosion = 3f; // Radio del �rea de da�o
    [SerializeField] private float _da�oFijo = 1f; // Da�o constante al jugador
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

        // Instanciar la explosi�n
        if (_explosionPrefab != null)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        }

        // Aplicar da�o fijo en �rea
        AplicarDa�oFijo();

        _onExplodeEvent?.Invoke();
        Destroy(gameObject);
    }

    private void AplicarDa�oFijo()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radioExplosion, _layerJugador);

        foreach (Collider2D collider in colliders)
        {
            MovimientoJugador jugador = collider.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                jugador.TomarDa�o(_da�oFijo); // Da�o constante
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