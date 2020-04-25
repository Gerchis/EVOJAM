using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Misiones
{
    
    Misiones()
    {
        
    }

    public Misiones(string _titulo, Image[] _posiciones, int[] _illuminati)
    {
        resultado = 0;
        titulo = _titulo;
        for (int i = 0; i < _posiciones.Length; i++)
        {
            //Asignamos los illuminati a la misión
            posiciones[i].sprite = _posiciones[i].sprite;
            
            //Precalculamos el resultado antes de que vote el jugador
            resultado += _illuminati[i];
        }
    }

    ~Misiones()
    {

    }

    string titulo;
    Image[] posiciones;
    int resultado;

}
