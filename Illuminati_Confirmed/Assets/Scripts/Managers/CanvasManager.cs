﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    /*index
     * ###################
     * VARIABLES GENERALES
     * ###################
     */

    GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    GameObject parentPrensa;
    GameObject parentTienda;
    GameObject parentMisiones;

    public Sprite[] spritesSociedad;
    public Sprite[] spritesEconomia;
    public Sprite[] spritesDesarrollo;

        
    /*index
     * ###################
     * CANVAS ESTATICO: INVENTARIO
     * ###################
     */

    Button pwrCensura;
    Button pwrPublicidad;
    Button pwrRevelar;
    Button pwrEspia;

    //gm.jugador.inventario.pwrCensura;


    /*index
     * ###################
     * CANVAS ESTATICO: INVOLUCION
     * ###################
     */

    #region INVOLUCION
    //Slider principal.
    Slider sliderSociedad;
    Slider sliderEconomia;
    Slider sliderDesarrollo;
    Slider sliderInvolucion;

    //Sliders complementarios para displayar progreso jugador en un efecto.
    Slider sliderSociedadPositivo;
    Slider sliderSociedadNegativo;
    Slider sliderEconomiaPositivo;
    Slider sliderEconomiaNegativo;
    Slider sliderDesarrolloPositivo;
    Slider sliderDesarrolloNegativo;
    Slider sliderInvolucionPositivo;
    Slider sliderInvolucionNegativo;

    //Por cada noticia[] > efecto[]
    int[][] modificacionValores;

    void actualizarSliders()
    {
        int previoSociedad;
        int previoEconomia;
        int previoDesarrollo;
        int previoInvolucion;

        int modificacionSociedad = 0;
        int modificacionEconomia = 0;
        int modificacionDesarrollo = 0;

        //Obtenemos la modificacion de cada mision
        getModificacionValores();

        //Obtenemos la modificación final de los sliders
        for (int i = 0; i < gm.maxMisionesJugables; i++)
        {
            modificacionSociedad += modificacionValores[i][0];
            modificacionEconomia += modificacionValores[i][1];
            modificacionDesarrollo += modificacionValores[i][2];
        }

        //Guardamos valor previo de los efectos actuales
        previoSociedad = gm.sociedadActual;
        previoEconomia = gm.economiaActual;
        previoDesarrollo = gm.desarrolloActual;
        previoInvolucion = gm.involucionActual;

        //Modificamos el valor de los efectos actuales
        gm.sociedadActual += modificacionSociedad;
        gm.economiaActual += modificacionEconomia;
        gm.desarrolloActual += modificacionDesarrollo;
        gm.calcularInvolucion();

        //Reseteamos a 0 los sliders de Positivo/Negativo
        sliderSociedadPositivo.value = 0;
        sliderSociedadNegativo.value = 0;
        sliderEconomiaPositivo.value = 0;
        sliderEconomiaNegativo.value = 0;
        sliderDesarrolloPositivo.value = 0;
        sliderDesarrolloNegativo.value = 0;
        sliderInvolucionPositivo.value = 0;
        sliderInvolucionNegativo.value = 0;

        //Seteamos los sliders complementarios y principal en función de la evolución
        setSlidersComplementario(previoSociedad, gm.sociedadActual, Estadisticas.SOCIEDAD);
        setSlidersComplementario(previoEconomia, gm.economiaActual, Estadisticas.ECONOMIA);
        setSlidersComplementario(previoDesarrollo, gm.desarrolloActual, Estadisticas.DESARROLLO);
        setSlidersComplementario(previoDesarrollo, gm.desarrolloActual, Estadisticas.TOTAL_ESTADISTICAS);
    }

    void setSlidersComplementario(int previo, int actual, Estadisticas efecto)
    {
        int valorSliderPositivo = 0;
        int valorSliderNegativo = 0;
        int valorSlider = 0;

        if (previo < actual) // Efecto aumenta
        {
            valorSliderPositivo = actual;
            valorSlider = previo;
        }
        else if (previo > actual) // Efecto disminuye
        {
            valorSliderNegativo = previo;
            valorSlider = actual;
        }
        else //No hay cambios
        {
            valorSlider = actual;
        }

        switch (efecto)
        {
            case Estadisticas.SOCIEDAD:
            sliderSociedadPositivo.value = valorSliderPositivo;
            sliderSociedadNegativo.value = valorSliderNegativo;
            sliderSociedad.value = valorSlider;
            break;
            case Estadisticas.ECONOMIA:
            sliderEconomiaPositivo.value = valorSliderPositivo;
            sliderEconomiaNegativo.value = valorSliderNegativo;
            sliderEconomia.value = valorSlider;
            break;
            case Estadisticas.DESARROLLO:
            sliderDesarrolloPositivo.value = valorSliderPositivo;
            sliderDesarrolloNegativo.value = valorSliderNegativo;
            sliderDesarrollo.value = valorSlider;
            break;
            case Estadisticas.TOTAL_ESTADISTICAS: //INVOLUCION
            sliderInvolucionPositivo.value = valorSliderPositivo;
            sliderInvolucionNegativo.value = valorSliderNegativo;
            sliderInvolucion.value = valorSlider;
            break;
        }
    }

    void getModificacionValores()
    {
        //Limpiamos contenidos previos
        for (int i = 0; i < gm.maxMisionesJugables; i++)
        {
            for (int j = 0; j < gm.maxMisionesJugables; j++)
            {
                modificacionValores[i][j] = 0;
            }
        }


        //Calculamos nuevos contenidos.
        //i = noticia
        for (int i = 0; i < gm.maxMisionesJugables; i++)
        {

            //j = efecto
            for (int j = 0; j < gm.maxMisionesJugables; j++)
            {
                if (gm.jugador.powerupActivo(PowerupsName.CENSURA, i))
                {
                    //Aplicamos efecto del powerup
                    j = gm.maxMisionesJugables;
                    continue;
                }

                modificacionValores[i][j] = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].efectosNoticia[j].valor;

                //Si es última iteración del for(j) AKA ya se ha calculado todos los modificacionValores[i][k] & pwrPublicidad == mision.
                if (j == gm.maxMisionesJugables - 1 && gm.jugador.powerupActivo(PowerupsName.PUBLICIDAD, i))
                {
                    //Aplicamos efecto del powerup
                    for (int k = 0; k < gm.maxMisionesJugables; k++)
                    {
                        modificacionValores[i][k] *= 2;
                    }
                }
            }
        }

    }

    #endregion
    /*index
     * ###################
     * CANVAS DINAMICO: MISIONES
     * ###################
     */
    TextMeshProUGUI[] titulosMisionesSeleccionadas = new TextMeshProUGUI[3];    
    Image[] iconosM2 = new Image[3];
    Image[] iconosM1 = new Image[3];
    Image[] iconosM0 = new Image[3];

    
    public void setCanvasMisiones()
    {      
        //Para cada mision en pantalla
        for (int i = 0; i < gm.maxMisionesJugables; i++)
        {
            //Variables locales
            

            //Mostramos/ocultamos los iconos relevantes de la misión
            switch (i)
            {
                case 0:
                setActiveIconosMision(iconosM0, getEfectoPrincipal(i));
                break;
                case 1:
                setActiveIconosMision(iconosM1, getEfectoPrincipal(i));
                break;
                case 2:
                setActiveIconosMision(iconosM2, getEfectoPrincipal(i));
                break;
            }
                  
            //Seteamos los titulos
            titulosMisionesSeleccionadas[i].text = gm.misionesIngame[gm.idMisionesSeleccionadas[i]].titulo;
        }
    }

    void setActiveIconosMision(Image[] iconos, Estadisticas efectoPrincipal)
    {
        switch (efectoPrincipal)
        {
            case Estadisticas.SOCIEDAD:
                iconos[0].enabled = true;
                iconos[1].enabled = false;
                iconos[2].enabled = false;
            break;

            case Estadisticas.ECONOMIA:
                iconos[0].enabled = false;
                iconos[1].enabled = true;
                iconos[2].enabled = false;
            break;

            case Estadisticas.DESARROLLO:
                iconos[0].enabled = false;
                iconos[1].enabled = false;
                iconos[2].enabled = true;
            break;
        }   
    }

    Estadisticas getEfectoPrincipal(int i)
    {
        int valorMin = 0;
        Estadisticas topEfecto = Estadisticas.TOTAL_ESTADISTICAS;

        for (int j = 0; j < gm.maxMisionesJugables; j++)
        {
            int num = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].efectosNoticia[j].valor;

            if ( num < valorMin)
            {
                valorMin = num;
                topEfecto = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].efectosNoticia[j].estadistica;
            }
        }
        
        return topEfecto;
    }

    

    /*index
     * ###################
     * CANVAS DINAMICO: PRENSA
     * ###################
     */
    TextMeshProUGUI[] titulosNoticias = new TextMeshProUGUI[3];
    TextMeshProUGUI[] textosNoticias = new TextMeshProUGUI[3];
    TextMeshProUGUI[] imagenesNoticias = new TextMeshProUGUI[3];
    string[] efectoSociedad = new string[3];
    string[] efectoEconomia = new string[3];
    string[] efectoDesarrollo = new string[3];
    Image[] IconoSociedad = new Image[3];
    Image[] IconoEconomia = new Image[3];
    Image[] IconoDesarrollo = new Image[3];
    Button[] BotonCensura = new Button[3];
    Button[] BotonPublicidad = new Button[3];

    void setCanvasNoticias()
    {
        for (int i = 0; i < gm.maxMisionesJugables; i++)
        {
            titulosNoticias[i].text = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].titulo;
            textosNoticias[i].text = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].texto;

            efectoSociedad[i] = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].efectosNoticia[0].valor.ToString();
            efectoEconomia[i] = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].efectosNoticia[1].valor.ToString();
            efectoDesarrollo[i] = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].efectosNoticia[2].valor.ToString();

            IconoSociedad[i].sprite = spritesSociedad[0];
            IconoEconomia[i].sprite = spritesEconomia[0];
            IconoDesarrollo[i].sprite = spritesDesarrollo[0];

        }
    }

    /*index
    * ###################
    * FUNCIONES UNITY
    * ###################
    */

    void Start()
    {
        // -----------------
        // |    GENERAL    |
        // -----------------
        parentPrensa = GameObject.Find("parentPrensa");
        parentTienda = GameObject.Find("parentTienda");
        parentMisiones = GameObject.Find("parentMisiones");

        // ----------------
        // |    PRENSA    |
        // ----------------
        //Referencas del Canvas Prensa...
        titulosNoticias[0] = GameObject.Find("N1-Titular").GetComponent<TextMeshProUGUI>();
        titulosNoticias[1] = GameObject.Find("N2-Titular").GetComponent<TextMeshProUGUI>();
        titulosNoticias[2] = GameObject.Find("N3-Titular").GetComponent<TextMeshProUGUI>();

        textosNoticias[0] = GameObject.Find("N1-Text").GetComponent<TextMeshProUGUI>();
        textosNoticias[1] = GameObject.Find("N2-Text").GetComponent<TextMeshProUGUI>();
        textosNoticias[2] = GameObject.Find("N3-Text").GetComponent<TextMeshProUGUI>();

        imagenesNoticias[0] = GameObject.Find("N1-Imagen").GetComponent<TextMeshProUGUI>();
        imagenesNoticias[1] = GameObject.Find("N2-Imagen").GetComponent<TextMeshProUGUI>();
        imagenesNoticias[2] = GameObject.Find("N3-Imagen").GetComponent<TextMeshProUGUI>();

        efectoSociedad[0] = GameObject.Find("N1-EfectoSociedad").GetComponent<TextMeshProUGUI>().text;
        efectoSociedad[1] = GameObject.Find("N2-EfectoSociedad").GetComponent<TextMeshProUGUI>().text;
        efectoSociedad[2] = GameObject.Find("N3-EfectoSociedad").GetComponent<TextMeshProUGUI>().text;

        efectoEconomia[0] = GameObject.Find("N1-EfectoEconomia").GetComponent<TextMeshProUGUI>().text;
        efectoEconomia[1] = GameObject.Find("N2-EfectoEconomia").GetComponent<TextMeshProUGUI>().text;
        efectoEconomia[2] = GameObject.Find("N3-EfectoEconomia").GetComponent<TextMeshProUGUI>().text;

        efectoDesarrollo[0] = GameObject.Find("N1-EfectoDesarrollo").GetComponent<TextMeshProUGUI>().text;
        efectoDesarrollo[1] = GameObject.Find("N2-EfectoDesarrollo").GetComponent<TextMeshProUGUI>().text;
        efectoDesarrollo[2] = GameObject.Find("N3-EfectoDesarrollo").GetComponent<TextMeshProUGUI>().text;

        IconoSociedad[0] = GameObject.Find("N1-IconoSociedad").GetComponent<Image>();
        IconoSociedad[1] = GameObject.Find("N2-IconoSociedad").GetComponent<Image>();
        IconoSociedad[2] = GameObject.Find("N3-IconoSociedad").GetComponent<Image>();

        IconoEconomia[0] = GameObject.Find("N1-IconoEconomia").GetComponent<Image>();
        IconoEconomia[1] = GameObject.Find("N2-IconoEconomia").GetComponent<Image>();
        IconoEconomia[2] = GameObject.Find("N3-IconoEconomia").GetComponent<Image>();

        IconoDesarrollo[0] = GameObject.Find("N1-IconoDesarrollo").GetComponent<Image>();
        IconoDesarrollo[1] = GameObject.Find("N2-IconoDesarrollo").GetComponent<Image>();
        IconoDesarrollo[2] = GameObject.Find("N3-IconoDesarrollo").GetComponent<Image>();

        BotonCensura[0] = GameObject.Find("N1-BotonCensura").GetComponent<Button>();
        BotonCensura[1] = GameObject.Find("N1-BotonCensura").GetComponent<Button>();
        BotonCensura[2] = GameObject.Find("N1-BotonCensura").GetComponent<Button>();

        BotonPublicidad[0] = GameObject.Find("N1-BotonPublicidad").GetComponent<Button>();
        BotonPublicidad[0] = GameObject.Find("N1-BotonPublicidad").GetComponent<Button>();
        BotonPublicidad[0] = GameObject.Find("N1-BotonPublicidad").GetComponent<Button>();



        //Obtenemos referencias de los powerups...
        pwrCensura = GameObject.Find("pwrCensura").GetComponent<Button>();
        pwrPublicidad = GameObject.Find("pwrPublicidad").GetComponent<Button>();
        pwrRevelar = GameObject.Find("pwrRevelar").GetComponent<Button>();
        pwrEspia = GameObject.Find("pwrEspia").GetComponent<Button>();


        // ------------------
        // |    MISIONES    |
        // ------------------

        titulosMisionesSeleccionadas[0] = GameObject.Find("M0-Titulo").GetComponent<TextMeshProUGUI>();
        titulosMisionesSeleccionadas[1] = GameObject.Find("M1-Titulo").GetComponent<TextMeshProUGUI>();
        titulosMisionesSeleccionadas[2] = GameObject.Find("M2-Titulo").GetComponent<TextMeshProUGUI>();

        iconosM0[0] = GameObject.Find("M0-SociedadIcono").GetComponent<Image>();
        iconosM0[1] = GameObject.Find("M0-EconomiaIcono").GetComponent<Image>();
        iconosM0[2] = GameObject.Find("M0-DesarrolloIcono").GetComponent<Image>();

        iconosM1[0] = GameObject.Find("M0-SociedadIcono").GetComponent<Image>();
        iconosM1[1] = GameObject.Find("M0-EconomiaIcono").GetComponent<Image>();
        iconosM1[2] = GameObject.Find("M0-DesarrolloIcono").GetComponent<Image>();

        iconosM2[0] = GameObject.Find("M0-SociedadIcono").GetComponent<Image>();
        iconosM2[1] = GameObject.Find("M0-EconomiaIcono").GetComponent<Image>();
        iconosM2[2] = GameObject.Find("M0-DesarrolloIcono").GetComponent<Image>();

        // -----------------
        // |    GENERAL    |
        // -----------------

        //Desactivamos parents...
        parentPrensa.SetActive(false);
        parentTienda.SetActive(false);
        parentMisiones.SetActive(false);


        if(gm)


    }

}
