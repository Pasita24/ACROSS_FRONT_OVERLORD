using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoEnemigo : MonoBehaviour
{
    private EnemyShoot enemyShoot;
    public Transform controladorDisparo;
    public float distanciaLinea;
    public LayerMask capaJugador;
    public bool jugadorEnRango;
    public float tiempoEntreDisparos;
    public float tiempoUltimoDisparo;
    public GameObject balaEnemigo;
    public float tiempoEsperaDisparo;
    public Animator animator;

    private void Start() {
        enemyShoot = GetComponent<EnemyShoot>();
    }
    private void Update()
{
    jugadorEnRango = Physics2D.Raycast(controladorDisparo.position, transform.right, distanciaLinea, capaJugador);

    if (enemyShoot != null)
    {
        enemyShoot.SetCanMove(!jugadorEnRango); // Detener movimiento si jugador está en rango
    }

    if (jugadorEnRango)
    {
        if (Time.time > tiempoEntreDisparos + tiempoUltimoDisparo)
        {
            tiempoUltimoDisparo = Time.time;
            animator.SetTrigger("Disparar");
            Invoke(nameof(Disparar), tiempoEsperaDisparo);
        }
    }
}
    private void Disparar()
    {
        Instantiate(balaEnemigo, controladorDisparo.position, controladorDisparo.rotation);   
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controladorDisparo.position, controladorDisparo.position + transform.right * distanciaLinea);
    }
}
