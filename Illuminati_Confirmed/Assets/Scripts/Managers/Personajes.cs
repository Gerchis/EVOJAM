using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Personajes
{
    Personajes()
    {

    }

    Personajes(RolSecreto _rol)
    {
        switch (_rol)
        {
            case RolSecreto.COMUNISTA:
                
            break;
            case RolSecreto.LIBERAL:
            break;
            case RolSecreto.CIENTIFICO:
            break;
            case RolSecreto.EMPRENDEDOR:
            break;
            case RolSecreto.SOCIALISTA:
            break;
            case RolSecreto.CAPITALISTA:
            break;
            case RolSecreto.FILANTROPO:
            break;
            case RolSecreto.CAOTICO:
            break;
            case RolSecreto.TOTAL_ROLES:
            break;
        }
    }

    ~Personajes()
    {

    }

    //ATRIBUTOS
    Image avatar;
    RolSecreto rol;


    //RECURSOS
    int seguidores;
    int apoyos;
    int influencia;

    //objetivos
    
    /*
     * Revisar orden de afinidades en GameManager > public enum Estadisticas;
     */
    bool[] afinidadesEstadisticas;


    void initAfinidades()
    {
        

    }

    //IA
    int misionActual;
    int intencionVoto;

    bool decidirVoto()
    {
        intencionVoto = 0;

        for (int i = 0; i < (int)Estadisticas.TOTAL_ESTADISTICAS; i++)
        {
            int valorEstadistica = GameManager.Instance.noticiasIngame[misionActual].efectosNoticia[i].valor;

            if(afinidadesEstadisticas[i])
            {
                intencionVoto += valorEstadistica;
                
            } else
            {
                intencionVoto -= valorEstadistica;
            }
        }

        if(intencionVoto>0)
        {
            return true;
        } else if (intencionVoto<0)
        {
            return false;
        } else //RANDOM
        {
            return (Random.Range(0, 2) == 1);
        }

        
    }
    


}
