using System.Collections;
using UnityEngine;

public class CanonVerticalAutomatico : MonoBehaviour
{
    [Header("Disparo")]
    public Transform puntoDisparo;
    public GameObject balaPrefab;
    public float tiempoEntreDisparos = 1f;     // Tiempo entre cada disparo
    public float tiempoEsperaExtra = 1f;       // Pausa después del disparo

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(CicloDisparo());
    }

    private IEnumerator CicloDisparo()
    {
        while (true)
        {
            // Dispara una sola bala
            animator.SetTrigger("Disparar"); // Opcional
            Disparar();

            // Espera el tiempo entre disparos
            yield return new WaitForSeconds(tiempoEntreDisparos);

            // Espera tiempo adicional antes de disparar otra vez
            yield return new WaitForSeconds(tiempoEsperaExtra);
        }
    }

    private void Disparar()
    {
        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
        bala.GetComponent<BalaCanon>()?.SetDireccion(Vector2.down);
    }
}
