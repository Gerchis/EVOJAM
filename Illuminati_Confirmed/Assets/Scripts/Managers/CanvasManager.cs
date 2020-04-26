using System.Collections;
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

    GameManager gm;
    GameObject canvasPrensa;
    GameObject canvasResultados;
    GameObject canvasMisiones;

    public Sprite[] spritesSociedad;
    public Sprite[] spritesEconomia;
    public Sprite[] spritesDesarrollo;

    public Sprite[] spriteVotaciones;
    public Sprite spriteLocation; 


    public TextMeshProUGUI[] preciosPowerUps;
    public TextMeshProUGUI[] resultados;

        
    /*index
     * ###################
     * CANVAS ESTATICO: INVENTARIO
     * ###################
     */

    //Button pwrCensura;
    //Button pwrPublicidad;
    //Button pwrRevelar;
    //Button pwrEspia;

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

    //public GameObject[] informacionesConocidas;

    //INTEGRACIÓN: Continue de la tienda.
    public void setCanvasMisiones()
    {
        //Inicializamos misiones
        gm.initMisiones();


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

            //Configuramos la información mostrada en canvas de la ficha de cada IA
            for (int j=0; j < gm.misionesIngame[gm.idMisionesSeleccionadas[i]].listaPersonajes.Capacity; j++)
            {
                string id;

                //Para cada IA de la misión...
                if (j <gm.misionesIngame[gm.idMisionesSeleccionadas[i]].listaPersonajes.Count)
                {
                    //...activamos que se pueda abrir la ficha
                    id = "M" + i + "-S" + j + "-Avatar";
                    GameObject avatar = GameObject.Find(id);
                    avatar.GetComponent<Button>().interactable = true;

                    //...añadimos su avatar
                    avatar.GetComponent<Image>().sprite = gm.misionesIngame[gm.idMisionesSeleccionadas[i]].listaPersonajes[j].avatar;

                    //...reiniciamos la intención de voto revelada del turno anterior
                    id = "M" + i + "-S" + j + "-Voto";
                    GameObject.Find(id).SetActive(false);

                    id = "M" + i + "-S" + j + "-pwrVoto";
                    GameObject pwrVoto = GameObject.Find(id);
                    pwrVoto.SetActive(true);

                    if(gm.jugador.VerificarDisponibilidad(PowerupsName.AVERIGUAR_VOTO))
                    {
                        pwrVoto.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        pwrVoto.GetComponent<Button>().interactable = false;
                    }                    


                    //...ficha del personaje...
                    if(gm.misionesIngame[gm.idMisionesSeleccionadas[i]].listaPersonajes[j].getPersonajeInvestigado())
                    {
                        //...actualizamos su valor de apoyos
                        id = "M" + i + "-S" + j + "-ApoyosTexto";
                        GameObject.Find(id).GetComponent<TextMeshProUGUI>().text = gm.misionesIngame[gm.idMisionesSeleccionadas[i]].listaPersonajes[j].getApoyosRAW().ToString();
                        
                        //...la mostramos por pantalla
                        id = "M" + i + "-S" + j + "-Informacion";
                        GameObject.Find(id).SetActive(true);

                        id = "M" + i + "-S" + j + "-pwrInfo";
                        GameObject pwrInfo = GameObject.Find(id);
                        pwrInfo.SetActive(false);

                        if (gm.jugador.VerificarDisponibilidad(PowerupsName.INVESTIGADO))
                        {
                            pwrInfo.GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            pwrInfo.GetComponent<Button>().interactable = false;
                        }

                    }
                }
                else //Para cada slot desocupado de la misión...
                {
                    //Desactivamos que se pueda abrir la ficha
                    id = "M" + i + "-S" + j + "-Avatar";
                    GameObject.Find(id).GetComponent<Button>().interactable = false;
                }    
            }
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
        int valorMin=0;
        
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


    //NOTA: Podria unificar las dos funciones en una, y parsear 3 parametros. Pero el tercero dependeria de un enum, y no de un numero. Y casi que prefiero controlarlo con una funcion propia para cada powerup
    public void aplicarPwrVoto(string idParams)
    {
        //Parseamos lo que obtenemos del inspector de Unity a 2 variables
        //idMision-idSlot
        string[] parametros = idParams.Split('-');

        int i = int.Parse(parametros[0]);
        int j = int.Parse(parametros[1]);

        //Aplicamos powerup VOTO
        string id;
        id = "M" + i + "-S" + j + "-Voto";
        GameObject.Find(id).SetActive(true);

        id = "M" + i + "-S" + j + "-VotoIcono";
        if(gm.misionesIngame[gm.idMisionesSeleccionadas[i]].listaPersonajes[j].GetVotacion())
        {
            GameObject.Find(id).GetComponent<Image>().sprite = spriteVotaciones[1];
        } else
        {
            GameObject.Find(id).GetComponent<Image>().sprite = spriteVotaciones[0];
        }

        id = "M" + i + "-S" + j + "-pwrVoto";
        GameObject.Find(id).SetActive(false);

        gm.jugador.ConsumirPowerup(PowerupsName.AVERIGUAR_VOTO);
    }

    public void aplicarPwrInfo(string idParams)
    {
        //Parseamos lo que obtenemos del inspector de Unity a 2 variables
        //idMision-idSlot
        string[] parametros = idParams.Split('-');

        int i = int.Parse(parametros[0]);
        int j = int.Parse(parametros[1]);

        //Aplicamos powerup INVESTIGAR
        gm.misionesIngame[gm.idMisionesSeleccionadas[i]].listaPersonajes[j].setPersonajeInvestigado(true);

        string id;
        id = "M" + i + "-S" + j + "-Informacion";
        GameObject.Find(id).SetActive(true);

        id = "M" + i + "-S" + j + "-pwrInfo";
        GameObject.Find(id).SetActive(false);

        gm.jugador.ConsumirPowerup(PowerupsName.INVESTIGADO);

        //Actualizamos los sprites de las afiliaciones

        bool[] afinidades = gm.misionesIngame[gm.idMisionesSeleccionadas[i]].listaPersonajes[j].getAfinidadesEstadisticas();
        int num;

        id = "M" + i + "-S" + j + "-SociedadIcono";
        if (afinidades[0]) {num=1; } else {num=2; }
        GameObject.Find(id).GetComponent<Image>().sprite = spritesSociedad[num];

        id = "M" + i + "-S" + j + "-EconomiaIcono";
        if (afinidades[1]) { num = 1; } else { num = 2; }
        GameObject.Find(id).GetComponent<Image>().sprite = spritesEconomia[num];

        id = "M" + i + "-S" + j + "-DesarrolloIcono";
        if (afinidades[2]) { num = 1; } else { num = 2; }
        GameObject.Find(id).GetComponent<Image>().sprite = spritesDesarrollo[num];

    }


    public void switchButton(string idParams)
    {
        //Parseamos lo que obtenemos del inspector de Unity a 2 variables
        //idMision-Char(Y/N)
        string[] parametros = idParams.Split('-');

        int i = int.Parse(parametros[0]);
        char[] vote = parametros[1].ToCharArray();

        //Actualizamos comportamiento botones y seteamos su aportación de apoyos a la votación
        string id;

        if (vote[0] == 'Y')
        {
            id = "M" + i + "-SP-VotoSI";
            GameObject.Find(id).GetComponent<Button>().interactable = false;
            id = "M" + i + "-SP-VotoNO";
            GameObject.Find(id).GetComponent<Button>().interactable = true;

            //votacion = false >>>> suma apoyos al resultado
            gm.jugador.SetVotacion(true);
        }
        else if (vote[0] == 'N')
        {
            id = "M" + i + "-SP-VotoNO";
            GameObject.Find(id).GetComponent<Button>().interactable = false;
            id = "M" + i + "-SP-VotoSI";
            GameObject.Find(id).GetComponent<Button>().interactable = true;

            //votacion = false >>>> resta apoyos al resultado
            gm.jugador.SetVotacion(false);
        }
    }

    
    public void enterPlayerSlot(int m)
    {
        string id;

        for (int i = 0; i < gm.maxMisionesJugables; i++)
        {
            if (i != m) //Slots donde no estamos
            {
                //Seteamos que NO estamos en esa misión
                gm.misionesIngame[gm.idMisionesSeleccionadas[i]].setJugadorEnMision(false);

                //Asignamos avatar neutro
                id = "M" + i + "-SP-Avatar";
                GameObject.Find(id).GetComponent<Image>().sprite = spriteLocation;

                //Ocultamos popup
                id = "M" + i + "-SP-BackGround";
                GameObject.Find(id).SetActive(false);
                id = "M" + i + "-SP-VotoSI";
                GameObject.Find(id).SetActive(false);
                id = "M" + i + "-SP-VotoNO";
                GameObject.Find(id).SetActive(false);
            }
            else //Entramos en un SLOT
            {
                //Seteamos que estamos en esa misión
                gm.misionesIngame[gm.idMisionesSeleccionadas[i]].setJugadorEnMision(true);

                //Asignamos avatar del jugador
                id = "M" + i + "-SP-Avatar";
                GameObject.Find(id).GetComponent<Image>().sprite = gm.jugador.avatar;

                //Mostramos popup
                id = "M" + i + "-SP-BackGround";
                GameObject.Find(id).SetActive(true);
                id = "M" + i + "-SP-VotoSI";
                GameObject.Find(id).SetActive(true);
                id = "M" + i + "-SP-VotoNO";
                GameObject.Find(id).SetActive(true);
            }
        }
    }

    /*index
     * ###################
     * CANVAS DINAMICO: TIENDA
     * ###################
     */

    public void ComprarPowerup(int pwrName)
    {
        gm.BuyPowerUp((PowerupsName)pwrName);
    }

    /*index
     * ###################
     * CANVAS DINAMICO: PRENSA
     * ###################
     */
    public Sprite spriteCensura;
    public Sprite spritePublicidad;
    public Sprite spriteNoticia;

    TextMeshProUGUI[] titulosNoticias = new TextMeshProUGUI[3];
    TextMeshProUGUI[] textosNoticias = new TextMeshProUGUI[3];
    TextMeshProUGUI[] imagenesNoticias = new TextMeshProUGUI[3];
    string[] efectoSociedad = new string[3];
    string[] efectoEconomia = new string[3];
    string[] efectoDesarrollo = new string[3];
    Image[] IconoSociedad = new Image[3];
    Image[] IconoEconomia = new Image[3];
    Image[] IconoDesarrollo = new Image[3];
    Image[] ImagenCensura = new Image[3];
    Image[] ImagenPublicidad = new Image[3];
    Image[] ImagenNoticia = new Image[3];
    Button[] BotonCensura = new Button[3];
    Button[] BotonPublicidad = new Button[3];

    void setCanvasNoticias()
    {
        for (int i = 0; i < gm.maxMisionesJugables; i++)
        {
            titulosNoticias[i].text = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].titulo;
            textosNoticias[i].text = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].texto;
            ImagenNoticia[i].sprite = spriteNoticia;

            efectoSociedad[i] = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].efectosNoticia[0].valor.ToString();
            efectoEconomia[i] = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].efectosNoticia[1].valor.ToString();
            efectoDesarrollo[i] = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].efectosNoticia[2].valor.ToString();

            IconoSociedad[i].sprite = spritesSociedad[0];
            IconoEconomia[i].sprite = spritesEconomia[0];
            IconoDesarrollo[i].sprite = spritesDesarrollo[0];

            ImagenCensura[i].sprite = spriteCensura;
            ImagenCensura[i].enabled = false;
            ImagenPublicidad[i].sprite = spritePublicidad;
            ImagenPublicidad[i].enabled = false;
        }
        verificarPowerUp(PowerupsName.CENSURA);
        verificarPowerUp(PowerupsName.PUBLICIDAD);
        actualizarSliders();
    }

    public void aplicarCensura(int idNoticia)
    {
        ImagenCensura[idNoticia].enabled = true;
        efectoSociedad[idNoticia] = "0";
        efectoEconomia[idNoticia] = "0";
        efectoDesarrollo[idNoticia] = "0";
        setNoticiaAfectada(PowerupsName.CENSURA, idNoticia);
        verificarPowerUp(PowerupsName.CENSURA);
        actualizarSliders();
    }

    public void aplicarPublicidad(int idNoticia)
    {
        int aux;

        ImagenPublicidad[idNoticia].enabled = true;

        aux = gm.noticiasIngame[gm.idMisionesSeleccionadas[idNoticia]].efectosNoticia[0].valor * 2;
        efectoSociedad[idNoticia] = aux.ToString();

        aux = gm.noticiasIngame[gm.idMisionesSeleccionadas[idNoticia]].efectosNoticia[1].valor * 2;
        efectoEconomia[idNoticia] = aux.ToString();

        aux = gm.noticiasIngame[gm.idMisionesSeleccionadas[idNoticia]].efectosNoticia[2].valor * 2;
        efectoDesarrollo[idNoticia] = aux.ToString();

        setNoticiaAfectada(PowerupsName.PUBLICIDAD, idNoticia);
        verificarPowerUp(PowerupsName.PUBLICIDAD);
        actualizarSliders();
    }


    void setNoticiaAfectada(PowerupsName pwrName, int idNoticia)
    {
        for (int i = 0; i < gm.jugador.inventario.Count; i++)
        {
            if (gm.jugador.inventario[i].pwrNombreEnum == pwrName)
            {
                gm.jugador.inventario[i].noticiaAfectada = idNoticia;
            }
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
        canvasPrensa = GameObject.Find("CanvasPrensa");
        canvasResultados = GameObject.Find("CanvasResultados");
        canvasMisiones = GameObject.Find("CanvasVotaciones");
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

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

        ImagenCensura[0] = GameObject.Find("N1-ImagenCensura").GetComponent<Image>();
        ImagenCensura[1] = GameObject.Find("N2-ImagenCensura").GetComponent<Image>();
        ImagenCensura[2] = GameObject.Find("N3-ImagenCensura").GetComponent<Image>();

        ImagenPublicidad[0] = GameObject.Find("N1-ImagenPublicidad").GetComponent<Image>();
        ImagenPublicidad[1] = GameObject.Find("N2-ImagenPublicidad").GetComponent<Image>();
        ImagenPublicidad[2] = GameObject.Find("N3-ImagenPublicidad").GetComponent<Image>();


        ImagenNoticia[0] = GameObject.Find("N1-Imagen").GetComponent<Image>();
        ImagenNoticia[1] = GameObject.Find("N2-Imagen").GetComponent<Image>();
        ImagenNoticia[2] = GameObject.Find("N3-Imagen").GetComponent<Image>();

        preciosPowerUps[0] = GameObject.Find("Price 1").GetComponent<TextMeshProUGUI>();
        preciosPowerUps[1] = GameObject.Find("Price 2").GetComponent<TextMeshProUGUI>();
        preciosPowerUps[2] = GameObject.Find("Price 3").GetComponent<TextMeshProUGUI>();
        preciosPowerUps[3] = GameObject.Find("Price 4").GetComponent<TextMeshProUGUI>();
        preciosPowerUps[4] = GameObject.Find("Price 5").GetComponent<TextMeshProUGUI>();
        preciosPowerUps[5] = GameObject.Find("Price 6").GetComponent<TextMeshProUGUI>();
        BotonCensura[0] = GameObject.Find("N1-BotonCensurar").GetComponent<Button>();
        BotonCensura[1] = GameObject.Find("N2-BotonCensurar").GetComponent<Button>();
        BotonCensura[2] = GameObject.Find("N3-BotonCensurar").GetComponent<Button>();

        BotonPublicidad[0] = GameObject.Find("N1-BotonPublicitar").GetComponent<Button>();
        BotonPublicidad[0] = GameObject.Find("N2-BotonPublicitar").GetComponent<Button>();
        BotonPublicidad[0] = GameObject.Find("N3-BotonPublicitar").GetComponent<Button>();

        resultados[0] = GameObject.Find("Elementos").GetComponent<TextMeshProUGUI>();
        resultados[1] = GameObject.Find("Numeros").GetComponent<TextMeshProUGUI>();


        //Obtenemos referencias de los powerups... DEPRECATED
        //pwrCensura = GameObject.Find("pwrCensura").GetComponent<Button>();
        //pwrPublicidad = GameObject.Find("pwrPublicidad").GetComponent<Button>();
        //pwrRevelar = GameObject.Find("pwrRevelar").GetComponent<Button>();
        //pwrEspia = GameObject.Find("pwrEspia").GetComponent<Button>();


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

        sliderSociedad = GameObject.Find("SS-sliderSociedad").GetComponent<Slider>();
        sliderEconomia = GameObject.Find("SE-sliderEconomia").GetComponent<Slider>();
        sliderDesarrollo = GameObject.Find("SD-sliderDesarrollo").GetComponent<Slider>();
        sliderInvolucion = GameObject.Find("SI-sliderInvolucion").GetComponent<Slider>();

        sliderSociedadPositivo = GameObject.Find("SS-sliderSociedadPositivo").GetComponent<Slider>();
        sliderSociedadNegativo = GameObject.Find("SS-sliderSociedadNegativo").GetComponent<Slider>();

        sliderEconomiaPositivo = GameObject.Find("SE-sliderEconomiaPositivo").GetComponent<Slider>();
        sliderEconomiaNegativo = GameObject.Find("SE-sliderEconomiaNegativo").GetComponent<Slider>();

        sliderDesarrolloPositivo = GameObject.Find("SD-sliderDesarrolloPositivo").GetComponent<Slider>();
        sliderDesarrolloNegativo = GameObject.Find("SD-sliderDesarrolloNegativo").GetComponent<Slider>();

        sliderInvolucionPositivo = GameObject.Find("SI-sliderInvolucionPositivo").GetComponent<Slider>();
        sliderInvolucionNegativo = GameObject.Find("SI-sliderInvolucionPositivo").GetComponent<Slider>();


        // -----------------
        // |    GENERAL    |
        // -----------------

        gm.InitGame();

        //Desactivamos parents...
        canvasPrensa.SetActive(false);
        canvasResultados.SetActive(false);
        setCanvasMisiones();
        canvasMisiones.SetActive(true);

    }

    public void continueToResult()
    {
        for (int i = 0; i < gm.precios.Length; i++)
        {
            preciosPowerUps[i].text = gm.precios[i].ToString();
        }

        resultados[0].text = "Seguidores Ganados\r\nInfluencia Ganada\r\n-----------------------------------------\r\nTOTAL";
        resultados[1].text = gm.modificadorSeguidores.ToString() + "\r\n" + gm.jugador.seguidores.ToString() + "\r\n-----------------------------------------\r\n" + gm.jugador.influencia.ToString();
    }

    void verificarPowerUp(PowerupsName _name)
    {
        if (gm.jugador.VerificarDisponibilidad(_name))
        {
            for (int i = 0; i < gm.maxMisionesJugables; i++)
            {
                switch (_name)
                {
                    case PowerupsName.CENSURA:
                        BotonCensura[i].interactable = true;
                        break;
                    case PowerupsName.PUBLICIDAD:
                        BotonPublicidad[i].interactable = true;
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            for (int i = 0; i < gm.maxMisionesJugables; i++)
            {
                switch (_name)
                {
                    case PowerupsName.CENSURA:
                        BotonCensura[i].interactable = false;
                        break;
                    case PowerupsName.PUBLICIDAD:
                        BotonPublicidad[i].interactable = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }

}
