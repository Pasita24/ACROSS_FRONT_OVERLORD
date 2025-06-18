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
        // Simula explosión tras un tiempo
        Invoke(nameof(Explode), explosionTime);
    }

    private void Explode()
    {
        // Aquí puedes agregar efectos de explosión, daño, etc.
        UnityEngine.Debug.Log("¡Granada explotó!");
        Destroy(gameObject);
    }
}
