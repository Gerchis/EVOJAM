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
    
    public void precalcularResultadoFinal()
    {    
        for (int i = 0; i < listaPersonajes.Count; i++)
        {
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
    bool jugadorEnMision;

    public void setJugadorEnMision(bool _b) { jugadorEnMision = _b; }
}
