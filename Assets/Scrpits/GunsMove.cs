using UnityEngine;

public class GunsMove : MonoBehaviour
{
    [SerializeField] private float floatAmplitude = 0.5f; // Amplitud del movimiento (cuánto sube/baja)
    [SerializeField] private float floatFrequency = 1f; // Frecuencia del movimiento (velocidad de la oscilación)

    private Vector3 initialPosition; // Posición inicial del arma

    void Start()
    {
        // Guardar la posición inicial del arma
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calcular el desplazamiento en el eje Y usando una función seno para un movimiento suave
        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Actualizar la posición del arma, moviéndola solo en el eje Y
        transform.position = initialPosition + new Vector3(0f, yOffset, 0f);
    }
}