using System.Diagnostics;
using UnityEngine;

public class InventarioMano : MonoBehaviour
{
    public enum ObjetoEnMano
    {
        Nada,
        Granada,
        Ladrillo
    }


    public ObjetoEnMano objetoActual = ObjetoEnMano.Nada;

    public bool TieneGranada()
    {
        return objetoActual == ObjetoEnMano.Granada;
    }

    public void TomarGranada()
    {
        if (objetoActual == ObjetoEnMano.Nada)
        {
            objetoActual = ObjetoEnMano.Granada;
            UnityEngine.Debug.Log("Granada tomada");
        }
    }
    public void TomarLadrillo()
    {
        if (objetoActual == ObjetoEnMano.Nada)
        {
            objetoActual = ObjetoEnMano.Ladrillo;
            UnityEngine.Debug.Log("Ladrillo tomado");
        }
    }


    public void UsarObjeto()
    {
        objetoActual = ObjetoEnMano.Nada;
        UnityEngine.Debug.Log("Objeto usado (mano vacía)");
    }
}
