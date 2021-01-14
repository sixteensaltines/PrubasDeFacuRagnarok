using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectosScripteadosAnimaciones : PlayerEffects
{
    private RaunerInputs raunerInputs;
    private RaunerCombate raunerCombate;
    private Animator anim;
    public GameObject SkinFalso; //Skin que nos muestra donde esta el personaje!

    public bool PosibleGolpe;

    void Start()
    {
        SkinFalso.GetComponent<SpriteRenderer>().enabled = false;

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

    public void PuedeEnviarDanio() => raunerCombate.EnviaDanio();

    public void EndCombo()
    {
        PosibleGolpe = false;
        raunerCombate.NumeroDeAtaque = 0;

        anim.SetBool("Ataque", false);
        anim.SetInteger("QueAtaque", raunerCombate.NumeroDeAtaque);

        raunerInputs.BlockWalk = false;
        raunerInputs.BlockJump = false;
        raunerInputs.BlockShield = false;
        raunerInputs.BlockDash = false;

        raunerInputs.BlockAttack = true;

        Invoke("In_DesbloqueaAtaque", raunerCombate.CadenciaCombo);
    }
    public void In_DesbloqueaAtaque() => raunerInputs.BlockAttack = false;

    public void BlockMovements() 
    {
        raunerInputs.BlockWalk = true;
        raunerInputs.BlockJump = true;
        raunerInputs.BlockShield = true;
        raunerInputs.BlockDash = true;

        raunerInputs.QuitForces();
    }

    public void AnimacionEmpujeOff() => anim.SetBool("Empuje", false);

    public void BloqueoSupremoDeAcciones() => raunerInputs.ControlSupremoInputs(true);
    public void DesbloqueoSupremoDeAcciones() => raunerInputs.ControlSupremoInputs(false);
    public void DestruirObjeto() => Debug.Log("RaunerMuerto"); 

}
