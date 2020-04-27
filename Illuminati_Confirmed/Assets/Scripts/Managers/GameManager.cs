using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum Estadisticas
{
    SOCIEDAD,
    ECONOMIA,
    DESARROLLO,
    TOTAL_ESTADISTICAS
}

public enum RolSecreto
{
    COMUNISTA,
    LIBERAL,
    CIENTIFICO,
    EMPRENDEDOR,
    SOCIALISTA,
    CAPITALISTA,
    FILANTROPO,
    CAOTICO,
    TOTAL_ROLES
}

public enum PowerupsName
{
    CENSURA,
    PUBLICIDAD,
    INVESTIGADO,
    AVERIGUAR_VOTO,
    APOYOS,
    SEGUIDORES,
    TOTAL_POWERUPSNAME
}


public class GameManager : MonoBehaviour
{

    /*index
     * VARIABLES DE JUEGO
     */
    public int maxMisionesJugables = 3;
    public int maxSlotsMisiones = 3;
    public Personajes jugador;
    Personajes[] pnjs = new Personajes[7];
    public Sprite[] avatares;
    bool[] avataresControl = new bool[(int)RolSecreto.TOTAL_ROLES];
    bool[] rolControl = new bool[(int)RolSecreto.TOTAL_ROLES];
    public Image playerAvatar;


    //VALUES PJS
    int randRol;
    int randAvatar;
    int influencia;
    int apoyos;
    int seguidores;

    public int[] precios;

    public void InitGame()
    {

        sociedadActual = Random.Range(80, 101);
        economiaActual = Random.Range(80, 101);
        desarrolloActual = Random.Range(80, 101);

        involucionActual = sociedadActual + economiaActual + desarrolloActual;
        involucionObjetivo = 120; //120 de 300

        //TODO: Hacer flujo de rondas. Win/Lose GAME
        turnActual = 0;
        turnMax = 12;

        for (int i = 0; i < (int)RolSecreto.TOTAL_ROLES; i++)
        {
            avataresControl[i] = false;
            rolControl[i] = false;
        }


        SetValuesPjs();
        jugador = new Personajes((RolSecreto)randRol, avatares[randAvatar], influencia, apoyos, seguidores, false);
        playerAvatar = GameObject.Find("PlayerAvatar").GetComponent<Image>();
        playerAvatar.sprite = avatares[randAvatar];
        for (int i = 0; i < pnjs.Length; i++)
        {
            SetValuesPjs();
            pnjs[i] = new Personajes((RolSecreto)randRol, avatares[randAvatar], influencia, apoyos, seguidores, true);
        }

        //TODO: Hacer INIT de sliders
        initSlidersValues();
    }

    void actualizarSliders()
    {
        GameObject.Find("CanvasManager").GetComponent<CanvasManager>().actualizarSliders();
    }

    void initSlidersValues()
    {
        GameObject.Find("CanvasManager").GetComponent<CanvasManager>().initSlidersValues();
    }

    void SetValuesPjs()
    {
        do
        {
            randRol = Random.Range(0, (int)RolSecreto.TOTAL_ROLES);
        } while (rolControl[randRol]);
        
        do
        {
            randAvatar = Random.Range(0, avatares.Length);
        } while(avataresControl[randAvatar]);
        
        avataresControl[randAvatar] = true;
        rolControl[randRol] = true;

        influencia = Random.Range(100, 300);
        apoyos = Random.Range(1, 3);
        seguidores = Random.Range(1, 3);
    }

    /*index
     * ESTADISTICAS DE JUEGO
     */

    [Header("EstadisticasDeJuego")]
    
    public int sociedadActual;
    public int economiaActual;
    public int desarrolloActual;
    [Space(10)]
    public int sociedadObjetivo;
    public int economiaObjetivo;
    public int desarrolloObjetivo;  
    [Space(10)]
    public int involucionActual;
    public int involucionObjetivo;
    
    //TODO HACER FLOW JUEGO
    int turnActual;
    int turnMax;

    public void calcularInvolucion()
    {
        involucionActual = sociedadActual + economiaActual + desarrolloActual;
    }


