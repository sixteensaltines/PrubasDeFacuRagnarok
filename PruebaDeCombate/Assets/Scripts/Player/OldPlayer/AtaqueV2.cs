using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueV2 : MonoBehaviour
{
    public Inputs En_Inputs;
    public ModoDeJuego En_ModoDeJuego;
    public AnimacionPlayer En_AnimacionPlayer;
    public Movimiento En_Movimiento;

    public Transform T_Mango;
    public Transform T_Punta;

    private bool rayo;
    private bool seMandoGolpe;

    public int DanioAEnemigo;

    private void FixedUpdate()
    {
        if (rayo)
        {
            RayoLanzaPlayer();
        }
    }
    void RayoLanzaPlayer()
    {
        if (rayo && !seMandoGolpe)
        {
            RaycastHit2D hit = Physics2D.Linecast(T_Mango.position, T_Punta.position, 1 << LayerMask.NameToLayer("Accion"));
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemigo"))
            {
                /*EnemigoSimple En_EnemigoSimple = hit.collider.GetComponentInParent<EnemigoSimple>();

                if (En_EnemigoSimple != null)
                {
                    //En_EnemigoSimple.Vida(DanioAEnemigo);
                    seMandoGolpe = true;
                }*/
            }
            else
            {//Borrable
                Debug.DrawLine(T_Mango.position, T_Punta.position, Color.green);
            }
        }
    }
    void OnOffRayoEnemigo()
    {
        if (!rayo)
        {
            rayo = true;
        }
        else
        {
            rayo = false;
            seMandoGolpe = false;
        }
    }

    //[HideInInspector]
    public bool ActiveCombo;
    //[HideInInspector]
    public int QueAtaqueEs; 

    public float CadenciaDeGolpe;

    [HideInInspector]
    public float AttackFlow;

    public bool puedeReactivarse; 
    public bool cancelarCombo = true;

    public bool yaDioElGolpe = false;

    void Update()
    {
        if (En_Inputs.BD_Attack && !yaDioElGolpe && QueAtaqueEs < 2)
        {

            QueAtaqueEs++;
            ActiveCombo = true;
            yaDioElGolpe = true;

            if (QueAtaqueEs == 1)
            {
                AttackFlow = 1;
                En_AnimacionPlayer.AnimAttack_Guard();
            }
        }

        if (puedeReactivarse && En_Inputs.BD_Attack && QueAtaqueEs == 1)  
        {
            AttackFlow = 2;
            En_AnimacionPlayer.AnimAttack_Guard();
            cancelarCombo = false;
        }


        if (puedeReactivarse && En_Inputs.BD_Attack && QueAtaqueEs == 2) 
        {
            AttackFlow = 3;
            En_AnimacionPlayer.AnimAttack_Guard();
            cancelarCombo = false;
        }
    }

    void OnCombo() 
    {
        puedeReactivarse = true;
        yaDioElGolpe = false;
        cancelarCombo = true;
    }

    void OffCombo()
    {
        puedeReactivarse = false;

        if (cancelarCombo)EndAttack();
        else cancelarCombo = true;

        if (QueAtaqueEs > 2)
        {
            EndAttack();
        }
    }
    public void EndAttack()
    {
        En_Inputs.BlockAttack = true;
        Invoke("In_CadenciaCombo", CadenciaDeGolpe);

        En_Movimiento.DesbloquearMovimiento();

        AttackFlow = 0.0f;
        QueAtaqueEs = 0;
        yaDioElGolpe = false;
        ActiveCombo = false;
        En_AnimacionPlayer.EndAttackAnim();
    }

    void In_CadenciaCombo() => En_Inputs.BlockAttack = false;
}


