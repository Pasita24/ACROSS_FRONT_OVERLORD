using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;
        public float parallaxMultiplier;
    }

    [Header("Configuración")]
    [SerializeField] private ParallaxLayer[] layers;
    [SerializeField] private Transform camara;

    private Vector3 posicionAnteriorCamara;

    private void Start()
    {
        if (camara == null)
        {
            camara = Camera.main.transform;
        }

        posicionAnteriorCamara = camara.position;
    }

    private void Update()
    {
        Vector3 deltaMovimiento = camara.position - posicionAnteriorCamara;

        foreach (ParallaxLayer layer in layers)
        {
            Vector3 nuevaPos = layer.layerTransform.position;
            nuevaPos += new Vector3(deltaMovimiento.x * layer.parallaxMultiplier, 0, 0);
            layer.layerTransform.position = nuevaPos;
        }

        posicionAnteriorCamara = camara.position;
    }
}

