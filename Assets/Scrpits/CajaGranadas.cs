using System.Diagnostics;
using UnityEngine;

public class CajaGranadas : MonoBehaviour
{
    //private bool granadaDisponible = true;
    private SpriteRenderer spriteRenderer;
    private float tiempo;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            UnityEngine.Debug.LogError("CajaGranadas necesita un SpriteRenderer.");
        }
    }

    private void Update()
    {
        // Oscila el brillo del sprite
        if (spriteRenderer != null)
        {
            tiempo += Time.deltaTime;
            float brillo = Mathf.Lerp(0.5f, 1.5f, (Mathf.Sin(tiempo * 2f) + 1f) / 2f);
            spriteRenderer.color = new Color(brillo, brillo, brillo, 1f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        InventarioMano inventario = other.GetComponent<InventarioMano>();

        if (inventario != null && !inventario.TieneGranada() && Input.GetKeyDown(KeyCode.E))
        {
            inventario.TomarGranada();
            UnityEngine.Debug.Log("Jugador tomó una granada");
            // Ya no destruimos el objeto ni lo deshabilitamos
        }
    }
}
