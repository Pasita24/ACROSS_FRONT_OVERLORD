using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ControladorEnemigos2 : MonoBehaviour
{
    private float minX, maxX, minY, maxY;
    [SerializeField] private Transform[] puntos;
    [SerializeField] private GameObject[] enemigos;
    [SerializeField] private float tiempoEnemigos;
    private float tiempoSiguienteEnemigo;

    [Header("Texto UI")]
    [SerializeField] private TextMeshProUGUI textoAlInicio;
    [SerializeField] private float duracionTexto = 5f;

    private void Start()
    {
        maxX = puntos.Max(punto => punto.position.x);
        minX = puntos.Min(punto => punto.position.x);
        maxY = puntos.Max(punto => punto.position.y);
        minY = puntos.Min(punto => punto.position.y);

        // Mostrar texto y ocultarlo después de X segundos
        if (textoAlInicio != null)
        {
            textoAlInicio.gameObject.SetActive(true);
            Invoke(nameof(OcultarTexto), duracionTexto);
        }
    }

    private void OcultarTexto()
    {
        if (textoAlInicio != null)
            textoAlInicio.gameObject.SetActive(false);
    }

    private void Update()
    {
        tiempoSiguienteEnemigo += Time.deltaTime;
        if (tiempoSiguienteEnemigo >= tiempoEnemigos)
        {
            tiempoSiguienteEnemigo = 0;
            CrearEnemigo();
        }
    }

    private void CrearEnemigo()
    {
        int numeroEnemigo = Random.Range(0, enemigos.Length);
        Vector2 posicionAleatoria = new(Random.Range(minX, maxX), Random.Range(minY, maxY));

        Instantiate(enemigos[numeroEnemigo], posicionAleatoria, Quaternion.identity);
    }
}
