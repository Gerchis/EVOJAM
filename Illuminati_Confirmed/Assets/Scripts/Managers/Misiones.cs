using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class Misiones
{
    
    Misiones()
    {
        listaPersonajes = new List<Personajes>();
        resultado = 0;
        misionJugada = false;
    }

    public int resultadoFinal()
    {
        if (!jugadorEnMision)
        {
            return resultado;
        }
        resultado += GameManager.Instance.jugador.getApoyos();

        return resultado;
    }
    
    public void InitMisiones()
    {    
        
        for (int i = 0; i < listaPersonajes.Count; i++)
        {
            //TODO: Generar el canvas de la ficha(afinidades, cantidad de apoyos y voto)

            //Precalculamos el resultado antes de que vote el jugador

            resultado += listaPersonajes[i].getApoyos();         
        }
    }

    ~Misiones()
    {

    }

    public string titulo;
    Image[] avatarPosiciones;
    public List<Personajes> listaPersonajes;
    int resultado;
    public bool misionJugada;
    public bool jugadorEnMision;
}
