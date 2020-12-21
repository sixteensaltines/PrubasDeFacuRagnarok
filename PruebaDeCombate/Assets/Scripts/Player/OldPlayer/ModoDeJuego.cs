using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModoDeJuego : MonoBehaviour
{
    public Inputs En_Inputs;

    public Movimiento En_Movimiento;
    public Desplazamiento En_Desplazamiento;

    //Por defecto es necesario que este en falso! 
    public bool EstaEnModoGuardia;
    private bool[] FlagDeLectura = new bool[2];

    public float VelMovimientoExp;
    public float VelMovimientoGuard;

    public float FuerzaDesplazamientoExp;
    public float FuerzaDesplazamientoGuard;

    public float TiempoDesplazamientoExp;
    public float TiempoDesplazamientoGuard;

    private void Start()
    {
        EstaEnModoGuardia = false;
        En_Inputs.BlockJump = false;
        En_Movimiento.Speed = VelMovimientoExp;
        En_Desplazamiento.FuerzaDesplazamiento = FuerzaDesplazamientoExp;
        En_Desplazamiento.ContadorDefaultDesplazamiento = TiempoDesplazamientoExp;
    }
    void Update()
    {
        
        if (EstaEnModoGuardia && !FlagDeLectura[0])
        {
            GuardModeActive();
            FlagDeLectura[0] = true;
            FlagDeLectura[1] = false;
        }
        if (!EstaEnModoGuardia && !FlagDeLectura[1])
        {
            ExpModeActive();
            FlagDeLectura[1] = true;
            FlagDeLectura[0] = false;
        }

        CambiaDeLadoSkin();
    }
    void CambiaDeLadoSkin()
    {
        if (En_Inputs.BH_Right && !En_Inputs.BH_Left)
        {
             transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (En_Inputs.BH_Left && !En_Inputs.BH_Right)
        {
             transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    void GuardModeActive()
    {
        En_Inputs.BlockJump = true;
        En_Inputs.BlockAttack = false;
        En_Movimiento.Speed = VelMovimientoGuard;
        En_Desplazamiento.FuerzaDesplazamiento = FuerzaDesplazamientoGuard;
        En_Desplazamiento.ContadorDefaultDesplazamiento = TiempoDesplazamientoGuard;
    }
    void ExpModeActive()
    {
        En_Inputs.BlockJump = false;
        En_Inputs.BlockAttack = true;
        En_Movimiento.Speed = VelMovimientoExp;
        En_Desplazamiento.FuerzaDesplazamiento = FuerzaDesplazamientoExp;
        En_Desplazamiento.ContadorDefaultDesplazamiento = TiempoDesplazamientoExp;
    }
}
