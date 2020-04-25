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
    Personajes jugador;
    Personajes[] pnjs;
    


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
    public int turnActual;
    public int turnMax;
    [Space(10)]
    [Range(0, 100)]
    public int involucionActual;
    [Range(0, 100)]
    public int involucionObjectivo;

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
    int[][] misionesPosiciones;

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
        misionesPosiciones;
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
