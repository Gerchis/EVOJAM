using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Personajes
{
    Personajes()
    {

    }

    public Personajes(RolSecreto _rol, Image _avatar, int _influencia, int _apoyos, int _seguidores, bool _ia)
    {
        avatar.sprite = _avatar.sprite;
        influencia = _influencia;
        apoyos = _apoyos;
        seguidores = _seguidores;
        ia = _ia;
        personajeInvestigado = false;

        switch (_rol)
        {
            case RolSecreto.COMUNISTA:
            afinidadesEstadisticas[0] = true;
            afinidadesEstadisticas[1] = false;
            afinidadesEstadisticas[2] = false;
            break;
            case RolSecreto.LIBERAL:
            afinidadesEstadisticas[0] = false;
            afinidadesEstadisticas[1] = true;
            afinidadesEstadisticas[2] = false;
            break;
            case RolSecreto.CIENTIFICO:
            afinidadesEstadisticas[0] = false;
            afinidadesEstadisticas[1] = false;
            afinidadesEstadisticas[2] = true;
            break;
            case RolSecreto.EMPRENDEDOR:
            afinidadesEstadisticas[0] = true;
            afinidadesEstadisticas[1] = true;
            afinidadesEstadisticas[2] = false;
            break;
            case RolSecreto.SOCIALISTA:
            afinidadesEstadisticas[0] = true;
            afinidadesEstadisticas[1] = false;
            afinidadesEstadisticas[2] = true;
            break;
            case RolSecreto.CAPITALISTA:
            afinidadesEstadisticas[0] = false;
            afinidadesEstadisticas[1] = true;
            afinidadesEstadisticas[2] = true;
            break;
            case RolSecreto.FILANTROPO:
            afinidadesEstadisticas[0] = true;
            afinidadesEstadisticas[1] = true;
            afinidadesEstadisticas[2] = true;
            break;
            case RolSecreto.CAOTICO:
            afinidadesEstadisticas[0] = false;
            afinidadesEstadisticas[1] = false;
            afinidadesEstadisticas[2] = false;
            break;
        }

        if (!ia)
        {
            if (afinidadesEstadisticas[0])
            {
                GameManager.Instance.sociedadObjetivo = 33;
                //COLORES slider positivo/negativo
            }
            else
            {
                GameManager.Instance.sociedadObjetivo = -1;
            }

            if (afinidadesEstadisticas[1])
            {
                GameManager.Instance.economiaObjetivo = 33;
            }
            else
            {
                GameManager.Instance.economiaObjetivo = -1;
            }

            if (afinidadesEstadisticas[2])
            {
                GameManager.Instance.desarrolloObjetivo = 33;
            } else
            {
                GameManager.Instance.desarrolloObjetivo = -1;
            }
        }

    }

    ~Personajes()
    {

    }

    //ATRIBUTOS GENERALES
    public Image avatar;
    RolSecreto rol;
    //Revisar orden de afinidades en GameManager > public enum Estadisticas;
    bool[] afinidadesEstadisticas;

    public bool[] getAfinidadesEstadisticas()
    {
        return afinidadesEstadisticas;
    }


    //ATRIBUTOS GENERALES IA
    bool ia;
    bool personajeInvestigado;

    public bool getPersonajeInvestigado() { return personajeInvestigado; }
    public void setPersonajeInvestigado(bool _b) { personajeInvestigado = _b; }

    //RECURSOS
    public int seguidores;
    public int apoyos;
    public int influencia;

    //POWERUPS

    GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    public List<Powerups> inventario;

    public bool VerificarDisponibilidad(PowerupsName pwrNombre)
    {
        for (int i = 0; i < inventario.Count; i++)
        {
            if (pwrNombre == inventario[i].pwrNombreEnum)
            {
                return true;
            }
        }
        return false;
    }

    public void ConsumirPowerup(PowerupsName pwrNombre)
    {
        for (int i = 0; i < inventario.Count; i++)
        {
            if (pwrNombre == inventario[i].pwrNombreEnum)
            {
                inventario[i].cantidad--;

                if (inventario[i].cantidad <= 0)
                {
                    inventario.RemoveAt(i);
                }
            }
        }
    }


    public bool powerupActivo(PowerupsName pwrNombre, int _value)
    {
        for (int k = 0; k<gm.jugador.inventario.Count; k++)
        {
            if (gm.jugador.inventario[k].pwrNombreEnum == pwrNombre && gm.jugador.inventario[k].value == _value)
            {
                //Desactivamos powerup
                gm.jugador.inventario[k].value = -1;
      
                return true;
            }
        }
        return false;
    }

        

    //IA
    int misionActual;
    bool votacion;

    public void setMisionActual(int _idMision)
    {
        misionActual = _idMision;
    }



    //La IA escoge si el voto para cumplir la misión es Si o No.
    bool decidirVoto()
    {
        int intencionVoto = 0;

        for (int i = 0; i < (int)Estadisticas.TOTAL_ESTADISTICAS; i++)
        {
            int valorEstadistica = GameManager.Instance.noticiasIngame[misionActual].efectosNoticia[i].valor;

            if (afinidadesEstadisticas[i])
            {
                intencionVoto += valorEstadistica;

            }
            else
            {
                intencionVoto -= valorEstadistica;
            }
        }

        if (intencionVoto > 0)
        {
            return true;
        }
        else if (intencionVoto < 0)
        {
            return false;
        }
        else //Si intención de voto == 0: RANDOM
        {
            return (Random.Range(0, 2) == 1);
        }
    }

    public int getApoyosRAW()
    {
        return apoyos;
    }

    public int getApoyos()
    {
        if (ia)
        {
            SetVotacion();
        }
        
        
        if (votacion)
        {
            return apoyos;
        }
        else
        {
            return -apoyos;
        }
    }

    void SetVotacion()
    {
        votacion = decidirVoto();
    }

    public void SetVotacion(bool _voto)
    {
        votacion = _voto;
    }
}
