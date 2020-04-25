using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class Misiones
{
    
    Misiones()
    {
        
    }

    public Misiones(string _titulo, Image[] _posiciones, int[] _apoyos)
    {
        resultado = 0;
        titulo = _titulo;
        misionJugada = false;
        for (int i = 0; i < _posiciones.Length; i++)
        {
            //Asignamos los illuminati a la misión
            posiciones[i].sprite = _posiciones[i].sprite;
            
            //Precalculamos el resultado antes de que vote el jugador
            resultado += _apoyos[i];
        }
    }

    ~Misiones()
    {

    }

    public string titulo;
    Image[] posiciones;
    int resultado;
    bool misionJugada;
}
