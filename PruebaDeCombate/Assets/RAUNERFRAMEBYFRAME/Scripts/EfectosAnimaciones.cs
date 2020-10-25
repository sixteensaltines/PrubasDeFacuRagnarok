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
        raunerInputs.BlockAttack = true;
        PosibleGolpe = false;
        raunerCombate.NumeroDeAtaque = 0;
        raunerCombate.CadenciaAtaquesCD = true;
        anim.SetBool("Ataque", false);
        anim.SetInteger("QueAtaque", raunerCombate.NumeroDeAtaque);
        Invoke("In_DesbloqueaAtaque", raunerCombate.CadenciaCombo);
    }
    public void In_DesbloqueaAtaque()
    {
        raunerCombate.CadenciaAtaquesCD = false;
        raunerInputs.BlockAttack = false;
    }

}
