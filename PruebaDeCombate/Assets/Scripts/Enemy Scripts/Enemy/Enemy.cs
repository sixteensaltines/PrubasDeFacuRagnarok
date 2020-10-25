﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyAnims
{
    //TODO: FALTA UN CONTROLADOR DE VIDA! seguramente en la configuracion del enemigo vaya un public int con las vidas. 

    #region Constantes del enemigo
    [HideInInspector]
    public const float RANGOATAQUE = 2f;
    [HideInInspector]
    public const float RANGOSTUN = 1.2f;
    [HideInInspector]
    public float RangoVision = 11.5f;
    [HideInInspector]
    public const float DISTANCIA_ENTRADA_MODOGUARDIA = 5f;
    #endregion

    private bool esquiveActivado = false;

    public float MedidorDistancia(Vector3 ObjetoA, Vector3 ObjetoB)
    {
        return Vector3.Distance(ObjetoA, ObjetoB);
    }

    public void RotacionSkinEnemigo(Vector3 PlayerPosition)
    {
        if (transform.position.x < PlayerPosition.x) transform.eulerAngles = new Vector3(0, 0, 0);
        else transform.eulerAngles = new Vector3(0, 180, 0);   
    }


    #region ToolTip
    [Tooltip("Hasta donde camina cuando vaya hacia atras, para esquivar ataques del player")]
    #endregion
    public Transform HastaDondeCamina;//Hacia atras

    public void Caminata(Vector3 PlayerPosition, float MultiplicadorDeVelocidad, Animator anim)
    {
        //Control del mutiplicador, busca que sea 1 o mayor que 1
        if (MultiplicadorDeVelocidad < 0) MultiplicadorDeVelocidad = 1f;

        if (!esquiveActivado)
        {
            ControlDeVelocidades(MultiplicadorDeVelocidad, MedidorDistancia(PlayerPosition, transform.position));
            EjecutaMovimiento(PlayerPosition, MedidorDistancia(PlayerPosition, transform.position), anim);
        }
        else transform.position = Vector3.MoveTowards(transform.position, HastaDondeCamina.position, velocidadMovimiento * Time.deltaTime); //Esquiva
    }

    #region Variables de Movimiento hacia player
    private float velocidadMovimiento = 1f;
    private float velocidadMovimientoDefault;
    private bool haceFaltaIgualarVelocidades = true; //flag
    #endregion
    public void ControlDeVelocidades(float MultiplicadorDeVel,float distTotalAPlayer)
    {
        if (haceFaltaIgualarVelocidades)
        {
            haceFaltaIgualarVelocidades = false;
            velocidadMovimientoDefault = velocidadMovimiento * MultiplicadorDeVel;
        }

        if (distTotalAPlayer < DISTANCIA_ENTRADA_MODOGUARDIA)
        {
            if (velocidadMovimiento > velocidadMovimientoDefault * 0.9)
            {
                velocidadMovimiento = velocidadMovimiento - 0.01f * velocidadMovimientoDefault;
            }
        }
        else velocidadMovimiento = velocidadMovimientoDefault;
    }

    public void EjecutaMovimiento(Vector3 PlayerPosition, float distTotalAPlayer, Animator anim)
    {
        if (distTotalAPlayer <= RangoVision && distTotalAPlayer > RANGOATAQUE && DetectorSuelo_ConPlayer())
        {
            RangoVision = 50f;
            AnimCaminata(true); //Se cancela desde AnimEstatico()
            transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, velocidadMovimiento * Time.deltaTime);
        }
        if (distTotalAPlayer < RANGOATAQUE) AnimCaminata(false);
    }

    public void SaltoDePlataformas(Vector3 PlayerPosition, Rigidbody2D rbEnemigo, Animator anim)
    {
        EjecutaSalto(PlayerPosition, rbEnemigo, MedidorDistancia(PlayerPosition, transform.position));
        AnimacionSalto(anim);
    }

    #region Variables de lectura de rayo frontal
    #region Tooltip
    [Tooltip("El rayo frontal detectara las paredes que el player debe saltar, aun que no lo use el enemigo en concreto, es necesario ponerlo")]
    #endregion
    public Transform T_RayoFrontal;
    private float DistanciaRayo;
    #endregion
    public bool DeteccionPared_Frente(Vector3 PlayerPosition)
    {
        if (transform.position.x < PlayerPosition.x) DistanciaRayo = -2.3f;
        else DistanciaRayo = 2.3f;

        Vector2 FinalRayo = transform.position + Vector3.left * DistanciaRayo;

        Debug.DrawLine(transform.position, FinalRayo, Color.green);

        return Physics2D.Linecast(transform.position, FinalRayo, 1 << LayerMask.NameToLayer("Piso"));

    }

    public bool DeteccionPlayer_Frente()
    {
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z),
         T_RayoFrontal.position, Color.blue);

        return Physics2D.Linecast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z),
         T_RayoFrontal.position, 1 << LayerMask.NameToLayer("Player"));
    }

    #region Variables EjecutaSalto
    private float fuerzaSalto = 5f;
    #endregion
    public void EjecutaSalto(Vector3 PlayerPosition, Rigidbody2D rbEnemigo,float distTotalAPlayer)
    {   //Salto hacia adelante
        if (DeteccionPared_Frente(PlayerPosition) && !DeteccionPlayer_Frente() && (DetectorSuelo() || DetectorSuelo_ConPlayer()))
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, 7f * Time.deltaTime);
            rbEnemigo.velocity = Vector3.up * fuerzaSalto;   
        }

        //SaltoAlVacio
        if (!DeteccionPared_Frente(PlayerPosition) && !DeteccionPlayer_Frente() && distTotalAPlayer <= RANGOATAQUE)
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, velocidadMovimiento*2f * Time.deltaTime);
        }
    }

    #region Variables DetectorSuelo
    private float largoDelRayo = 0.9f;
    #endregion
    public bool DetectorSuelo()
    {
        Debug.DrawRay(transform.position, Vector2.down * largoDelRayo, Color.blue);
        RaycastHit2D ray;
        return ray = Physics2D.Raycast(transform.position, Vector2.down, largoDelRayo, LayerMask.GetMask("Piso"));
    }

    public bool DetectorSuelo_ConPlayer()
    {
        RaycastHit2D ray;
        return ray = Physics2D.Raycast(transform.position, Vector2.down, largoDelRayo, LayerMask.GetMask("PisoConPlayer"));
    }

    private float PosicionAnteriorY;
    void AnimacionSalto(Animator anim)
    {
        if ((DetectorSuelo() || DetectorSuelo_ConPlayer()))
        {
            PosicionAnteriorY = transform.position.y;
            AnimSalto(false);
            AnimCaida(false);
        }
        else if (!DetectorSuelo() && !DetectorSuelo_ConPlayer() && PosicionAnteriorY > transform.position.y)
        {
            AnimCaida(true);
            PosicionAnteriorY = transform.position.y;
        }
        else if (!DetectorSuelo()  && !DetectorSuelo_ConPlayer() && PosicionAnteriorY < transform.position.y)
        {

            AnimSalto(true);
            PosicionAnteriorY = transform.position.y;
        }
    }

    public void Stun()
    {

    }

    #region Tooltip
    [Tooltip("La probabilidad de bloqueo medira cuando el player ataque, dependiendo del % insertado, el enemigo tomara medidas o no, el % no debe superar el 100 %")]
    #endregion
    public int ProbabilidadBloqueoOcasional;
    private bool esPosibleBloquear= true;
    public void BloqueoOcasional(bool PlayerAtaca, Animator anim, Vector3 playerDistancia)
    {
        if (PlayerAtaca && esPosibleBloquear && MedidorDistancia(playerDistancia, transform.position) < RANGOATAQUE)
        {
            if (ProbabilidadBloqueoOcasional >= Random.Range(1, 100)) BloqueaAtaque(anim);
        }
    }
   void BloqueaAtaque(Animator anim)
    {
        esPosibleBloquear = false;
        AnimBloqueo_Ocasional(true);
        Invoke("In_CancelarBloqueoOcasional", Random.Range(0.9f, 2f));
    }

    void In_CancelarBloqueoOcasional()
    {
        esPosibleBloquear = true;
        AnimBloqueo_Ocasional(false);
    }

    ///////////////////////////////////////////////////////////////////////////

    public int QueAccion; //TODO:PASAR A PRIVATE 

    public int ProbabilidadAtaque;
    public int ProbabilidadBloqueo;
    public int ProbabilidadEsquive;
    public int ProbabilidadDashear;
    public int ProbabilidadDeEnemEstatico;
    public int ProbabilidadLanzarFlecha;

    private bool animacionEstatica;

    public string nuevaAccion; //TODO:PASAR A PRIVATE 
    private string viejaAccion;

    public float ContadorEntreAcciones;
    private bool bloquearBuscador_Acciones;

    private bool Flag1;

    /// <summary>
    /// Activa el modo combate para el enemigo, teniendo asi en cuenta cada uno de los factores de la pelea, algunos cambios tendran que ajustarse desde el inspector.
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="MinTiempoEntreAcciones"></param>
    /// <param name="MinTiempoEntreAcciones_Default"></param>
    public void ModoCombate(Animator anim, float MinTiempoEntreAcciones, float distTotalAPlayer)
    {
            if (Flag1)
            {
                Flag1 = !Flag1;
                ContadorEntreAcciones = MinTiempoEntreAcciones;
            }

            if (!animacionEstatica) //Se activa desde las animaciones. 
            {
                if (distTotalAPlayer <= RANGOATAQUE)
                {
                     if (!bloquearBuscador_Acciones)
                     {
                            if (ContadorEntreAcciones >= 0) ContadorEntreAcciones -= Time.deltaTime;
                            else
                            {
                                nuevaAccion = SeleccionaAccionAleatoria();

                                //buscarNuevaAccion por defecto es false
                                   if (buscarNuevaAccion && nuevaAccion != viejaAccion) Restaurar_ValoresDeteccionDeRepeticiones();

                                  if (nuevaAccion == viejaAccion)
                                  {
                                       CantidadDeRepeticionPosible();
        
                                       if (!buscarNuevaAccion)
                                       {
                                            bloquearBuscador_Acciones = true;
                                            ActivarAnimacion(nuevaAccion, anim);
                                            ContadorEntreAcciones = MinTiempoEntreAcciones;
                                       }
                                       //else vuelve a tirar nueva accion, lo hara de forma automatica
                                  }
                                  else
                                  {
                                        viejaAccion = nuevaAccion; //Obtiene nuevo valor para la proxima vuelta

                                        bloquearBuscador_Acciones = true;   
                                        ActivarAnimacion(nuevaAccion, anim);
                                        ContadorEntreAcciones = MinTiempoEntreAcciones;
                                  }
                            }
                }

                }
            }
            else
            {
                ActivarAnimacion("Estatico", anim);
                animacionEstatica = false;
                bloquearBuscador_Acciones = false;
            }
    }

    /// <summary>
    /// Selecciona dos rangos, max y min, busca la posicion del objeto entre esos rangos, devuelve un booleano
    /// </summary>
    /// <param name="RangoMax"></param>
    /// <param name="RangoMin"></param>
    /// <param name="PosicionObjetoAEstudiar"></param>
    /// <returns></returns>
    bool BuscaObjetoEntre_Dos_Puntos(float RangoMax, float RangoMin, float PosicionObjetoAEstudiar)
    {
        if (RangoMax > PosicionObjetoAEstudiar && PosicionObjetoAEstudiar > RangoMin) return true;
        else return false;
    }

    /// <summary>
    /// Devuelve una accion por medio de un numero random. El valor sera devuelto segun el porcentaje de posibilidades que uno designe dentro del inspector. LA SUMA SIEMPRE DEBE DAR 100% 
    /// </summary>
    /// <returns></returns>
    string SeleccionaAccionAleatoria()
    {
        QueAccion = Random.Range(1, 100);

        if (QueAccion <= ProbabilidadAtaque)
        { return "Ataque"; }

        else if (QueAccion <= ProbabilidadAtaque + ProbabilidadEsquive)
        { return "Esquive"; }

        else if (QueAccion <= ProbabilidadAtaque + ProbabilidadEsquive + ProbabilidadBloqueo)
        { return "Bloqueo"; }

        else if (QueAccion <= ProbabilidadAtaque + ProbabilidadEsquive + ProbabilidadBloqueo + ProbabilidadDeEnemEstatico)
        { return "Estatico"; }

        else if (QueAccion <= ProbabilidadAtaque + ProbabilidadEsquive + ProbabilidadBloqueo + ProbabilidadDeEnemEstatico + ProbabilidadLanzarFlecha)
        { return "Flechazo"; }

        else if (QueAccion <= ProbabilidadAtaque + ProbabilidadEsquive + ProbabilidadBloqueo + ProbabilidadDeEnemEstatico + ProbabilidadLanzarFlecha + ProbabilidadDashear)

        { return "Dashear"; }

        else
        {
            Debug.Log("VALOR IMPOSIBLE, AJUSTE LOS VALORES PARA QUE JUNTOS SUMEN  100% DE PROBABILIDADES");
            return "VALOR IMPOSIBLE";
        }
    }

    //Automaticamente se sumara uno al numero seleccionado. 
    private int cantRepAtaqueDefault = 2; //+1
    private int cantRepAtaque;

    private int cantRepBloqueoDefault = 1; //+1
    private int cantRepBloqueo;

    private bool buscarNuevaAccion;

    void CantidadDeRepeticionPosible()
    {
        if (cantRepAtaque == 0 && cantRepBloqueo == 0) //Iguala valores default en primera vuelta 
        {
            cantRepAtaque = cantRepAtaqueDefault;
            cantRepBloqueo = cantRepBloqueoDefault;
        }

        switch (nuevaAccion)
        {
            case "Estatico":
                buscarNuevaAccion = true;
                break;

            case "Ataque":
                if (cantRepAtaque > 0)
                {
                    cantRepAtaque--;
                    buscarNuevaAccion = false;
                }
                else buscarNuevaAccion = true;
                break;

            case "Bloqueo":
                if (cantRepBloqueo > 0)
                {
                    cantRepBloqueo--;
                    buscarNuevaAccion = false;
                }
                else buscarNuevaAccion = true;
                break;
            case "Esquive":
                buscarNuevaAccion = true;
                break;

                //CASES
                //else if (TipoDeAcccion == "Flechazo");
                //else if (TipoDeAcccion == "Dashear");
        }
    }
    void Restaurar_ValoresDeteccionDeRepeticiones()
    {
        cantRepBloqueo = cantRepBloqueoDefault;
        cantRepAtaque = cantRepAtaqueDefault;
    }

    //ANIMACIONES

    void ActivarAnimacion(string Accion, Animator anim)
    {
        if (Accion == "Bloqueo")
        {
            AnimBloqueo_Random(true);
        }
        else if (Accion == "Ataque")
        {
            AnimAtaque(1, Random.Range(1, 3));
            
        }
        else if (Accion == "Esquive")
        {
            AnimEsquive(EsPosibleEsquivar());
            if (EsPosibleEsquivar())
            {
                esquiveActivado = true; //No permite leer otras acciones.
                AnimCaminata(false);
                AnimEsquive(true);
            }
        }
        else if (Accion == "Estatico")
        {
            AnimEstatico();
            animacionEstatica = true;
        }
        //FALTA AGREGAR EL RESTO DE ANIMACIONES
    }

    public Transform T_Rayo_Espalda;
    bool EsPosibleEsquivar()
    {   
        return Physics2D.Linecast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z),
        T_Rayo_Espalda.position, 1 << LayerMask.NameToLayer("Piso"));
    }


    public void CancelarCaminata_Atras()
    {
        animacionEstatica = true;
        esquiveActivado = false;
    }//cuando termina la animacion deja de caminar activando este metodo

    public void MantieneBloqueo() => Invoke("ActivarAnimacionEstatico", Random.Range(1f, 2f));   //Se activa desde las animaciones 

    void ActivarAnimacionEstatico() => animacionEstatica = true; //Se activa desde animaciones 
}