using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaEnemy : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private float daño;

    private Vector2 direccion = Vector2.left; // Por defecto a la izquierda

    public void SetDireccion(Vector2 nuevaDireccion)
    {
        direccion = nuevaDireccion.normalized;
    }

    private void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<MovimientoJugador>().TomarDaño(daño);
            Destroy(gameObject);
        }
    }
}

