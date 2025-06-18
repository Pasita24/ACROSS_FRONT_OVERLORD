using UnityEngine;

public class LanzadorDeGranadas : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private GameObject prefabGranada;
    [SerializeField] private Transform puntoLanzamiento;
    [SerializeField] private float fuerzaLanzamiento = 10f;

    [Header("Trayectoria")]
    [SerializeField] private int puntosTrayectoria = 30;
    [SerializeField] private GameObject puntoPrefab;

    private GameObject[] puntosVisuales;
    private InventarioMano inventario;

    private void Start()
    {
        puntosVisuales = new GameObject[puntosTrayectoria];
        for (int i = 0; i < puntosTrayectoria; i++)
        {
            puntosVisuales[i] = Instantiate(puntoPrefab);
            puntosVisuales[i].SetActive(false);
        }

        inventario = GetComponent<InventarioMano>();
    }

    private void Update()
    {
        if (inventario != null && inventario.TieneGranada())
        {
            MostrarTrayectoria();

            if (Input.GetKeyDown(KeyCode.G))
            {
                LanzarGranada();
                inventario.UsarObjeto(); // Mano vacía luego de lanzar
            }
        }
        else
        {
            OcultarTrayectoria();
        }
    }

    private void LanzarGranada()
    {
        GameObject granada = Instantiate(prefabGranada, puntoLanzamiento.position, Quaternion.identity);
        Rigidbody2D rb = granada.GetComponent<Rigidbody2D>();
        rb.velocity = CalcularDireccionLanzamiento() * fuerzaLanzamiento;
    }

    private void MostrarTrayectoria()
    {
        Vector2 posicionInicial = puntoLanzamiento.position;
        Vector2 velocidadInicial = CalcularDireccionLanzamiento() * fuerzaLanzamiento;
        float tiempoEntrePuntos = 0.1f;

        for (int i = 0; i < puntosTrayectoria; i++)
        {
            float t = i * tiempoEntrePuntos;
            Vector2 posicion = posicionInicial + velocidadInicial * t + 0.5f * Physics2D.gravity * t * t;
            puntosVisuales[i].transform.position = posicion;
            puntosVisuales[i].SetActive(true);
        }
    }

    private void OcultarTrayectoria()
    {
        foreach (var punto in puntosVisuales)
        {
            punto.SetActive(false);
        }
    }

    private Vector2 CalcularDireccionLanzamiento()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direccion = (mouseWorldPosition - puntoLanzamiento.position);
        return direccion.normalized;
    }

}
