using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldadoPatrullero : MonoBehaviour
{
    [SerializeField] private float vida;
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;

    private Vector3 destinoActual;
    private SpriteRenderer sr;
    private Animator animator;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        destinoActual = puntoB.position;
    }

    private void Update()
    {
        // Movimiento hacia el destino actual
        transform.position = Vector2.MoveTowards(transform.position, destinoActual, velocidad * Time.deltaTime);

        // Si llegó a un extremo, cambiar dirección
        if (Vector2.Distance(transform.position, destinoActual) < 0.1f)
        {
            destinoActual = destinoActual == puntoA.position ? puntoB.position : puntoA.position;
            sr.flipX = !sr.flipX;
        }
    }
}

