using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class BossVida : MonoBehaviour
{
    [Header("Vida del Boss")]
    [SerializeField] private float vidaMaxima = 5f;
    [SerializeField] private Slider sliderVidaBoss;

    private float vidaActual;

    private void Start()
    {
        vidaActual = vidaMaxima;

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

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        UnityEngine.Debug.Log("Boss derrotado.");
        Destroy(gameObject);
    }
}
