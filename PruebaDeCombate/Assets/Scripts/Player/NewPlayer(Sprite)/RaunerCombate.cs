using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaunerCombate : RaunerAnims
{
    private Animator anim;
    private RaunerInputs raunerInputs;

    private EfectosScripteadosAnimaciones efectosAnimaciones;

    public GameObject CollidersObject;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        raunerInputs = GetComponent<RaunerInputs>();
        efectosAnimaciones = GetComponentInChildren<EfectosScripteadosAnimaciones>();
    }

    void Update()
    {
        Block();
        Combo();
        //if(efectosAnimaciones.EsPosibleParry)Parry(); //ActivadoDesdeAnimaciones
    }

    void Block()
    {
        if (raunerInputs.BH_Block && DetectaSuelo())
        {
            AnimBloqueo(anim, true);

            raunerInputs.QuitForces();

            raunerInputs.BlockJump = true;
            raunerInputs.BlockWalk = true;

            CollidersObject.layer = 14; //PlayerBloqueando
        }
        if(raunerInputs.BU_Block)
        {
            AnimBloqueo(anim, false);

            raunerInputs.BlockJump = false;
            raunerInputs.BlockWalk = false;

            CollidersObject.layer = 12; //Player
        }
    }

    public bool CancelaDanio() //Cuando se mide el daño desde el "Medidor de vida", pregunta por este metodo
    {
        if (raunerInputs.BH_Block && DetectaSuelo()) return true;
        else return false;
    }

    [HideInInspector]
    public int NumeroDeAtaque;
    public float CadenciaCombo;

    void Combo()
    {
        if (raunerInputs.BD_Attack && NumeroDeAtaque == 0 && DetectaSuelo())
        {
            NumeroDeAtaque++;
            GolpeAnim(anim, NumeroDeAtaque, true);
        }
        else if (raunerInputs.BD_Attack && NumeroDeAtaque == 1 && efectosAnimaciones.EstadoDelCombo() && DetectaSuelo())
        {
            NumeroDeAtaque++;
            efectosAnimaciones.ComboOff();
            GolpeAnim(anim, NumeroDeAtaque, true);
        }
        else if (raunerInputs.BD_Attack && NumeroDeAtaque == 2 && efectosAnimaciones.EstadoDelCombo() && DetectaSuelo())
        {
            NumeroDeAtaque++;
            efectosAnimaciones.ComboOff();
            GolpeAnim(anim, NumeroDeAtaque, true);
        }
    }

    public Transform LugarDeAtaque;
    public LayerMask Layer_EnemigoSinBloquear;
    public LayerMask Layer_EnemigoBloqueando;
    public float RangoDeAtaque;
    public void EnviaDanio()
    {
        DetectaEnemigo();
        DetectaEnemigoBloqueando();
    }

    private void DetectaEnemigo()
    {
        Collider2D[] DanioAEnemigo = Physics2D.OverlapCircleAll(LugarDeAtaque.position, RangoDeAtaque, Layer_EnemigoSinBloquear);

        foreach (Collider2D collider in DanioAEnemigo)
        {
            //EfectosDelCombo(NumeroDeAtaque, false);
            collider.GetComponentInParent<ContadorDeVida>().LlegaDanio();
        }
    }

    private void DetectaEnemigoBloqueando()
    {
        Collider2D[] EscudoEnemigo = Physics2D.OverlapCircleAll(LugarDeAtaque.position, RangoDeAtaque, Layer_EnemigoBloqueando);

        foreach (Collider2D collider in EscudoEnemigo)
        {
            //EfectosDelCombo(NumeroDeAtaque, true);
        }       
    }
  

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(LugarDeAtaque.position, RangoDeAtaque);
    }
    /*[HideInInspector]
    public CircleCollider2D colliderEffector;
    [HideInInspector]
    public bool ActiveParry; //Devuelve proyectiles
    void Parry()
    {
        if (medidorVida.LlegaDanio)
        {
            anim.SetBool("Parry", true);

            ActiveParry = true; //Se apaga desde los efectos de animaciones

            colliderEffector = GetComponent<CircleCollider2D>();
            colliderEffector.radius = 3f; //ActivoEmpuje
            Invoke("In_RadioOriginal", 0.1f);
            
            //TODO: POSIBLE EFECTO 
        }
    }
    void In_RadioOriginal()
    {
        colliderEffector.radius = 0.1f; //DesactivoEmpuje;
    }*/


}
