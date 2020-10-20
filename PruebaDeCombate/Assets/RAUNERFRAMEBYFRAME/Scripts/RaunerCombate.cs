using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaunerCombate : GeneralPlayer
{
    public Animator anim;
    public RaunerInputs raunerInputs;

    private MedidorVida medidorVida;
    public EfectosAnimaciones efectosAnimaciones;

    void Start()
    {
        raunerInputs = GetComponent<RaunerInputs>();
        medidorVida = GetComponent<MedidorVida>();
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

    void Parry()
    {
        if (medidorVida.LlegaDanio)
        {
            anim.SetBool("Parry", true);

            colliderEffector = GetComponent<CircleCollider2D>();
            colliderEffector.radius = 3f; //ActivoEmpuje
            colliderEffector.radius = 0.1f; //DesactivoEmpuje;

            //TODO: POSIBLE EFECTO 
        }
    }


}
