using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

public class GameManager : MonoBehaviour
{

    /*index
     * VARIABLES DE JUEGO
     */

    int[] rand; //Tamaño 3, las 3 misiones que pueden salir.

    int[][] misionesPosiciones;

    misionesPosiciones[0][0] = 2; //Mision 1, posicion 1 = jugador 2
    /*index
     * ESTADISTICAS DE JUEGO
     */

    public int sociedad;
    public int economia;
    public int desarrollo;

    public int sociedadObjetivo;
    public int economiaObjetivo;
    public int desarrolloObjetivo;

    public int turn;
    public int turnMax;

    public int involucion;

    public int calcularInvolucion()
    {
        int num = 0;
        
        //TODO: Calcular la involucion;

        return num;
    }


    /*index
     * CANVAS: MISIONES
     */

    public Misiones[] misionesIngame;
    Personajes personajesIngame;

    

    /*index
     * CANVAS: PRENSA
     */

    public Noticias[] noticiasIngame;

    public Dictionary<Misiones, Noticias> mapaMisionesNoticias = new Dictionary<Misiones, Noticias>();

    public void initMapa()
    {
        for (int i = 0; i < misionesIngame.Length; i++)
        {
            mapaMisionesNoticias.Add(misionesIngame[i], noticiasIngame[i]);
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
