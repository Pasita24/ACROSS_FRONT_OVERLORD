using System.Diagnostics;
using UnityEngine;

public class CajaGranadas : MonoBehaviour
{
    private bool granadaDisponible = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!granadaDisponible) return;

        InventarioMano inventario = other.GetComponent<InventarioMano>();

        if (inventario != null && !inventario.TieneGranada())
        {
            inventario.TomarGranada();
            granadaDisponible = false;
            UnityEngine.Debug.Log("Jugador tomó una granada");
            Destroy(gameObject); // Elimina la caja o puedes reemplazarla por una caja vacía
        }
    }
}
