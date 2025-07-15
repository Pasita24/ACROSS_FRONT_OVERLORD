using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonVertical : MonoBehaviour
{
    [Header("Disparo")]
    public Transform puntoDisparo;
    public GameObject balaPrefab;
    public float radioDeteccion = 10f;
    public float toleranciaX = 0.5f; // Tolerancia horizontal para alineación
    public float tiempoEntreDisparos = 2f;

    private float tiempoSiguienteDisparo = 0f;
    private Transform jugador;
    private Animator animator;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (jugador == null) return;

        float distanciaVertical = transform.position.y - jugador.position.y;
        float diferenciaX = Mathf.Abs(transform.position.x - jugador.position.x);

        bool jugadorDebajo = distanciaVertical > 0 && distanciaVertical <= radioDeteccion && diferenciaX <= toleranciaX;

        if (jugadorDebajo && Time.time >= tiempoSiguienteDisparo)
        {
            animator.SetTrigger("Disparar");
            tiempoSiguienteDisparo = Time.time + tiempoEntreDisparos;
        }
    }

    // Este método se llama desde la animación
    public void Disparar()
    {
        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
        bala.GetComponent<BalaCanon>()?.SetDireccion(Vector2.down);
    }
}

