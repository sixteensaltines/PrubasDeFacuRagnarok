using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyAnims
{
    #region Constantes del enemigo
    [HideInInspector]
    public const float RANGOATAQUE = 2.1f;
    [HideInInspector]
    public float RangoVision = 11.5f;
    [HideInInspector]
    public const float DISTANCIA_ENTRADA_MODOGUARDIA = 5f;

    private Vector3 v_PosicionPlayer;
    private Rigidbody2D RigidBodyEnemigo;
    #endregion

    #region Variables De Distancia
    #endregion
    public float MedidorDistancia(Vector3 ObjetoA, Vector3 ObjetoB)
    {
        return Vector3.Distance(ObjetoA, ObjetoB);
    }

    public void RotacionSkinEnemigo(Vector3 PlayerPosition)
    {
        if (transform.position.x < PlayerPosition.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    #region Variables DetectorSuelo
    private float radio = 0.34f;
    private LayerMask layerPiso;
    private bool estaSuelo;
    private bool esperarSuCaida;
    #endregion
    public void DetectorSuelo()
    {
        layerPiso = LayerMask.GetMask("Piso");
        estaSuelo = Physics2D.OverlapCircle(transform.GetChild(1).GetChild(2).position, radio, layerPiso);

        //ReactivarSalto_ParaUnaProximaVez
        if (esperarSuCaida && estaSuelo)
        {
            esperarSuCaida = false;
            estaSaltando = false;
        }
        if (!estaSuelo && estaSaltando)
        {
            esperarSuCaida = true;
        }
    }

    #region Variable Seguimiento Player
    private float multiplicadorDeVelocidad;
    #endregion
    public void ControladorSeguimientoPlayer(Vector3 PlayerPosition, float MultiplicadorDeVelocidad)
    {
        v_PosicionPlayer = PlayerPosition;

        //Control del mutiplicador, busca que sea 1 o mayor que 1
        if (MultiplicadorDeVelocidad > 0)
        {
            multiplicadorDeVelocidad = MultiplicadorDeVelocidad;
        }
        else
        {
            multiplicadorDeVelocidad = 1f;
        }

            ControlDeVelocidades(multiplicadorDeVelocidad, MedidorDistancia(PlayerPosition, transform.position));
            EjecutaMovimiento(v_PosicionPlayer, MedidorDistancia(PlayerPosition, transform.position));
    }

    #region Variables de Movimiento hacia player
    private float velocidadMovimiento = 1f;
    private float velocidadMovimientoDefault;
    private bool haceFaltaIgualarVelocidades = true;
    #endregion
    public void ControlDeVelocidades(float MultiplicadorDeVel,float distTotalAPlayer)
    {
        if (haceFaltaIgualarVelocidades)
        {
            haceFaltaIgualarVelocidades = false;
            velocidadMovimientoDefault = velocidadMovimiento * MultiplicadorDeVel;

        }

        if (!estaSaltando)
        {
            if (distTotalAPlayer < DISTANCIA_ENTRADA_MODOGUARDIA)
            {

                if (velocidadMovimiento > velocidadMovimientoDefault * 0.9)
                {
                    velocidadMovimiento = velocidadMovimiento - 0.01f * velocidadMovimientoDefault;
                }
            }
            else
            {
                velocidadMovimiento = velocidadMovimientoDefault;
            }
        }
    }

    public void EjecutaMovimiento(Vector3 PlayerPosition, float distTotalAPlayer)
    {
        if (distTotalAPlayer <= RangoVision && distTotalAPlayer >= RANGOATAQUE)
        {
            RangoVision = 20f;
            transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, velocidadMovimiento * Time.deltaTime);
        }
    }

    public void Salto(Vector3 PlayerPosition, Rigidbody2D rbEnemigo)
    {
        v_PosicionPlayer = PlayerPosition;
        RigidBodyEnemigo = rbEnemigo;

        LecturaDeEntornoConRayo_Frente(v_PosicionPlayer);
        EjecutaSalto(v_PosicionPlayer, RigidBodyEnemigo, MedidorDistancia(PlayerPosition, transform.position));
        DetectorSuelo();
    }

    #region Variables de lectura de rayo frontal
    public Transform T_RayoFrontal;
    private float DistanciaRayo;
    private RaycastHit2D obstaculoFrontal;
    private RaycastHit2D deteccionPlayer_Frente;
    #endregion
    public void LecturaDeEntornoConRayo_Frente(Vector3 PlayerPosition)
    {
        if (transform.position.x < PlayerPosition.x)
            DistanciaRayo = -2.3f;
        else DistanciaRayo = 2.3f;

        Vector2 FinalRayo = transform.position + Vector3.left * DistanciaRayo;

        obstaculoFrontal = Physics2D.Linecast(transform.position, FinalRayo, 1 << LayerMask.NameToLayer("Piso"));

        deteccionPlayer_Frente = Physics2D.Linecast(new Vector3(transform.position.x, transform.position.y+ 1f,transform.position.z),
            T_RayoFrontal.position, 1 << LayerMask.NameToLayer("Player"));
    }

    #region Variables EjecutaSalto
    private bool estaSaltando;
    private float fuerzaSalto = 5f;
    #endregion
    public void EjecutaSalto(Vector3 PlayerPosition, Rigidbody2D rbEnemigo,float distTotalAPlayer)
    {   //Salto hacia adelante
        if (obstaculoFrontal.collider != null && deteccionPlayer_Frente.collider == null && estaSuelo)
        {
            estaSaltando = true; //El DetectorDeSuelo() va a volver a detectar cuando cae
            transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, 7f * Time.deltaTime);
            rbEnemigo.velocity = Vector3.up * fuerzaSalto;   
        }

        //SaltoAlVacio
        if (obstaculoFrontal.collider == null && deteccionPlayer_Frente.collider == null)
        {
            if (distTotalAPlayer <= RANGOATAQUE)
            {
                transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, velocidadMovimiento*2f * Time.deltaTime);
            }
        }
    }

    public void Stun(float distTotalAPlayer)
    {
        if (deteccionPlayer_Frente.collider != null)
        {
            if (distTotalAPlayer < 1.5f)
            {
                //En_Movimiento.Stuneado = true;   //TODO: Acordarse del movimiento y el stun.   
            }
        }
    }//TODO: Falta agregar el stun en algun lado 

    //TODO: FALTA UN CONTROLADOR DE VIDA! seguramente en la configuracion del enemigo vaya un public int con las vidas. 

    
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
            if (BuscaObjetoEntre_Dos_Puntos(RANGOATAQUE + 0.3f, RANGOATAQUE - 0.2f, distTotalAPlayer))
            {
                if (ContadorEntreAcciones >= 0)
                {
                    ContadorEntreAcciones -= Time.deltaTime;
                }
                else
                {
                    nuevaAccion = SeleccionaAccionAleatoria();

                    if (buscarNuevaAccion && nuevaAccion != viejaAccion) Restaurar_ValoresDeteccionDeRepeticiones();

                    if (nuevaAccion == viejaAccion)
                    {
                        CantidadDeRepeticionPosible();

                        if (!buscarNuevaAccion)
                        {
                            DesativarAnimacion("Estatico", anim);
                            ActivarAnimacion(nuevaAccion, anim);
                            ContadorEntreAcciones = MinTiempoEntreAcciones;
                        }
                        //else vuelve a tirar nueva accion, lo hara de forma automatica
                    }
                    else
                    {
                        viejaAccion = nuevaAccion; //Obtiene nuevo valor para la proxima vuelta

                        DesativarAnimacion("Estatico", anim);
                        ActivarAnimacion(nuevaAccion, anim);
                        ContadorEntreAcciones = MinTiempoEntreAcciones;
                    }
                }
            }
        }
        else
        {
            DesativarAnimacion(anim);
            animacionEstatica = false;
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

                //CASES
                //else if (TipoDeAcccion == "Flechazo");
                //else if (TipoDeAcccion == "Esquive");
                //else if (TipoDeAcccion == "Dashear");
        }
    }
    void Restaurar_ValoresDeteccionDeRepeticiones()
    {
        cantRepBloqueo = cantRepBloqueoDefault;
        cantRepAtaque = cantRepAtaqueDefault;
    }

    //ANIMACIONES
    public void MantieneBloqueo() => Invoke("ActivarAnimacionEstatico", Random.Range(1f, 2f));   //Se activa desde las animaciones 

    void ActivarAnimacionEstatico() => animacionEstatica = true; //Se activa desde animaciones 

    void ActivarAnimacion(string Accion, Animator anim)
    {
        if (Accion == "Bloqueo") AnimBloqueo(anim,true);
        else if (Accion == "Ataque") AnimAtaque(anim,true);
        else if (Accion == "Estatico") AnimEstatico(anim,true);
    }
    void DesativarAnimacion(string Accion, Animator anim)
    {
        if (Accion == "Bloqueo") AnimBloqueo(anim, false);
        else if (Accion == "Ataque") AnimAtaque(anim, false);
        else if (Accion == "Estatico") AnimEstatico(anim, false);
    }
    /// <summary>
    /// Desactiva Todas las animaciones y deja por defecto encendido la animacion de estatico
    /// </summary>
    /// <param name="anim"></param>
    void DesativarAnimacion(Animator anim)
    {
        AnimBloqueo(anim, false);
        AnimAtaque(anim, false);
        AnimEstatico(anim, true);
    }
}