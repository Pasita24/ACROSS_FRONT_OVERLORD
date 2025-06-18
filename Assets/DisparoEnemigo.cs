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
    Vector2 direccionDisparo = new Vector2(Mathf.Sign(transform.localScale.x), 0f);
    jugadorEnRango = Physics2D.Raycast(controladorDisparo.position, direccionDisparo, distanciaLinea, capaJugador);

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
    GameObject bala = Instantiate(balaEnemigo, controladorDisparo.position, Quaternion.identity);

    // Obtener la dirección dependiendo de flipX
    bool flipX = GetComponent<SpriteRenderer>().flipX;
    Vector2 direccion = flipX ? Vector2.right : Vector2.left;

    bala.GetComponent<BalaEnemy>().SetDireccion(direccion);
}



    private void OnDrawGizmos()
    {
        Vector2 direccionDisparo = new Vector2(Mathf.Sign(transform.localScale.x), 0f);
        Gizmos.DrawLine(controladorDisparo.position, controladorDisparo.position + (Vector3)(direccionDisparo * distanciaLinea));
    }
}
