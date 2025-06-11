using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiempodeVida : MonoBehaviour
{
    [SerializeField] private float tiempodeVida;
    private void Start() {
        Destroy(gameObject, tiempodeVida);
    }
}
