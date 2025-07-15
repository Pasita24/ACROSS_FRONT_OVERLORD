using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tocando : MonoBehaviour
{
    [SerializeField] private MonoBehaviour controlador;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            controlador.enabled = false;
        }
    }
}