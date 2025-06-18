using UnityEngine;
using UnityEngine.Events;

public class Granada : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float _explosionTime = 3f; // Tiempo base antes de explotar
    [SerializeField] private UnityEvent _onTriggerEnterEvent; // Evento al tocar el balcón
    [SerializeField] private UnityEvent _onExplodeEvent; // Evento al explotar

    [Header("Debug")]
    [SerializeField] private float _currentTimer; // Tiempo restante visible en el Inspector
    [SerializeField] private bool _isFrozen; // Estado de congelación

    private bool _isBeingDestroyed = false;

    private void Start()
    {
        _currentTimer = _explosionTime; // Inicializa el timer
    }

    private void Update()
    {
        if (_isFrozen) return; // Si está congelada, no cuenta tiempo

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
            _onTriggerEnterEvent?.Invoke(); // Activa el evento (para iniciar diálogo)
        }
    }

    private void Explode()
    {
        if (_isBeingDestroyed) return;
        _isBeingDestroyed = true;
        _onExplodeEvent?.Invoke();
        Destroy(gameObject);
    }

    // Congela el tiempo (llamar desde Unity Event)
    public void FreezeTimer()
    {
        _isFrozen = true; // Detiene el timer en Update()
    }

    // Descongela el tiempo (llamar desde Unity Event)
    public void UnfreezeTimer()
    {
        _isFrozen = false; // Reanuda el timer en Update()
    }
}