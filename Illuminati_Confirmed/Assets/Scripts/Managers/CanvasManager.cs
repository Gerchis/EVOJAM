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


    TextMeshProUGUI[] preciosPowerUps = new TextMeshProUGUI[6];
    TextMeshProUGUI[] resultados = new TextMeshProUGUI[2];


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

    GameObject sliderSociedadObjetivo;
    GameObject sliderEconomiaObjetivo;
    GameObject sliderDesarrolloObjetivo;
    GameObject sliderInvolucionObjetivo;    

    //Por cada noticia[] > efecto[]
    int[,] modificacionValores = new int[3,3];

    
    public void setDefinitiveSliders()
    {
        sliderSociedad.value = gm.sociedadActual;
        sliderEconomia.value = gm.economiaActual;
        sliderDesarrollo.value = gm.desarrolloActual;
        sliderInvolucion.value = gm.involucionActual;
    }

    public void initSlidersValues()
    {

        if(gm.sociedadObjetivo == -1)
        {
            sliderSociedadObjetivo.SetActive(false);
        } else
        {
            sliderSociedadObjetivo.GetComponent<Slider>().value = gm.sociedadObjetivo;
        }

        if (gm.economiaObjetivo == -1)
        {
            sliderEconomiaObjetivo.SetActive(false);
        }
        else
        {
            sliderEconomiaObjetivo.GetComponent<Slider>().value = gm.economiaObjetivo;
        }

        if(gm.desarrolloObjetivo == -1)
        {
            sliderDesarrolloObjetivo.SetActive(false);
        }
        else
        {
            sliderDesarrolloObjetivo.GetComponent<Slider>().value = gm.desarrolloObjetivo;
        }

        sliderSociedad.value = gm.sociedadActual;
        sliderEconomia.value = gm.economiaActual;
        sliderDesarrollo.value = gm.desarrolloActual;

        sliderInvolucionObjetivo.GetComponent<Slider>().value = gm.involucionObjetivo;
        gm.calcularInvolucion();
        sliderInvolucion.value = gm.involucionActual;
    }


    public void actualizarSliders()
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
            modificacionSociedad += modificacionValores[i,0];
            modificacionEconomia += modificacionValores[i,1];
            modificacionDesarrollo += modificacionValores[i,2];
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
        setSlidersComplementario(previoInvolucion, gm.involucionActual, Estadisticas.TOTAL_ESTADISTICAS);
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
                modificacionValores[i,j] = 0;
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
                modificacionValores[i,j] = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].efectosNoticia[j].valor;

                //Si es última iteración del for(j) AKA ya se ha calculado todos los modificacionValores[i][k] & pwrPublicidad == mision.
                if (j == gm.maxMisionesJugables - 1 && gm.jugador.powerupActivo(PowerupsName.PUBLICIDAD, i))
                {
                    //Aplicamos efecto del powerup
                    for (int k = 0; k < gm.maxMisionesJugables; k++)
                    {
                        modificacionValores[i,k] *= 2;
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
    #region MISIONES
    TextMeshProUGUI[] titulosMisionesSeleccionadas = new TextMeshProUGUI[3];    
    Image[] iconosM2 = new Image[3];
    Image[] iconosM1 = new Image[3];
    Image[] iconosM0 = new Image[3];

    GameObject[,] Background = new GameObject[3, 3];
    Button[,] iaAvatar = new Button[3, 3];
    GameObject[,] Informacion = new GameObject[3, 3];
    GameObject[,] InformacionInfo = new GameObject[3, 3];
    TextMeshProUGUI[,] ApoyosTexto = new TextMeshProUGUI[3, 3];
    GameObject[,] Voto = new GameObject[3, 3];
    GameObject[,] VotoInfo = new GameObject[3, 3];
    GameObject[,] pwrVoto = new GameObject[3, 3];
    GameObject[,] pwrInfo = new GameObject[3, 3];
    GameObject[] spVotoSI = new GameObject[3];
    GameObject[] spVotoNO = new GameObject[3];
    GameObject[] spAvatar = new GameObject[3];
    GameObject[,] Exit = new GameObject[3, 3];
    GameObject[] spBackground = new GameObject[3];
    Image[,] SociedadIcono = new Image[3, 3];
    Image[,] EconomiaIcono = new Image[3, 3];
    Image[,] DesarrolloIcono = new Image[3, 3];
    Image [,] VotoIcono = new Image[3, 3];
    GameObject[,] Poderes = new GameObject[3, 3];

    //public GameObject[] informacionesConocidas;

    //INTEGRACIÓN: Continue de la tienda.
    public void setCanvasMisiones()
    {
        //Inicializamos misiones
        gm.initMisiones();

        //Para cada mision en pantalla
        for (int i = 0; i < gm.maxMisionesJugables; i++)
        {
            //Ocultamos panel de playerSlot
            spBackground[i].SetActive(false);
            spVotoSI[i].SetActive(false);
            spVotoNO[i].SetActive(false);

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
            for (int j=0; j < gm.maxSlotsMisiones; j++)
            {
                //Para cada IA de la misión...
                if (j <gm.misionesIngame[gm.idMisionesSeleccionadas[i]].listaPersonajes.Count)
                {
                    //...activamos que se pueda abrir la ficha
                    iaAvatar[i,j].interactable = true;

                    //...añadimos su avatar
                    iaAvatar[i,j].GetComponent<Image>().sprite = gm.misionesIngame[gm.idMisionesSeleccionadas[i]].listaPersonajes[j].avatar;

                    //...reiniciamos la intención de voto revelada del turno anterior
                    VotoInfo[i, j].SetActive(false);
                    pwrVoto[i, j].SetActive(true);

                    if (gm.jugador.VerificarDisponibilidad(PowerupsName.AVERIGUAR_VOTO))
                    {
                        pwrVoto[i, j].GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        pwrVoto[i, j].GetComponent<Button>().interactable = false;
                    }

                    //...ficha del personaje...
                    //...actualizamos su valor de apoyos
                    ApoyosTexto[i, j].text = gm.misionesIngame[gm.idMisionesSeleccionadas[i]].listaPersonajes[j].getApoyosRAW().ToString();
                    
                    if (gm.misionesIngame[gm.idMisionesSeleccionadas[i]].listaPersonajes[j].getPersonajeInvestigado())
                    {
                        //...la mostramos por pantalla la info
                        InformacionInfo[i,j].SetActive(true);
                        pwrInfo[i,j].SetActive(false);
                    }
                    else
                    {
                        //...la mostramos por pantalla el powerup
                        InformacionInfo[i, j].SetActive(false);
                        pwrInfo[i, j].SetActive(true);

                        if (gm.jugador.VerificarDisponibilidad(PowerupsName.INVESTIGADO))
                        {
                            pwrInfo[i, j].GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            pwrInfo[i, j].GetComponent<Button>().interactable = false;
                        }
                    }
                }
                else //Para cada slot desocupado de la misión...
                {
                    //Desactivamos que se pueda abrir la ficha
                    iaAvatar[i, j].GetComponent<Image>().sprite = null;
                    iaAvatar[i,j].interactable = false;
                    
                }    
            }
        }

        //Ocultamos toda la visibilidad de las fichas (por si queda alguna abierta)
        ocultarTodasVisibilidadFicha();
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
    //NOTA2: Para un futuro. Leer el nombre del elemento y sacar de allí los parametros tipo id M[X] e S[X]....me he dado cuenta tarde :/
    public void aplicarPwrVoto(string idParams)
    {
        //Parseamos lo que obtenemos del inspector de Unity a 2 variables
        //idMision-idSlot
        string[] parametros = idParams.Split('-');

        int i = int.Parse(parametros[0]);
        int j = int.Parse(parametros[1]);

        //Aplicamos powerup VOTO        
        VotoInfo[i, j].SetActive(true);

        if(gm.misionesIngame[gm.idMisionesSeleccionadas[i]].listaPersonajes[j].GetVotacion())
        {
            VotoIcono[i,j].sprite = spriteVotaciones[1];
        } else
        {
            VotoIcono[i, j].sprite = spriteVotaciones[0];
        }

        pwrVoto[i, j].SetActive(false);        

        gm.jugador.ConsumirPowerup(PowerupsName.AVERIGUAR_VOTO);
        UpdateInventario();

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

        InformacionInfo[i, j].SetActive(true);        
        pwrInfo[i, j].SetActive(false);
        
        gm.jugador.ConsumirPowerup(PowerupsName.INVESTIGADO);

        //Actualizamos los sprites de las afiliaciones
        bool[] afinidades = gm.misionesIngame[gm.idMisionesSeleccionadas[i]].listaPersonajes[j].getAfinidadesEstadisticas();
        int num;
        
        if (afinidades[0]) {num=1; } else {num=2; }
        SociedadIcono[i, j].GetComponent<Image>().sprite = spritesSociedad[num];

        if (afinidades[1]) { num = 1; } else { num = 2; }
        EconomiaIcono[i, j].GetComponent<Image>().sprite = spritesEconomia[num];

        if (afinidades[2]) { num = 1; } else { num = 2; }
        DesarrolloIcono[i, j].GetComponent<Image>().sprite = spritesDesarrollo[num];

        UpdateInventario();
    }


    public void votacionPlayer(string idParams)
    {
        //Parseamos lo que obtenemos del inspector de Unity a 2 variables
        //idMision-Char(Y/N)
        string[] parametros = idParams.Split('-');

        int i = int.Parse(parametros[0]);
        char[] vote = parametros[1].ToCharArray();

        //Actualizamos comportamiento botones y seteamos su aportación de apoyos a la votación
        if (vote[0] == 'Y')
        {
            spVotoSI[i].GetComponent<Button>().interactable = false;
            spVotoNO[i].GetComponent<Button>().interactable = true;

            //votacion = false >>>> suma apoyos al resultado
            gm.jugador.SetVotacion(true);
        }
        else if (vote[0] == 'N')
        {
            spVotoNO[i].GetComponent<Button>().interactable = false;
            spVotoSI[i].GetComponent<Button>().interactable = true;

            //votacion = false >>>> resta apoyos al resultado
            gm.jugador.SetVotacion(false);
        }
    }

    
    public void enterPlayerSlot(int m)
    {
        //BY DEFAULT: Si usuario no selecciona una opción SI/NO. Vota automaticamente SI.
        gm.jugador.SetVotacion(true);


        //Logica de acceso:
        for (int i = 0; i < gm.maxMisionesJugables; i++)
        {
            if (i != m) //Slots donde no estamos
            {
                //Seteamos que NO estamos en esa misión
                gm.misionesIngame[gm.idMisionesSeleccionadas[i]].setJugadorEnMision(false);

                //Asignamos avatar neutro
                spAvatar[i].GetComponent<Image>().sprite = spriteLocation;

                //Actualizamos botones de votacion para que vuelvan a estar disponibles
                spVotoNO[i].GetComponent<Button>().interactable = true;
                spVotoSI[i].GetComponent<Button>().interactable = true;

                //Ocultamos popup
                spBackground[i].SetActive(false);
                spVotoSI[i].SetActive(false);
                spVotoNO[i].SetActive(false);
            }
            else //Entramos en un SLOT
            {
                //Seteamos que estamos en esa misión
                gm.misionesIngame[gm.idMisionesSeleccionadas[i]].setJugadorEnMision(true);

                //Asignamos avatar del jugador
                spAvatar[i].GetComponent<Image>().sprite = gm.jugador.avatar;

                //Mostramos popup
                spBackground[i].SetActive(true);
                spVotoSI[i].SetActive(true);
                spVotoNO[i].SetActive(true);
            }
        }
    }


    public void VisibilidadFicha(string idParams)
    {
        //Parseamos lo que obtenemos del inspector de Unity a 2 variables
        //idMision-idSlot-Activar(T)/Desactivar(F)
        string[] parametros = idParams.Split('-');

        int i = int.Parse(parametros[0]);
        int j = int.Parse(parametros[1]);
        char[] estado = parametros[2].ToCharArray();
        bool activado;

        if (estado[0]=='Y')
        {
            activado = true;
        }
        else
        {
            activado = false;
        }

        //Actualizamos comportamiento botones y seteamos su aportación de apoyos a la votación
        Background[i,j].SetActive(activado);
        Exit[i, j].SetActive(activado);

        Informacion[i,j].SetActive(activado);
        Voto[i,j].SetActive(activado);
        Poderes[i,j].SetActive(activado);
    }

    void ocultarTodasVisibilidadFicha()
    {
        for (int i = 0; i < gm.maxMisionesJugables; i++)
        {
            for (int j = 0; j < gm.maxSlotsMisiones; j++)
            {
                Background[i, j].SetActive(false);
                Exit[i, j].SetActive(false);

                Informacion[i, j].SetActive(false);
                Voto[i, j].SetActive(false);
                Poderes[i, j].SetActive(false);
            }
        }
    }

    #endregion

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

    Image[] iconosPU = new Image[6];
    public Sprite[] powerUpSprites;
    Text[] countPU = new Text[6];

    public void setCanvasNoticias()
    {
        gm.exitMisiones();

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
        UpdateInventario();
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
        UpdateInventario();
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

        preciosPowerUps[0] = GameObject.Find("Price1").GetComponent<TextMeshProUGUI>();
        preciosPowerUps[1] = GameObject.Find("Price2").GetComponent<TextMeshProUGUI>();
        preciosPowerUps[2] = GameObject.Find("Price3").GetComponent<TextMeshProUGUI>();
        preciosPowerUps[3] = GameObject.Find("Price4").GetComponent<TextMeshProUGUI>();
        preciosPowerUps[4] = GameObject.Find("Price5").GetComponent<TextMeshProUGUI>();
        preciosPowerUps[5] = GameObject.Find("Price6").GetComponent<TextMeshProUGUI>();

        BotonCensura[0] = GameObject.Find("N1-BotonCensurar").GetComponent<Button>();
        BotonCensura[1] = GameObject.Find("N2-BotonCensurar").GetComponent<Button>();
        BotonCensura[2] = GameObject.Find("N3-BotonCensurar").GetComponent<Button>();

        BotonPublicidad[0] = GameObject.Find("N1-BotonPublicitar").GetComponent<Button>();
        BotonPublicidad[1] = GameObject.Find("N2-BotonPublicitar").GetComponent<Button>();
        BotonPublicidad[2] = GameObject.Find("N3-BotonPublicitar").GetComponent<Button>();

        resultados[0] = GameObject.Find("Elementos").GetComponent<TextMeshProUGUI>();
        resultados[1] = GameObject.Find("Numeros").GetComponent<TextMeshProUGUI>();

        GameObject.Find("Price1").SetActive(false);
        GameObject.Find("Price2").SetActive(false);
        GameObject.Find("Price3").SetActive(false);
        GameObject.Find("Price4").SetActive(false);
        GameObject.Find("Price5").SetActive(false);
        GameObject.Find("Price6").SetActive(false);


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

        iconosM1[0] = GameObject.Find("M1-SociedadIcono").GetComponent<Image>();
        iconosM1[1] = GameObject.Find("M1-EconomiaIcono").GetComponent<Image>();
        iconosM1[2] = GameObject.Find("M1-DesarrolloIcono").GetComponent<Image>();

        iconosM2[0] = GameObject.Find("M2-SociedadIcono").GetComponent<Image>();
        iconosM2[1] = GameObject.Find("M2-EconomiaIcono").GetComponent<Image>();
        iconosM2[2] = GameObject.Find("M2-DesarrolloIcono").GetComponent<Image>();

        Background[0, 0] = GameObject.Find("M0-S0-Background");
        Background[0, 1] = GameObject.Find("M0-S1-Background");
        Background[0, 2] = GameObject.Find("M0-S2-Background");
        Background[1, 0] = GameObject.Find("M1-S0-Background");
        Background[1, 1] = GameObject.Find("M1-S1-Background");
        Background[1, 2] = GameObject.Find("M1-S2-Background");
        Background[2, 0] = GameObject.Find("M2-S0-Background");
        Background[2, 1] = GameObject.Find("M2-S1-Background");
        Background[2, 2] = GameObject.Find("M2-S2-Background");

        Informacion[0, 0] = GameObject.Find("M0-S0-Informacion");
        Informacion[0, 1] = GameObject.Find("M0-S1-Informacion");
        Informacion[0, 2] = GameObject.Find("M0-S2-Informacion");
        Informacion[1, 0] = GameObject.Find("M1-S0-Informacion");
        Informacion[1, 1] = GameObject.Find("M1-S1-Informacion");
        Informacion[1, 2] = GameObject.Find("M1-S2-Informacion");
        Informacion[2, 0] = GameObject.Find("M2-S0-Informacion");
        Informacion[2, 1] = GameObject.Find("M2-S1-Informacion");
        Informacion[2, 2] = GameObject.Find("M2-S2-Informacion");

        InformacionInfo[0, 0] = GameObject.Find("M0-S0-InformacionInfo");
        InformacionInfo[0, 1] = GameObject.Find("M0-S1-InformacionInfo");
        InformacionInfo[0, 2] = GameObject.Find("M0-S2-InformacionInfo");
        InformacionInfo[1, 0] = GameObject.Find("M1-S0-InformacionInfo");
        InformacionInfo[1, 1] = GameObject.Find("M1-S1-InformacionInfo");
        InformacionInfo[1, 2] = GameObject.Find("M1-S2-InformacionInfo");
        InformacionInfo[2, 0] = GameObject.Find("M2-S0-InformacionInfo");
        InformacionInfo[2, 1] = GameObject.Find("M2-S1-InformacionInfo");
        InformacionInfo[2, 2] = GameObject.Find("M2-S2-InformacionInfo");

        Voto[0, 0] = GameObject.Find("M0-S0-Voto");
        Voto[0, 1] = GameObject.Find("M0-S1-Voto");
        Voto[0, 2] = GameObject.Find("M0-S2-Voto");
        Voto[1, 0] = GameObject.Find("M1-S0-Voto");
        Voto[1, 1] = GameObject.Find("M1-S1-Voto");
        Voto[1, 2] = GameObject.Find("M1-S2-Voto");
        Voto[2, 0] = GameObject.Find("M2-S0-Voto");
        Voto[2, 1] = GameObject.Find("M2-S1-Voto");
        Voto[2, 2] = GameObject.Find("M2-S2-Voto");

        VotoInfo[0, 0] = GameObject.Find("M0-S0-VotoInfo");
        VotoInfo[0, 1] = GameObject.Find("M0-S1-VotoInfo");
        VotoInfo[0, 2] = GameObject.Find("M0-S2-VotoInfo");
        VotoInfo[1, 0] = GameObject.Find("M1-S0-VotoInfo");
        VotoInfo[1, 1] = GameObject.Find("M1-S1-VotoInfo");
        VotoInfo[1, 2] = GameObject.Find("M1-S2-VotoInfo");
        VotoInfo[2, 0] = GameObject.Find("M2-S0-VotoInfo");
        VotoInfo[2, 1] = GameObject.Find("M2-S1-VotoInfo");
        VotoInfo[2, 2] = GameObject.Find("M2-S2-VotoInfo");

        pwrVoto[0, 0] = GameObject.Find("M0-S0-pwrVoto");
        pwrVoto[0, 1] = GameObject.Find("M0-S1-pwrVoto");
        pwrVoto[0, 2] = GameObject.Find("M0-S2-pwrVoto");
        pwrVoto[1, 0] = GameObject.Find("M1-S0-pwrVoto");
        pwrVoto[1, 1] = GameObject.Find("M1-S1-pwrVoto");
        pwrVoto[1, 2] = GameObject.Find("M1-S2-pwrVoto");
        pwrVoto[2, 0] = GameObject.Find("M2-S0-pwrVoto");
        pwrVoto[2, 1] = GameObject.Find("M2-S1-pwrVoto");
        pwrVoto[2, 2] = GameObject.Find("M2-S2-pwrVoto");

        pwrInfo[0, 0] = GameObject.Find("M0-S0-pwrInfo");
        pwrInfo[0, 1] = GameObject.Find("M0-S1-pwrInfo");
        pwrInfo[0, 2] = GameObject.Find("M0-S2-pwrInfo");
        pwrInfo[1, 0] = GameObject.Find("M1-S0-pwrInfo");
        pwrInfo[1, 1] = GameObject.Find("M1-S1-pwrInfo");
        pwrInfo[1, 2] = GameObject.Find("M1-S2-pwrInfo");
        pwrInfo[2, 0] = GameObject.Find("M2-S0-pwrInfo");
        pwrInfo[2, 1] = GameObject.Find("M2-S1-pwrInfo");
        pwrInfo[2, 2] = GameObject.Find("M2-S2-pwrInfo");

        SociedadIcono[0, 0] = GameObject.Find("M0-S0-SociedadIcono").GetComponent<Image>();
        SociedadIcono[0, 1] = GameObject.Find("M0-S1-SociedadIcono").GetComponent<Image>();
        SociedadIcono[0, 2] = GameObject.Find("M0-S2-SociedadIcono").GetComponent<Image>();
        SociedadIcono[1, 0] = GameObject.Find("M1-S0-SociedadIcono").GetComponent<Image>();
        SociedadIcono[1, 1] = GameObject.Find("M1-S1-SociedadIcono").GetComponent<Image>();
        SociedadIcono[1, 2] = GameObject.Find("M1-S2-SociedadIcono").GetComponent<Image>();
        SociedadIcono[2, 0] = GameObject.Find("M2-S0-SociedadIcono").GetComponent<Image>();
        SociedadIcono[2, 1] = GameObject.Find("M2-S1-SociedadIcono").GetComponent<Image>();
        SociedadIcono[2, 2] = GameObject.Find("M2-S2-SociedadIcono").GetComponent<Image>();

        EconomiaIcono[0, 0] = GameObject.Find("M0-S0-EconomiaIcono").GetComponent<Image>();
        EconomiaIcono[0, 1] = GameObject.Find("M0-S1-EconomiaIcono").GetComponent<Image>();
        EconomiaIcono[0, 2] = GameObject.Find("M0-S2-EconomiaIcono").GetComponent<Image>();
        EconomiaIcono[1, 0] = GameObject.Find("M1-S0-EconomiaIcono").GetComponent<Image>();
        EconomiaIcono[1, 1] = GameObject.Find("M1-S1-EconomiaIcono").GetComponent<Image>();
        EconomiaIcono[1, 2] = GameObject.Find("M1-S2-EconomiaIcono").GetComponent<Image>();
        EconomiaIcono[2, 0] = GameObject.Find("M2-S0-EconomiaIcono").GetComponent<Image>();
        EconomiaIcono[2, 1] = GameObject.Find("M2-S1-EconomiaIcono").GetComponent<Image>();
        EconomiaIcono[2, 2] = GameObject.Find("M2-S2-EconomiaIcono").GetComponent<Image>();

        DesarrolloIcono[0, 0] = GameObject.Find("M0-S0-DesarrolloIcono").GetComponent<Image>();
        DesarrolloIcono[0, 1] = GameObject.Find("M0-S1-DesarrolloIcono").GetComponent<Image>();
        DesarrolloIcono[0, 2] = GameObject.Find("M0-S2-DesarrolloIcono").GetComponent<Image>();
        DesarrolloIcono[1, 0] = GameObject.Find("M1-S0-DesarrolloIcono").GetComponent<Image>();
        DesarrolloIcono[1, 1] = GameObject.Find("M1-S1-DesarrolloIcono").GetComponent<Image>();
        DesarrolloIcono[1, 2] = GameObject.Find("M1-S2-DesarrolloIcono").GetComponent<Image>();
        DesarrolloIcono[2, 0] = GameObject.Find("M2-S0-DesarrolloIcono").GetComponent<Image>();
        DesarrolloIcono[2, 1] = GameObject.Find("M2-S1-DesarrolloIcono").GetComponent<Image>();
        DesarrolloIcono[2, 2] = GameObject.Find("M2-S2-DesarrolloIcono").GetComponent<Image>();

        iaAvatar[0, 0] = GameObject.Find("M0-S0-Avatar").GetComponent<Button>();
        iaAvatar[0, 1] = GameObject.Find("M0-S1-Avatar").GetComponent<Button>();
        iaAvatar[0, 2] = GameObject.Find("M0-S2-Avatar").GetComponent<Button>();
        iaAvatar[1, 0] = GameObject.Find("M1-S0-Avatar").GetComponent<Button>();
        iaAvatar[1, 1] = GameObject.Find("M1-S1-Avatar").GetComponent<Button>();
        iaAvatar[1, 2] = GameObject.Find("M1-S2-Avatar").GetComponent<Button>();
        iaAvatar[2, 0] = GameObject.Find("M2-S0-Avatar").GetComponent<Button>();
        iaAvatar[2, 1] = GameObject.Find("M2-S1-Avatar").GetComponent<Button>();
        iaAvatar[2, 2] = GameObject.Find("M2-S2-Avatar").GetComponent<Button>();

        Poderes[0, 0] = GameObject.Find("M0-S0-Poderes");
        Poderes[0, 1] = GameObject.Find("M0-S1-Poderes");
        Poderes[0, 2] = GameObject.Find("M0-S2-Poderes");
        Poderes[1, 0] = GameObject.Find("M1-S0-Poderes");
        Poderes[1, 1] = GameObject.Find("M1-S1-Poderes");
        Poderes[1, 2] = GameObject.Find("M1-S2-Poderes");
        Poderes[2, 0] = GameObject.Find("M2-S0-Poderes");
        Poderes[2, 1] = GameObject.Find("M2-S1-Poderes");
        Poderes[2, 2] = GameObject.Find("M2-S2-Poderes");

        Exit[0, 0] = GameObject.Find("M0-S0-Exit");
        Exit[0, 1] = GameObject.Find("M0-S1-Exit");
        Exit[0, 2] = GameObject.Find("M0-S2-Exit");
        Exit[1, 0] = GameObject.Find("M1-S0-Exit");
        Exit[1, 1] = GameObject.Find("M1-S1-Exit");
        Exit[1, 2] = GameObject.Find("M1-S2-Exit");
        Exit[2, 0] = GameObject.Find("M2-S0-Exit");
        Exit[2, 1] = GameObject.Find("M2-S1-Exit");
        Exit[2, 2] = GameObject.Find("M2-S2-Exit");

        ApoyosTexto[0, 0] = GameObject.Find("M0-S0-ApoyosTexto").GetComponent<TextMeshProUGUI>();
        ApoyosTexto[0, 1] = GameObject.Find("M0-S1-ApoyosTexto").GetComponent<TextMeshProUGUI>();
        ApoyosTexto[0, 2] = GameObject.Find("M0-S2-ApoyosTexto").GetComponent<TextMeshProUGUI>();
        ApoyosTexto[1, 0] = GameObject.Find("M1-S0-ApoyosTexto").GetComponent<TextMeshProUGUI>();
        ApoyosTexto[1, 1] = GameObject.Find("M1-S1-ApoyosTexto").GetComponent<TextMeshProUGUI>();
        ApoyosTexto[1, 2] = GameObject.Find("M1-S2-ApoyosTexto").GetComponent<TextMeshProUGUI>();
        ApoyosTexto[2, 0] = GameObject.Find("M2-S0-ApoyosTexto").GetComponent<TextMeshProUGUI>();
        ApoyosTexto[2, 1] = GameObject.Find("M2-S1-ApoyosTexto").GetComponent<TextMeshProUGUI>();
        ApoyosTexto[2, 2] = GameObject.Find("M2-S2-ApoyosTexto").GetComponent<TextMeshProUGUI>();

        spVotoNO[0] = GameObject.Find("M0-SP-VotoNO");
        spVotoNO[1] = GameObject.Find("M1-SP-VotoNO");
        spVotoNO[2] = GameObject.Find("M2-SP-VotoNO");

        spVotoSI[0] = GameObject.Find("M0-SP-VotoSI");
        spVotoSI[1] = GameObject.Find("M1-SP-VotoSI");
        spVotoSI[2] = GameObject.Find("M2-SP-VotoSI");

        spAvatar[0] = GameObject.Find("M0-SP-Avatar");
        spAvatar[1] = GameObject.Find("M1-SP-Avatar");
        spAvatar[2] = GameObject.Find("M2-SP-Avatar");

        spBackground[0] = GameObject.Find("M0-SP-Background");
        spBackground[1] = GameObject.Find("M1-SP-Background");
        spBackground[2] = GameObject.Find("M2-SP-Background");




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

        sliderSociedadObjetivo = GameObject.Find("SS-sliderObjetivo");
        sliderEconomiaObjetivo = GameObject.Find("SE-sliderObjetivo");
        sliderDesarrolloObjetivo = GameObject.Find("SD-sliderObjetivo");
        sliderInvolucionObjetivo = GameObject.Find("SI-sliderObjetivo");
       
        iconosPU[0] = GameObject.Find("IconPU1").GetComponent<Image>();
        iconosPU[1] = GameObject.Find("IconPU2").GetComponent<Image>();
        iconosPU[2] = GameObject.Find("IconPU3").GetComponent<Image>();
        iconosPU[3] = GameObject.Find("IconPU4").GetComponent<Image>();
        iconosPU[4] = GameObject.Find("IconPU5").GetComponent<Image>();
        iconosPU[5] = GameObject.Find("IconPU6").GetComponent<Image>();

        countPU[0] = GameObject.Find("Slot1").GetComponent<Text>();
        countPU[1] = GameObject.Find("Slot2").GetComponent<Text>();
        countPU[2] = GameObject.Find("Slot3").GetComponent<Text>();
        countPU[3] = GameObject.Find("Slot4").GetComponent<Text>();
        countPU[4] = GameObject.Find("Slot5").GetComponent<Text>();
        countPU[5] = GameObject.Find("Slot6").GetComponent<Text>();

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

    public void UpdateInventario()
    {
        for (int i = 0; i < iconosPU.Length; i++)
        {
            iconosPU[i].color = new Color(255, 255, 255, 0);
            countPU[i].text = null;
        }

        for (int i = 0; i < gm.jugador.inventario.Count; i++)
        {
            iconosPU[i].sprite = powerUpSprites[(int)gm.jugador.inventario[i].pwrNombreEnum];
            iconosPU[i].color = new Color(255, 255, 255, 1);

            switch (gm.jugador.inventario[i].pwrNombreEnum)
            {
                case PowerupsName.APOYOS:

                    countPU[i].text = gm.jugador.apoyos.ToString();

                    break;
                case PowerupsName.SEGUIDORES:

                    countPU[i].text = gm.jugador.seguidores.ToString();

                    break;
                default:

                    countPU[i].text = gm.jugador.inventario[i].cantidad.ToString();

                    break;
            }
        }
    }

    public void initSeguidores()
    {
        gm.jugador.inventario.Add(new Powerups());
        gm.jugador.inventario[0].pwrNombreEnum = PowerupsName.SEGUIDORES;

        gm.jugador.inventario.Add(new Powerups());
        gm.jugador.inventario[1].pwrNombreEnum = PowerupsName.APOYOS;
    }

    public void continueToResult()
    {
        for (int i = 0; i < gm.precios.Length; i++)
        {
            Debug.LogWarning(i);
            Debug.Log(preciosPowerUps.Length);
            Debug.Log(gm.precios.Length);
            Debug.Log(gm.precios[i].ToString());
            Debug.Log(preciosPowerUps[i].text);
            
            preciosPowerUps[i].text = "casa";
            //preciosPowerUps[i].text = gm.precios[i].ToString();
        }

        resultados[0].text = "Seguidores Ganados\r\nInfluencia Ganada\r\n-----------------------------------------\r\nTOTAL";
        resultados[1].text = gm.modificadorSeguidores.ToString() + "\r\n" + gm.jugador.seguidores.ToString() + "\r\n-----------------------------------------\r\n" + gm.jugador.influencia.ToString();

        UpdateInventario();
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
