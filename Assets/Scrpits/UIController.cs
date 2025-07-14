using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private GameObject MenuGameover;
    private MovimientoJugador Jugador;

    private void Start()
    {
        Jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<MovimientoJugador>();
        Jugador.MuerteJugador += ActivarMenu;
    }

    private void ActivarMenu(object sender, EventArgs e)
    {
        MenuGameover.SetActive(true);
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SalirAmenu(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }
}