    /*index
     * LOGICA: MISIONES
     */

    [Header("Misiones")]
    [SerializeField]
    public Misiones[] misionesIngame;

    public int[] idMisionesSeleccionadas; //Tamaño 3, las 3 misiones que pueden salir. Guardamos el ID de la mision.

    public void initMisiones()
    {
        //Seleccionamos Misiones que se jugarán
        seleccionarMisiones();
        
        //Asignamos IA a las Misiones
        asignarPosiciones();

        //Precalculamos el resultado de las Misiones.
        for (int i = 0; i < maxMisionesJugables; i++)
        {
            misionesIngame[idMisionesSeleccionadas[i]].precalcularResultadoFinal();
        }
    }

    public void exitMisiones()
    {
        //Calculamos el resultado de las Misiones donde está participando el jugador.
        for (int i = 0; i < maxMisionesJugables; i++)
        {
            misionesIngame[idMisionesSeleccionadas[i]].resultadoFinal();
        }
    }

    void seleccionarMisiones()
    {
        int num;
        bool controlFail = false;
        int control = 0;

        for (int i = 0; i < maxMisionesJugables; i++)
        {
            do
            {
                control++;
                num = Random.Range(0, misionesIngame.Length);
                if (control >= 100000) { controlFail = true; } //Paranonia de MARC
            } while (misionesIngame[num].misionJugada  || controlFail);

            if (controlFail)
            {
                Debug.LogWarning("ALERTA: Más de 100.000 iteraciónes en num RANDOM.");
            }
            control = 0;
            controlFail = false;

            misionesIngame[num].misionJugada = true;
            idMisionesSeleccionadas[i] = num;
        }       
    }

    void asignarPosiciones()
    {

        int num;

        for (int i = 0; i < pnjs.Length; i++)
        {
            do
            {
                num = Random.Range(0, maxMisionesJugables);
            } while (misionesIngame[idMisionesSeleccionadas[num]].listaPersonajes.Count >= 3);            
            misionesIngame[idMisionesSeleccionadas[num]].listaPersonajes.Add(pnjs[i]);
            pnjs[i].setMisionActual(idMisionesSeleccionadas[num]);
        }
    }

    /*index
     * LOGICA: PRENSA
     */
    [Header("Noticias")]
    [SerializeField]
    public Noticias[] noticiasIngame;




    //maxMisionesJugables
    //idMisionesSeleccionadas[i]

    //Resultados

    public int modificadorSeguidores;

    public void AddInfluencia( Personajes _pj)
    {
        _pj.influencia += _pj.seguidores;
    }

    public void CheckSeguidores()
    {
        //Si el resultado de la votacion es favorable a los intereses gana seguidores
    }

    public void BuyPowerUp(PowerupsName _pw)
    {


        if (precios[(int)_pw] <= jugador.influencia)
        {
            for (int i = 0; i < jugador.inventario.Count; i++)
            {
                if (jugador.inventario[i].pwrNombreEnum == _pw)
                {
                    
                    jugador.inventario[i].cantidad++;

                    return;
                }
            }

            Powerups powerUp = new Powerups();

            powerUp.pwrNombreEnum = _pw;

            powerUp.cantidad = 1;

            jugador.inventario.Add(powerUp);

        }
    }

    //AL FINAL NO LO NECESITAMOS
    //public Dictionary<Misiones, Noticias> mapaMisionesNoticias = new Dictionary<Misiones, Noticias>();

    //public void initMapa()
    //{
    //    for (int i = 0; i < misionesIngame.Length; i++)
    //    {
    //        mapaMisionesNoticias.Add(misionesIngame[i], noticiasIngame[i]);
    //    }
    //}



    public void initNoticias()
    {
        noticiasIngame = new Noticias[7];
    }

    /*index
     * FUNCIONES UNITY
     */

    // Instanciar GameManager
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }

    }

    private void Start()
    {
        for (int i = 0; i < misionesIngame.Length; i++)
        {
            noticiasIngame[i].titulo = misionesIngame[i].titulo;
        }
    }

}
