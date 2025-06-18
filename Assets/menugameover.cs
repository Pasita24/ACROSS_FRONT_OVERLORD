using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menugameover : MonoBehaviour
{
    [SerializeField] private GameObject menuGameOver;
    private MovimientoJugador movimientoJugador;

    private void Start()
    {
        movimientoJugador = GameObject.FindGameObjectWithTag("Player").GetComponent<MovimientoJugador>();
        movimientoJugador.MuerteJugador += ActivarMenu;
    }

    private void ActivarMenu(object sender, EventArgs e)
    {
        menuGameOver.SetActive(true); 
    }
    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
