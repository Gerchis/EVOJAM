﻿using System.Collections;
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
    }

    ~Personajes()
    {

    }

    //ATRIBUTOS
    public Image avatar;
    RolSecreto rol;
    //Revisar orden de afinidades en GameManager > public enum Estadisticas;
    bool[] afinidadesEstadisticas;
    bool ia;

    //RECURSOS
    int seguidores;
    int apoyos;
    int influencia;

    //POWERUPS
    Powerups inventario;

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
