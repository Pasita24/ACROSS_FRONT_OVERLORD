using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruir : MonoBehaviour
{
    public float tiempo = 1f; // Igual a la duración de la animación

    void Start()
    {
        Destroy(gameObject, tiempo);
    }
}
