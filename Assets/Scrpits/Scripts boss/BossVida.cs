using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System.Collections; 


public class BossVida : MonoBehaviour
{
    [Header("Vida del Boss")]
    [SerializeField] private float vidaMaxima = 5f;
    [SerializeField] private Slider sliderVidaBoss;
    [SerializeField] private GameObject objetoActivarAlMorir;

    private SpriteRenderer spriteRenderer;
    private Color colorOriginal;


    private float vidaActual;

    private void Start()
    {
        vidaActual = vidaMaxima;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }

        if (sliderVidaBoss != null)
        {
            sliderVidaBoss.maxValue = vidaMaxima;
            sliderVidaBoss.value = vidaActual;
            sliderVidaBoss.gameObject.SetActive(true); // Lo activa al iniciar
        }
    }

    public void TomarDaño(float daño)
    {
        vidaActual -= daño;
        vidaActual = Mathf.Max(vidaActual, 0);

        if (sliderVidaBoss != null)
        {
            sliderVidaBoss.value = vidaActual;
        }

        if (spriteRenderer != null)
        {
            StartCoroutine(ParpadeoTransparencia(2, 0.1f)); // Parpadea 2 veces rápido
        }

        if (vidaActual <= 0)
        {
            Morir();
        }
    }


    private IEnumerator ParpadeoTransparencia(int cantidadParpadeos, float duracionParpadeo)
    {
        for (int i = 0; i < cantidadParpadeos; i++)
        {
            // Hacerlo semi-transparente (alpha = 0.3)
            Color c = spriteRenderer.color;
            c.a = 0.3f;
            spriteRenderer.color = c;

            yield return new WaitForSeconds(duracionParpadeo);

            // Restaurar alpha original (1.0)
            c.a = 1f;
            spriteRenderer.color = c;

            yield return new WaitForSeconds(duracionParpadeo);
        }
    }

    private void Morir()
    {
        UnityEngine.Debug.Log("Boss derrotado.");

        if (objetoActivarAlMorir != null)
        {
            objetoActivarAlMorir.SetActive(true);
        }

        Destroy(gameObject);
    }

    private IEnumerator CambiarColorTemporalmente(Color nuevoColor, float duracion)
    {
        spriteRenderer.color = nuevoColor;
        yield return new WaitForSeconds(duracion);
        spriteRenderer.color = colorOriginal;
    }

}
