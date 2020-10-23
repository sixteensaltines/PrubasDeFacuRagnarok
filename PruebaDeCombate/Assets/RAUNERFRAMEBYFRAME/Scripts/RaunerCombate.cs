using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaunerCombate : GeneralPlayer
{
    private Animator anim;
    private RaunerInputs raunerInputs;

    private MedidorVida medidorVida;
    private EfectosAnimaciones efectosAnimaciones;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        raunerInputs = GetComponent<RaunerInputs>();
        medidorVida = GetComponent<MedidorVida>();
        efectosAnimaciones = GetComponentInChildren<EfectosAnimaciones>();

    }

    void Update()
    {
        Block();
        if(efectosAnimaciones.EsPosibleParry)Parry(); //ActivadoDesdeAnimaciones
    }

    void Block()
    {
        if (raunerInputs.BH_Block)
        {
            anim.SetBool("Block", true);

            raunerInputs.QuitForces = true;
            
            raunerInputs.BlockJump = true;
            raunerInputs.BlockWalk = true;
        }
        else
        {
            anim.SetBool("Block", false);

            raunerInputs.QuitForces = false;

            raunerInputs.BlockJump = false;
            raunerInputs.BlockWalk = false;
        }
    }

    public bool CancelaDanio() //Cuando se mide el daño desde el "Medidor de vida", pregunta por este metodo
    {
        if (raunerInputs.BH_Block) return true;
        else return false;
    }

    [HideInInspector]
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
    }


}
