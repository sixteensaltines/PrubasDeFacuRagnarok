using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectosAnimaciones : MonoBehaviour
{
    private RaunerInputs raunerInputs;
    private RaunerCombate raunerCombate;
    private Animator anim;

    public bool PosibleGolpe;

    void Start()
    {
        raunerInputs = GetComponentInParent<RaunerInputs>();
        raunerCombate = GetComponentInParent<RaunerCombate>();
        anim = GetComponent<Animator>();
    }

    public void ComboOn() => PosibleGolpe = true;

    public void ComboOff() => PosibleGolpe = false;

    public bool EstadoDelCombo()
    {
        return PosibleGolpe;
    }

    public void EndCombo()
    {

        PosibleGolpe = false;
        raunerCombate.NumeroDeAtaque = 0;

        anim.SetBool("Ataque", false);
        anim.SetInteger("QueAtaque", raunerCombate.NumeroDeAtaque);

        raunerInputs.BlockAttack = true;
        raunerCombate.BloqueoPorAnimacion = true;

        raunerInputs.BlockJump = false;
        raunerInputs.BlockWalk = false;
        raunerInputs.BlockShield = false;
        raunerInputs.BlockWalk = false;

        raunerInputs.QuitForces = false;

        Invoke("In_DesbloqueaAtaque", raunerCombate.CadenciaCombo);
    }
    public void In_DesbloqueaAtaque()
    {
        raunerCombate.BloqueoPorAnimacion = false;
        raunerInputs.BlockAttack = false;
    }


    public void BlockMovements() 
    {
        Debug.Log("CACACA");
        raunerInputs.BlockJump = true;
        raunerInputs.BlockWalk = true;
        raunerInputs.BlockShield = true;
        raunerInputs.BlockWalk = true;
        raunerInputs.QuitForces = true;
    }
}
