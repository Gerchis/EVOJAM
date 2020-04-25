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

public class GameManager : MonoBehaviour
{
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

    Misiones[] misionesIngame;

    

    /*index
     * CANVAS: PRENSA
     */

    Noticias[] noticiasIngame;

    Dictionary<Misiones, Noticias> mapaMisionesNoticias = new Dictionary<Misiones, Noticias>();

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
