using UnityEngine;

public class BalaCanon : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float daño = 1f;

    private void Update()
    {
        transform.Translate(Vector2.right * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<MovimientoJugador>()?.TomarDaño(daño);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
