using System.Diagnostics;
using UnityEngine;

public class CajaGranadas : MonoBehaviour
{
    // Velocidad de parpadeo (ajustable desde el inspector)
    public float velocidadParpadeo = 2f;

    // Rango de brillo (también ajustable si lo deseas)
    public float brilloMinimo = 0.5f;
    public float brilloMaximo = 1.5f;

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
        // Oscila el brillo del sprite usando la velocidad configurada
        if (spriteRenderer != null)
        {
            tiempo += Time.deltaTime;
            float brillo = Mathf.Lerp(brilloMinimo, brilloMaximo, (Mathf.Sin(tiempo * velocidadParpadeo) + 1f) / 2f);
            spriteRenderer.color = new Color(brillo, brillo, brillo, 1f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        InventarioMano inventario = other.GetComponent<InventarioMano>();

        if (inventario != null && !inventario.TieneGranada() && Input.GetKeyDown(KeyCode.F))
        {
            inventario.TomarGranada();
            UnityEngine.Debug.Log("Jugador tomó una granada");
        }
    }
}