using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Noticias
{
    Noticias() {

    }

    ~Noticias() {

    }

    public void SetTitulo(string _titulo) { titulo = _titulo; }

    [Header("Variables Noticia")]
    public string titulo;
    public string texto;
    public Image imagen;

    [System.Serializable]
    
    public struct Efectos
    {
        public Estadisticas estadistica;
        public int valor;
    }

    [Header("Efectos Noticia")]
    public Efectos[] efectosNoticia = new Efectos[3];

}
