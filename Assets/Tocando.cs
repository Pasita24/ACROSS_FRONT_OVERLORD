using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tocando : MonoBehaviour
{
    [SerializeField] public MonoBehaviour punto;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            punto.enabled = true;
        }
    }
}
