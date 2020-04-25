using System.Collections;
using System.Collections.Generic;
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

public struct Powerups
{
    int pwrCensura;
    int pwrPublicidad;
    int pwrRevelar;
    int pwrEspia;
}


public class GameManager : MonoBehaviour
{

    /*index
     * VARIABLES DE JUEGO
     */  
    int maxMisionesJugables = 3;
    public Personajes jugador;
    Personajes[] pnjs;
    Image[] avatares;
    bool[] avataresControl = new bool[] { false };
    bool[] rolControl = new bool[] { false };

    //VALUES PJS
    int randRol;
    int randAvatar;
    int influencia;
    int apoyos;
    int seguidores;

    void InitGame()
    {
        sociedadActual = Random.Range(80, 101);
        economiaActual = Random.Range(80, 101);
        desarrolloActual = Random.Range(80, 101);
        involucionActual = sociedadActual + economiaActual + desarrolloActual;
        involucionObjectivo = 100;

        turnActual = 0;
        turnMax = 12;

        SetValuesPjs();

        jugador = new Personajes((RolSecreto)randRol, avatares[randAvatar], influencia, apoyos, seguidores, false);
        avataresControl[randAvatar] = true;
        rolControl[randRol] = true;

        for (int i = 0; i < pnjs.Length; i++)
        {
            SetValuesPjs();

            pnjs[i] = new Personajes((RolSecreto)randRol, avatares[randAvatar], influencia, apoyos, seguidores, true);
            avataresControl[randAvatar] = true;
            rolControl[randRol] = true;
        }

        //TODO: valores de juego objetivo


    }

    void SetValuesPjs()
    {
        randRol = Random.Range(0, (int)RolSecreto.TOTAL_ROLES);
        do
        {
            randAvatar = Random.Range(0, avatares.Length);
        } while (avataresControl[randAvatar]);
        avataresControl[randAvatar] = true;

        influencia = Random.Range(100, 300);
        apoyos = Random.Range(1, 3);
        seguidores = Random.Range(1, 3);
    }

    /*index
     * ESTADISTICAS DE JUEGO
     */

    [Header("EstadisticasDeJuego")]
    [Range(0, 100)]
    public int sociedadActual;
    [Range(0, 100)]
    public int economiaActual;
    [Range(0, 100)]
    public int desarrolloActual;
    [Space(10)]
    [Range(0, 100)]
    public int sociedadObjetivo;
    [Range(0, 100)]
    public int economiaObjetivo;
    [Range(0, 100)]
    public int desarrolloObjetivo;  
    [Space(10)]
    [Range(0, 100)]
    public int involucionActual;
    [Range(0, 100)]
    public int involucionObjectivo;

    int turnActual;
    int turnMax;

    public int calcularInvolucion()
    {
        int num = 0;
        
        //TODO: Calcular la involucion;

        return num;
    }


    /*index
     * CANVAS: MISIONES
     */

    [Header("Misiones")]
    [SerializeField]
    public Misiones[] misionesIngame;

    int[] idMisionesSeleccionadas; //Tamaño 3, las 3 misiones que pueden salir. Guardamos el ID de la mision.

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
                if (control >= 100000) { controlFail = true; }
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
            } while (misionesIngame[idMisionesSeleccionadas[num]].listaPersonajes.Count == 3);

            misionesIngame[idMisionesSeleccionadas[num]].listaPersonajes.Add(pnjs[i]);
            pnjs[i].setMisionActual(idMisionesSeleccionadas[num]);

        }

    }
    


    /*index
     * CANVAS: PRENSA
     */
    [Header("Noticias")]
    [SerializeField]
    public Noticias[] noticiasIngame;

    public Dictionary<Misiones, Noticias> mapaMisionesNoticias = new Dictionary<Misiones, Noticias>();

    public void initMapa()
    {
        for (int i = 0; i < misionesIngame.Length; i++)
        {
            //mapaMisionesNoticias.Add(misionesIngame[i], noticiasIngame[i]);
        }
    }
    


    public void initNoticias()
    {
        noticiasIngame = new Noticias[7];

        //El personaje ya está en una misión 
        
        //TODO
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

}
