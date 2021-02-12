using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectosScripteadosAnimaciones : MonoBehaviour
{
    private RaunerInputs raunerInputs;
    private  RaunerCombate raunerCombate;
    private  PlayerEffects playerEffects;

    private Animator animRauner;

    public GameObject SkinFalso; //Skin que nos muestra donde esta el personaje!

    public bool PosibleGolpe;

    void Start()
    {
        SkinFalso.GetComponent<SpriteRenderer>().enabled = false;

        playerEffects = GetComponentInChildren<PlayerEffects>();

        raunerInputs = GetComponentInParent<RaunerInputs>();
        raunerCombate = GetComponentInParent<RaunerCombate>();
        animRauner = GetComponent<Animator>();
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

        animRauner.SetBool("Ataque", false);
        animRauner.SetInteger("QueAtaque", raunerCombate.NumeroDeAtaque);

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

    public void AnimacionEmpujeOff() => animRauner.SetBool("Empuje", false);

    public void BloqueoSupremoDeAcciones() => raunerInputs.ControlSupremoInputs(true);
    public void DesbloqueoSupremoDeAcciones() => raunerInputs.ControlSupremoInputs(false);
    public void DestruirObjeto() => Debug.Log("RaunerMuerto");


    //PARTICULAS////////////////////

    public void SpawnParticula_Caminata() => playerEffects.ParticulasCaminata();

    public void SpawnParticula_Salto() => playerEffects.ParticulaSalto();

    public void SpawnParticula_Caida() => playerEffects.Particula_Caida();

    public void SonidoCaminata() => playerEffects.SonidosAlCaminar();

}
