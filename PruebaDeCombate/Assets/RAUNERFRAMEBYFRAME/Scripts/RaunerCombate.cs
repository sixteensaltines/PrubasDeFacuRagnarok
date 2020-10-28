using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaunerCombate : GeneralPlayer
{
    private Animator anim;
    private RaunerInputs raunerInputs;

    private EfectosAnimaciones efectosAnimaciones;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        raunerInputs = GetComponent<RaunerInputs>();
        efectosAnimaciones = GetComponentInChildren<EfectosAnimaciones>();

    }

    void Update()
    {
        Block();
        //if(efectosAnimaciones.EsPosibleParry)Parry(); //ActivadoDesdeAnimaciones
    }

    void FixedUpdate() => Combo();

    void Block()
    {
        if (raunerInputs.BH_Block && DetectaSuelo())
        {
            AnimBloqueo(anim, true);

            raunerInputs.QuitForces();

            raunerInputs.BlockJump = true;
            raunerInputs.BlockWalk = true;
        }
        if(raunerInputs.BU_Block)
        {
            AnimBloqueo(anim, false);

            raunerInputs.BlockJump = false;
            raunerInputs.BlockWalk = false;
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
            EnviaDanio();
            GolpeAnim(anim, NumeroDeAtaque, true);
        }
        else if (raunerInputs.BD_Attack && NumeroDeAtaque == 1 && efectosAnimaciones.EstadoDelCombo() && DetectaSuelo())
        {
            NumeroDeAtaque++;
            EnviaDanio();
            efectosAnimaciones.ComboOff();
            GolpeAnim(anim, NumeroDeAtaque, true);
        }
        else if (raunerInputs.BD_Attack && NumeroDeAtaque == 2 && efectosAnimaciones.EstadoDelCombo() && DetectaSuelo())
        {
            NumeroDeAtaque++;
            EnviaDanio();
            efectosAnimaciones.ComboOff();
            GolpeAnim(anim, NumeroDeAtaque, true);
        }
    }

    public Transform LugarDeAtaque;
    public LayerMask LayerDelEnemigo;
    private int DanioPorAtaque = 1; //Por defecto "1" 
    public float RangoDeAtaque;
    void EnviaDanio()
    {
        Collider2D[] DanioAEnemigo = Physics2D.OverlapCircleAll(LugarDeAtaque.position, RangoDeAtaque, LayerDelEnemigo);

        foreach (Collider2D collider in DanioAEnemigo)
        {
            collider.GetComponent<EnemigoProvisional>().LlegaDanio(DanioPorAtaque);
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
