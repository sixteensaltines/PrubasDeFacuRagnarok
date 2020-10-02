using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionPlayer : MonoBehaviour
{
    public Inputs En_Inputs;

    public ModoDeJuego En_ModoDeJuego;

    public Animator anim;

    public Desplazamiento En_Desplazamiento;
    public BloqueoV2 En_BloqueoV2;
    public Salto En_Salto;
    public AtaqueV2 En_AtaqueV2;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        AnimEntryOrExitGuard();

        AnimWalk_Exp();
        AnimJump_Exp();

        AnimFalling_Exp_Guard();

        AnimDash_Guard();

        AnimWalk_Guard_Legs();
        AnimWalk_Guard_Torso();

        AnimBlock_Guard_Torso();

        AnimAttack_Guard();
    }

    void AnimWalk_Exp()
    {
        TimerWalk();

        if (En_Inputs.BH_Right && timerComplete || En_Inputs.BH_Left && timerComplete && En_Salto.EstaSuelo)
        {
            if ((En_Inputs.BH_Right && timerComplete) && (En_Inputs.BH_Left && timerComplete))
            {
                anim.SetBool("IsWalkExp", false);
            }
            else
            {
                anim.SetBool("IsWalkExp", true);
            }
        }
        else
        {
            anim.SetBool("IsWalkExp", false);
        }
    }
    void AnimFalling_Exp_Guard()
    {
        if (En_Salto.EstaSuelo) anim.SetBool("IsGrounded", true);
        else anim.SetBool("IsGrounded", false);
    }
    void AnimJump_Exp()
    {
        #region JUMP EXP
        if (En_Salto.estaSaltando) anim.SetBool("IsJump", true);
        else anim.SetBool("IsJump", false);
        #endregion
    }
    void AnimEntryOrExitGuard()
    {
        if (En_ModoDeJuego.EstaEnModoGuardia) anim.SetBool("ISGUARDMODE", true);
        else anim.SetBool("ISGUARDMODE", false);
    }
    void AnimDash_Guard()
    {
        if (En_Desplazamiento.Derecha || En_Desplazamiento.Izquierda)
        {
            anim.SetBool("IsDash", true);
            anim.SetBool("ISGUARDMODE", false);
        } 
        else
        {
            anim.SetBool("IsDash", false);
        }
    }
    void AnimWalk_Guard_Legs()
    {
        TimerWalk();

        if (En_Inputs.BH_Right && timerComplete || En_Inputs.BH_Left && timerComplete && En_Salto.EstaSuelo)
        {
            if ((En_Inputs.BH_Right && timerComplete) && (En_Inputs.BH_Left && timerComplete))
            {
                anim.SetBool("IsWalkGuard_Legs", false);
                anim.SetBool("IsIdleGuard_Legs", true);
            }
            else
            {
                anim.SetBool("IsIdleGuard_Legs", false);
                anim.SetBool("IsWalkGuard_Legs", true);
            }
        }
        else
        {
            anim.SetBool("IsWalkGuard_Legs", false);
            anim.SetBool("IsIdleGuard_Legs", true);
        }
    }
    void AnimWalk_Guard_Torso()
    {
        if ((En_Inputs.BH_Right || En_Inputs.BH_Left) && !En_BloqueoV2.ActivarEscudo && En_Salto.EstaSuelo)
        {
            anim.SetBool("IsWalkGuard_Torso", true);
        }
        else
        {
            anim.SetBool("IsWalkGuard_Torso", false);
        }
    }
    void AnimBlock_Guard_Torso()
    {
        if (En_BloqueoV2.ActivarEscudo)
        {
            anim.SetBool("IsIdleGuard_Torso", false);
            anim.SetBool("IsBlock_Torso", true);
        }
        else
        {
            anim.SetBool("IsBlock_Torso", false);
            anim.SetBool("IsIdleGuard_Torso", true);
        }
    }

    void OnIsGuardMode() => anim.SetBool("ISGUARDMODE", true);

    #region Variables Contador Caminata
    private float timerWalkDefault = 0.04f;
    private float timerWalk;
    private bool timerComplete;
    #endregion
    void TimerWalk()
    {
        if (En_Inputs.BH_Right || En_Inputs.BH_Left)
        {
            if (timerWalk > 0)
            {
                timerWalk -= Time.deltaTime;
            }
            else
            {
                timerComplete = true;
            }
        }
        else
        {
            timerWalk = timerWalkDefault;
            timerComplete = false;
        }
        
    }

    public void AnimAttack_Guard()
    {

        if (En_AtaqueV2.ActiveCombo)
        {
            anim.SetBool("ISGUARDMODE", false);
            anim.SetBool("ATTACKMODE", true);

            anim.SetFloat("AttackFlow", En_AtaqueV2.AttackFlow);
        }


    }
    public void EndAttackAnim() //LLAMADA DESDE ATAQUEV2
    {
        anim.SetBool("ISGUARDMODE", true);
        anim.SetBool("ATTACKMODE", false);
    }
}
