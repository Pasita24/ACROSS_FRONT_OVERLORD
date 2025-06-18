using UnityEngine;
using TMPro;

public class TriggerFinal : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject panelMensajeFinal; // Panel que contiene el mensaje final
    [SerializeField] private TextMeshProUGUI textoMensajeFinal;
    [SerializeField] private string mensajeFinal = "¡Mensaje entregado con éxito!";

    [Header("Configuración")]
    [SerializeField] private bool desactivarTrigger = true;

    private void Start()
    {
        // Asegurarse que el panel está desactivado al inicio
        if (panelMensajeFinal != null)
        {
            panelMensajeFinal.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Congelar al jugador
        var jugador = collision.GetComponent<MovimientoJugador>();
        jugador?.PausarMovimiento();

        // Mostrar el mensaje final
        MostrarMensaje();

        // Desactivar el trigger si es necesario
        if (desactivarTrigger)
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void MostrarMensaje()
    {
        if (panelMensajeFinal == null) return;

        panelMensajeFinal.SetActive(true);

        if (textoMensajeFinal != null)
        {
            textoMensajeFinal.text = mensajeFinal;
        }
    }

    // Método público para ocultar el mensaje cuando sea necesario
    public void OcultarMensaje()
    {
        if (panelMensajeFinal != null)
        {
            panelMensajeFinal.SetActive(false);
        }
    }
}