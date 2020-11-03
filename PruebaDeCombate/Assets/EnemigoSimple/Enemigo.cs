using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : AnimacionesEnemigos
{
    private float rangoVision = 11.5f;
    private const float RANGOATAQUE = 2.3f;

    public void CaminataAPlayer(Vector3 PlayerPosition)
    {
        RotacionSkinEnemigo(PlayerPosition);
        EjecutaMovimiento(PlayerPosition);
    }

    public void RotacionSkinEnemigo(Vector3 PlayerPosition)
    {
        if (transform.position.x < PlayerPosition.x) transform.eulerAngles = new Vector3(0, 0, 0);
        else transform.eulerAngles = new Vector3(0, 180, 0);
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
        EjecutaSalto(PlayerPosition,rbEnemigo);
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

        if(DetectaPared_Delante() && !DetectaPlayer_Delante() && DetectaSuelo())
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
                Vector2.right,largoRayo_AlPlayer, 1 << LayerMask.NameToLayer("Player"));
            if (ray == true) return true;
            else ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f),
                Vector2.right , largoRayo_AlPlayer, 1 << LayerMask.NameToLayer("PlayerBloqueando"));
            if (ray == true) return true;
            else return false;
        }
        else //Si se especifica un layer, busca ese solo! 
        {
            return ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f),
                Vector2.right , largoRayo_AlPlayer, 1 << LayerMask.NameToLayer(QuePlayer));
        }
    }

    public bool DetectaPared_Delante()
    {
        RaycastHit2D ray;
        return ray = Physics2D.Raycast((transform.position), Vector2.right , largoRayo_APared, 1 << LayerMask.NameToLayer("PisoConPlayer"));
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
    private bool esPosibleBloquear= true; 
    public void BloqueoOcasional(float PlayerAtaque, Vector3 PlayerPosition)
    {
        if (PlayerAtaque > 0 && esPosibleBloquear && Vector3.Distance(transform.position,PlayerPosition) < RANGOATAQUE)
        {
            if (ProbabilidadBloqueoOcasional >= Random.Range(1, 100)) BloqueaAtaque();
        }
    }
    void BloqueaAtaque()
    {
        gameObject.layer = 15; //EnemigoBloqueando
        esPosibleBloquear = false;
        AnimBloqueo_Ocasional(true);
        Invoke("In_CancelarBloqueoOcasional", Random.Range(0.9f, 2f));
    }

    void In_CancelarBloqueoOcasional()
    {
        gameObject.layer = 13; //Enemigo
        esPosibleBloquear = true;
        AnimBloqueo_Ocasional(false);
    }

    //NEWSCRIPT///////////////////////////////////////////
}
