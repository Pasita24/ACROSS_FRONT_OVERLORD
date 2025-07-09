using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonEnemigo : MonoBehaviour
{
    [Header("Detección del Jugador")]
    [SerializeField] private float radioDeteccion = 8f;
    [SerializeField] private LayerMask capaJugador;

    [Header("Disparo")]
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float tiempoEntreDisparos = 2f;
    private float temporizador;

    private Transform jugador;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
        temporizador = tiempoEntreDisparos;
    }

    private void Update()
    {
        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia <= radioDeteccion)
        {
            temporizador -= Time.deltaTime;

            if (temporizador <= 0f)
            {
                Disparar();
                temporizador = tiempoEntreDisparos;
            }
        }
    }

    private void Disparar()
    {
        // Apuntar hacia el jugador
        Vector2 direccion = (jugador.position - puntoDisparo.position).normalized;

        // Instanciar proyectil
        GameObject bala = Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);
        
        // Enviar dirección al proyectil
        bala.GetComponent<ProyectilCanon>().EstablecerDireccion(direccion);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}

