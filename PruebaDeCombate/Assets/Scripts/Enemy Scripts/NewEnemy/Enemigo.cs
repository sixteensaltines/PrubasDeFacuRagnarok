using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : AnimacionesEnemigos
{
    //Layers
    [HideInInspector]
    public LayerMask LayerPlayer = 12;
    [HideInInspector]
    public LayerMask LayerPlayerBloqueando = 14;
    [HideInInspector]
    public LayerMask LayerEnemigo = 13;
    [HideInInspector]
    public LayerMask LayerEnemigoBloqueando = 15;

    public GameObject CollidersEnemigo;

    private float rangoVision = 11.5f;
    private const float RANGOATAQUE = 2.3f;

    public void CaminataAPlayer(Vector3 PlayerPosition)
    {
        RotacionSkinEnemigo(PlayerPosition);
        EjecutaMovimiento(PlayerPosition);
    }

    private bool bloqueaRotacion;
    public void RotacionSkinEnemigo(Vector3 PlayerPosition)
    {
        if (!bloqueaRotacion)
        {
            if (transform.position.x < PlayerPosition.x) transform.eulerAngles = new Vector3(0, 0, 0);
            else transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private string AQueLadoMira()
    {
        if (transform.eulerAngles.y == 180) return "izquierda";
        else return "derecha";
    }

    public float VelocidadMovimiento;
    void EjecutaMovimiento(Vector3 PlayerPosition)
    {
        if (Vector3.Distance(transform.position, PlayerPosition) < rangoVision &&
            Vector3.Distance(transform.position, PlayerPosition) > RANGOATAQUE &&
            !DetectaSuelo("Piso")) //Si el layer es piso, deja de caminar!
        {
            AnimCaminata(true);
            rangoVision = 50f;
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(PlayerPosition.x, transform.position.y),
            VelocidadMovimiento * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, PlayerPosition) < RANGOATAQUE || !DetectaSuelo("PisoConPlayer")) AnimCaminata(false);
    }

    public void SaltoDePlataformas(Vector3 PlayerPosition, Rigidbody2D rbEnemigo)
    {
        EjecutaSalto(PlayerPosition, rbEnemigo);
        AnimacionDeSalto();
    }

    private float largoRayo_AlPlayer = 2.86f;
    private float largoRayo_APared = 2.3f;
    private float fuerzaSalto = 5f;
    void EjecutaSalto(Vector3 PlayerPosition, Rigidbody2D rbEnemigo)
    {
        //Actualizo los rayos con la posicion del player
        if (PlayerPosition.x > transform.position.x) largoRayo_AlPlayer = 2.86f;
        else largoRayo_AlPlayer = -2.86f;

        if (transform.position.x < PlayerPosition.x) largoRayo_APared = 2.3f;
        else largoRayo_APared = -2.3f;

        if (DetectaPared_Delante() && !DetectaPlayer_Delante() && DetectaSuelo())
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, 7f * Time.deltaTime);
            rbEnemigo.velocity = Vector3.up * fuerzaSalto;
        }
        if (!DetectaPared_Delante() && !DetectaPlayer_Delante() && Vector3.Distance(transform.position, PlayerPosition) < RANGOATAQUE)
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, VelocidadMovimiento * 2f * Time.deltaTime);
        }
    }

    private float PosicionAnteriorY;
    void AnimacionDeSalto()
    {
        if (DetectaSuelo())
        {
            PosicionAnteriorY = transform.position.y;
            AnimSalto(false);
            AnimCaida(false);
        }
        else if (!DetectaSuelo() && PosicionAnteriorY > transform.position.y)
        {
            AnimCaida(true);
            PosicionAnteriorY = transform.position.y;
        }
        else if (!DetectaSuelo() && PosicionAnteriorY < transform.position.y)
        {
            AnimSalto(true);
            PosicionAnteriorY = transform.position.y;
        }
    }

    private const float LARGORAYO_ALSUELO = 0.9f;

    /// <summary>
    /// El layer puede ser "Piso" o "PisoConPlayer"
    /// </summary>
    /// <param name="QueSuelo"></param>
    /// <returns></returns>
    public bool DetectaSuelo(string QueSuelo = "Null")
    {
        RaycastHit2D ray;

        if (QueSuelo == "Null") //Por defecto busca los dos
        {
            ray = Physics2D.Raycast(transform.position, Vector2.down, LARGORAYO_ALSUELO, 1 << LayerMask.NameToLayer("Piso"));
            if (ray == true) return true;
            else ray = Physics2D.Raycast(transform.position, Vector2.down, LARGORAYO_ALSUELO, 1 << LayerMask.NameToLayer("PisoConPlayer"));
            if (ray == true) return true;
            else return false;
        }
        else //Si se especifica un layer, busca ese solo! 
        {
            return ray = Physics2D.Raycast(transform.position, Vector2.down, LARGORAYO_ALSUELO, 1 << LayerMask.NameToLayer(QueSuelo));
        }
    }

    /// <summary>
    /// El layer puede ser "Player" o "PlayerBloqueando"
    /// </summary>
    /// <param name="QuePlayer"></param>
    /// <returns></returns>
    /// 
    public bool DetectaPlayer_Delante(string QuePlayer = "Null")
    {
        RaycastHit2D ray;

        if (QuePlayer == "Null") //Por defecto busca los dos
        {
            ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f),
                Vector2.right, largoRayo_AlPlayer, 1 << LayerMask.NameToLayer("Player"));
            if (ray == true) return true;
            else ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f),
                Vector2.right, largoRayo_AlPlayer, 1 << LayerMask.NameToLayer("PlayerBloqueando"));
            if (ray == true) return true;
            else return false;
        }
        else //Si se especifica un layer, busca ese solo! 
        {
            return ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f),
                Vector2.right, largoRayo_AlPlayer, 1 << LayerMask.NameToLayer(QuePlayer));
        }
    }

    public bool DetectaPared_Delante()
    {
        RaycastHit2D ray;
        return ray = Physics2D.Raycast((transform.position), Vector2.right, largoRayo_APared, 1 << LayerMask.NameToLayer("PisoConPlayer"));
    }

    public void DibujaRayos()
    {
        //Para detectar Suelo
        DibujaRayo(transform.position, Vector2.down * LARGORAYO_ALSUELO);

        //DetectaPlayer
        DibujaRayo(new Vector2(transform.position.x, transform.position.y + 1f), Vector2.right * largoRayo_AlPlayer);

        //DetectaPared
        DibujaRayo((transform.position), Vector2.right * largoRayo_APared);
    }

    void DibujaRayo(Vector3 Desde, Vector3 Hasta)
    {
        Debug.DrawRay(Desde, Hasta, Color.blue);
    }

    //NEWSCRIPT///////////////////////////////////////////

    #region Tooltip
    [Tooltip("La probabilidad de bloqueo medira cuando el player ataque, dependiendo del % insertado, el enemigo tomara medidas o no, el % no debe superar el 100 %")]
    #endregion
    public int ProbabilidadBloqueoOcasional;
    private bool esPosibleBloquear = true;
    public void BloqueoOcasional(float PlayerAtaque, Vector3 PlayerPosition)
    {

        if (PlayerAtaque > 0 && esPosibleBloquear && Vector3.Distance(transform.position, PlayerPosition) < RANGOATAQUE)
        {
            if (ProbabilidadBloqueoOcasional >= Random.Range(1, 100)) BloqueaAtaque();
            else
            {//En caso de que no haya caido en el random, bloqueo la posibilidad de buscar hasta que pase un tiempo
                esPosibleBloquear = false;
                Invoke("In_CancelarBloqueoOcasional", Random.Range(0.9f, 2f));
            }
        }
    }
    void BloqueaAtaque()
    {
        CollidersEnemigo.layer = LayerEnemigoBloqueando; //EnemigoBloqueando
        esPosibleBloquear = false;
        AnimBloqueo_Ocasional(true);
        Invoke("In_CancelarBloqueoOcasional", Random.Range(0.9f, 2f));
    }

    void In_CancelarBloqueoOcasional()
    {
        CollidersEnemigo.layer = LayerEnemigo; //Enemigo
        esPosibleBloquear = true;
        AnimBloqueo_Ocasional(false);
    }

    //NEWSCRIPT///////////////////////////////////////////

    //Combate////////////////////

    public int QueAccion; //TODO:PASAR A PRIVATE 

    public int ProbabilidadAtaque;
    public int ProbabilidadBloqueo;
    public int ProbabilidadEsquive;
    public int ProbabilidadDashear;
    public int ProbabilidadDeEnemEstatico;
    public int ProbabilidadLanzarFlecha;

    public string nuevaAccion; //TODO:PASAR A PRIVATE 
    private string viejaAccion;

    public float MinTiempoEntreAcciones;
    public float ContadorEntreAcciones;
    //[HideInInspector]
    public bool AccionEncontrada;

    private bool Flag1;

    /// <summary>
    /// Activa el modo combate para el enemigo, teniendo asi en cuenta cada uno de los factores de la pelea, algunos cambios tendran que ajustarse desde el inspector.
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="MinTiempoEntreAcciones"></param>
    /// <param name="MinTiempoEntreAcciones_Default"></param>
    public void ModoCombate(Vector3 PlayerPosition)
    {
        if (!Flag1)
        {
            Flag1 = true;
            ContadorEntreAcciones = MinTiempoEntreAcciones;
        } //UNA SOLA LECTURA! 

        if (Vector2.Distance(transform.position, PlayerPosition) <= RANGOATAQUE && 
            DetectaSuelo("PisoConPlayer") && !AccionEncontrada) 
        {

            if (ContadorEntreAcciones >= 0) ContadorEntreAcciones = ContadorEntreAcciones - Time.deltaTime;
            else
            {
                nuevaAccion = SeleccionaAccionAleatoria();

                if (nuevaAccion == viejaAccion && ContadorEntreAcciones <= 0)
                {
                    CantidadDeRepeticionPosible();

                    if (!buscarNuevaAccion)
                    {
                        AccionEncontrada = true;
                        ContadorEntreAcciones = MinTiempoEntreAcciones;
                    }
                    //else vuelve a tirar nueva accion, lo hara de forma automatica
                }
                else
                {
                    viejaAccion = nuevaAccion; //Obtiene nuevo valor para la proxima vuelta

                    AccionEncontrada = true;
                    ContadorEntreAcciones = MinTiempoEntreAcciones;
                }
            }
        }//SIN ACCION ENCONTRADA! 

        if (AccionEncontrada) //CON ACCION ENCONTRADA! 
        {
                switch (nuevaAccion)
                {

                    case "Estatico":
                        AccionEstatico();
                        break;

                    case "Ataque":
                        AccionAtaque();
                        break;
                    case "Bloqueo":
                        AccionBloqueo();
                        break;
                    case "Esquive":

                        break;
                    

                        //CASES
                        //else if (TipoDeAcccion == "Flechazo");
                        //else if (TipoDeAcccion == "Dashear");
                }
        }
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
            } //DEFAULT
        }

    #region Variables de CantidadDeRepeticionesPosibles
    //Automaticamente se sumara uno al numero seleccionado. 
    private int cantRepAtaqueDefault = 2; //+1
    private int cantRepAtaque;

    private int cantRepBloqueoDefault = 1; //+1
    private int cantRepBloqueo;

    private bool buscarNuevaAccion;
    #endregion
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

    #region VARIABLES DE ACCION "Estatico"
    private float cuantoTiempoEstatico;
    private bool flag2;
    #endregion
    void AccionEstatico()
    {
        if (!flag2)
        {
            flag2 = true;
            cuantoTiempoEstatico = Random.Range(1, 2f);
        }
        if (cuantoTiempoEstatico >= 0) cuantoTiempoEstatico -= Time.deltaTime; //QUIETO
        else
        {
            flag2 = false;
            AccionEncontrada = false;
        } //BUSCA NUEVA ACCION! 
    }

    #region VARIABLES DE ACCION "Ataque"
    private bool Flag2;
    #endregion 
    void AccionAtaque()
    {
        if (!Flag2)
        {
            AnimAtaque(1, Random.Range(1, 3));
            Flag2 = true;
        }
    }

    public Transform LugarDeAtaque;
    public LayerMask LayerDelPlayer;
    public float RangoDeAtaque;

    public void EnviaDanio()
    {
        Collider2D[] DanioAPlayer = Physics2D.OverlapCircleAll(LugarDeAtaque.position, RangoDeAtaque, LayerDelPlayer);

        foreach (Collider2D collider in DanioAPlayer)
        {
            collider.GetComponent<DanioYVidaRauner>().Empuje(false,AQueLadoMira()); //Empuja y saca vida, o mata!
        }
    }

    public void EndAttack()
    {
        AccionEncontrada = false;
        Flag2 = false;

        OffAtaque();

        AnimCaminata(false);
        AnimBloqueo_Random(false);
        AnimEsquive(false);

        /*anim.SetBool("Caminata", false);
        anim.SetBool("Ataque", false);
        anim.SetBool("BloqueoRandom", false);
        anim.SetBool("Esquive", false);*/
    }

    #region VARIABLES DE ACCION "Bloqueo"
    private bool flag3;
    private float cuantoTiempoBloqueando;
    #endregion
    void AccionBloqueo()
    {
        if (!flag3)
        {
            flag3 = true;
            cuantoTiempoBloqueando = Random.Range(2f, 2.5f);
        }
        if (cuantoTiempoBloqueando >= 0)
        {
            cuantoTiempoBloqueando -= Time.deltaTime;
            CollidersEnemigo.layer = LayerEnemigoBloqueando;
            AnimBloqueo_Random(true);
        }
        else
        {
            CollidersEnemigo.layer = LayerEnemigo;
            flag3 = false;
            AccionEncontrada = false;
            AnimBloqueo_Random(false);
        }
    }

    public void AccionesDeMuerte()
    {
        CollidersEnemigo.SetActive(false);
        bloqueaRotacion = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(LugarDeAtaque.position, RangoDeAtaque);
    }
} /////////////

