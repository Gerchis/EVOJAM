using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    /*index
     * VARIABLES GENERALES
     */

    GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    GameObject parentPrensa;
    GameObject parentTienda;
    GameObject parentMisiones;


    /*index
     * CANVAS ESTATICO: INVENTARIO
     */

    Button pwrCensura;
    Button pwrPublicidad;
    Button pwrRevelar;
    Button pwrEspia;

    //gm.jugador.inventario.pwrCensura;


    /*index
     * CANVAS ESTATICO: INVOLUCION
     */

    /*index
     * CANVAS DINAMICO: MISIONES
     */
    public TextMeshProUGUI[] titulosMisionesSeleccionadas = new TextMeshProUGUI[3];

    

    void setCanvasMisiones()
    {
        for (int i = 0; i < gm.maxMisionesJugables; i++)
        {
            titulosMisionesSeleccionadas[i].text = gm.misionesIngame[gm.idMisionesSeleccionadas[i]].titulo;
        }
    }

    /*index
     * CANVAS DINAMICO: PRENSA
     */
    TextMeshProUGUI[] titulosNoticias = new TextMeshProUGUI[3];
    TextMeshProUGUI[] textosNoticias = new TextMeshProUGUI[3];
    TextMeshProUGUI[] imagenesNoticias = new TextMeshProUGUI[3];
    string[] efectoSociedad = new string[3];
    string[] efectoEconomia = new string[3];
    string[] efectoDesarrollo = new string[3];

    void setCanvasNoticias()
    {
        for (int i = 0; i < gm.maxMisionesJugables; i++)
        {
            titulosNoticias[i].text = gm.noticiasIngame[gm.idMisionesSeleccionadas[i]].titulo;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //Obtenemos referencias de los apartados...
        parentPrensa = GameObject.Find("parentPrensa");
        parentTienda = GameObject.Find("parentTienda");
        parentMisiones = GameObject.Find("parentMisiones");

        //Obtenemos referencias del Canvas Prensa...
        titulosNoticias[0] = GameObject.Find("N1-Titular").GetComponent<TextMeshProUGUI>();
        titulosNoticias[1] = GameObject.Find("N2-Titular").GetComponent<TextMeshProUGUI>();
        titulosNoticias[2] = GameObject.Find("N3-Titular").GetComponent<TextMeshProUGUI>();

        textosNoticias[0] = GameObject.Find("N1-Text").GetComponent<TextMeshProUGUI>();
        textosNoticias[1] = GameObject.Find("N2-Text").GetComponent<TextMeshProUGUI>();
        textosNoticias[2] = GameObject.Find("N3-Text").GetComponent<TextMeshProUGUI>();

        imagenesNoticias[0] = GameObject.Find("N1-Imagen").GetComponent<TextMeshProUGUI>();
        imagenesNoticias[1] = GameObject.Find("N2-Imagen").GetComponent<TextMeshProUGUI>();
        imagenesNoticias[2] = GameObject.Find("N3-Imagen").GetComponent<TextMeshProUGUI>();

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

        //Obtenemos referencias de los powerups...
        pwrCensura = GameObject.Find("pwrCensura").GetComponent<Button>();
        pwrPublicidad = GameObject.Find("pwrPublicidad").GetComponent<Button>();
        pwrRevelar = GameObject.Find("pwrRevelar").GetComponent<Button>();
        pwrEspia = GameObject.Find("pwrEspia").GetComponent<Button>();


        //Desactivamos parents...
        parentPrensa.SetActive(false);
        parentTienda.SetActive(false);
        parentMisiones.SetActive(false);
    }
}
