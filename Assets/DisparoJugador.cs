using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisparoJugador : MonoBehaviour
{
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject bala;

    [Header("UI Recarga")]
    [SerializeField] private TextMeshProUGUI textoRecarga;

    [Header("Disparo")]
    [SerializeField] private int maxBalas = 5;
    [SerializeField] private float tiempoRecarga = 2f;

    private int balasActuales;
    private bool enRecarga;

    private void Start()
    {
        balasActuales = maxBalas;
        ActualizarTextoRecarga();
    }

    private void Update()
    {
        if (enRecarga) return;

        if (Input.GetButtonDown("Fire1") && !LanzadorDeGranadas.EstaApuntandoConGranada)
        {
            Disparar();
        }
    }

    private void Disparar()
    {
        if (balasActuales <= 0)
        {
            StartCoroutine(Recargar());
            return;
        }

        Instantiate(bala, controladorDisparo.position, controladorDisparo.rotation);
        balasActuales--;
        ActualizarTextoRecarga();

        if (balasActuales <= 0)
        {
            StartCoroutine(Recargar());
        }
    }

    private IEnumerator Recargar()
    {
        enRecarga = true;

        float tiempoRestante = tiempoRecarga;
        while (tiempoRestante > 0)
        {
            textoRecarga.text = "Recargando... " + tiempoRestante.ToString("F1") + "s";
            yield return new WaitForSeconds(0.1f);
            tiempoRestante -= 0.1f;
        }

        balasActuales = maxBalas;
        enRecarga = false;
        ActualizarTextoRecarga();
    }

    private void ActualizarTextoRecarga()
    {
        textoRecarga.text = "Balas: " + balasActuales + "/" + maxBalas;
    }
}
