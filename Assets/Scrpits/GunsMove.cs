using UnityEngine;

public class GunsMove : MonoBehaviour
{
    [SerializeField] private float floatAmplitude = 0.5f; // Amplitud del movimiento (cu�nto sube/baja)
    [SerializeField] private float floatFrequency = 1f; // Frecuencia del movimiento (velocidad de la oscilaci�n)

    private Vector3 initialPosition; // Posici�n inicial del arma

    void Start()
    {
        // Guardar la posici�n inicial del arma
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calcular el desplazamiento en el eje Y usando una funci�n seno para un movimiento suave
        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Actualizar la posici�n del arma, movi�ndola solo en el eje Y
        transform.position = initialPosition + new Vector3(0f, yOffset, 0f);
    }
}