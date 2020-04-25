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
    public Efectos[] efectosNoticia;

}
