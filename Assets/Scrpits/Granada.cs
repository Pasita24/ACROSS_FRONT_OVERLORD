using System.Diagnostics;
using UnityEngine;

public class Granada : MonoBehaviour
{
    [SerializeField] private float explosionTime = 3f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Simula explosi�n tras un tiempo
        Invoke(nameof(Explode), explosionTime);
    }

    private void Explode()
    {
        // Aqu� puedes agregar efectos de explosi�n, da�o, etc.
        UnityEngine.Debug.Log("�Granada explot�!");
        Destroy(gameObject);
    }
}
