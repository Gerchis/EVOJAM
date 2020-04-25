using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Noticias
{
    Noticias() {

    }

    public Noticias(string _titulo, string _texto, Image _imagen, int _valorSociedad, int _valorEconomia, int _valorDesarrollo)
    {
        titulo = _titulo;
        texto = _texto;
        imagen.sprite = _imagen.sprite;

        efectosNoticia[0].estadistica = Estadisticas.SOCIEDAD;
        efectosNoticia[0].valor = _valorSociedad;
        efectosNoticia[1].estadistica = Estadisticas.ECONOMIA;
        efectosNoticia[1].valor = _valorEconomia;
        efectosNoticia[2].estadistica = Estadisticas.DESARROLLO;
        efectosNoticia[2].valor = _valorDesarrollo;
    }

    ~Noticias() {

    }

    public string titulo;
    public string texto;
    public Image imagen;

    public struct Efectos
    {
        public Estadisticas estadistica;
        public int valor;
    }

    public Efectos[] efectosNoticia;

}
