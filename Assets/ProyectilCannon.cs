using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilCanon : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float daño = 1f;
    [SerializeField] private float tiempoVida = 5f;

    private Vector2 direccion;

    public void EstablecerDireccion(Vector2 dir)
    {
        direccion = dir.normalized;
        Destroy(gameObject, tiempoVida); // Destruir después de X segundos
    }

    private void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<MovimientoJugador>()?.TomarDaño(daño);
            Destroy(gameObject);
        }
    }
}

