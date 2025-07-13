using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DañoContinuo : MonoBehaviour
{
    [SerializeField] private float daño = 1f;
    [SerializeField] private float intervalo = 2f; // Cada 2 segundos
    private bool jugadorDentro = false;
    private MovimientoJugador jugador;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugador = other.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                jugadorDentro = true;
                StartCoroutine(InfligirDaño());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
            StopAllCoroutines();
        }
    }

    private IEnumerator InfligirDaño()
    {
        while (jugadorDentro)
        {
            jugador.TomarDaño(daño);
            yield return new WaitForSeconds(intervalo);
        }
    }
}

