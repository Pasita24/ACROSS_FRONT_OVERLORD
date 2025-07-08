using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadaEnemigo : MonoBehaviour
{
    public GameObject efectoExplosion; // Prefab del efecto de explosión
    public float daño = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aplicar daño al jugador
            MovimientoJugador jugador = other.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                jugador.TomarDaño(daño);
            }

            // Instanciar efecto de explosión
            if (efectoExplosion != null)
            {
                Instantiate(efectoExplosion, transform.position, Quaternion.identity);
            }

            // Destruir la granada
            Destroy(gameObject);
        }
        if (other.CompareTag("Ground"))
        {
            // Instanciar efecto de explosión
            if (efectoExplosion != null)
            {
                Instantiate(efectoExplosion, transform.position, Quaternion.identity);
            }

            // Destruir la granada
            Destroy(gameObject);
        }
    }
}

