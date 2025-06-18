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
    public static bool EstaApuntandoConGranada = false;


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
        EstaApuntandoConGranada = false;

        if (inventario != null && inventario.TieneGranada())
        {
            if (Input.GetMouseButton(1)) // Click derecho mantiene apuntado
            {
                EstaApuntandoConGranada = true;
                MostrarTrayectoria();

                if (Input.GetMouseButtonDown(0)) // Click izquierdo para lanzar
                {
                    LanzarGranada();
                    inventario.UsarObjeto(); // Mano vacía luego de lanzar
                    OcultarTrayectoria();
                }
            }
            else
            {
                OcultarTrayectoria();
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

        // Asegurarse de que la granada no esté congelada al lanzarla
        Granada granadaScript = granada.GetComponent<Granada>();
        if (granadaScript != null)
        {
            granadaScript.UnfreezeTimer();
        }
    }

    private void MostrarTrayectoria()
    {
        Vector2 posicionInicial = puntoLanzamiento.position;
        Vector2 direccion = CalcularDireccionLanzamiento();

        // Calcular distancia del mouse al centro del jugador
        float distancia = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);

        // Mapear la distancia a la cantidad de puntos
        int puntosVisibles = Mathf.Clamp(Mathf.RoundToInt(distancia * 2f), 5, puntosTrayectoria);


        Vector2 velocidadInicial = direccion * fuerzaLanzamiento;
        float tiempoEntrePuntos = 0.1f;

        for (int i = 0; i < puntosTrayectoria; i++)
        {
            if (i >= puntosVisibles)
            {
                puntosVisuales[i].SetActive(false);
                continue;
            }

            float t = i * tiempoEntrePuntos;
            Vector2 posicion = posicionInicial + velocidadInicial * t + 0.5f * Physics2D.gravity * t * t;

            Collider2D col = Physics2D.OverlapCircle(posicion, 0.1f);
            if (col != null && !col.isTrigger && col.gameObject != this.gameObject)
            {
                puntosVisuales[i].SetActive(false);
                for (int j = i + 1; j < puntosTrayectoria; j++)
                {
                    puntosVisuales[j].SetActive(false);
                }
                break;
            }

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

        // Usa la posición del jugador como centro para calcular la dirección, no el puntoLanzamiento
        Vector3 centroJugador = transform.position;

        Vector2 direccion = (mouseWorldPosition - centroJugador);
        return direccion.normalized;
    }


}
