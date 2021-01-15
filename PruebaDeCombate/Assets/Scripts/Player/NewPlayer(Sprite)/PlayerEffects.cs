using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    public Animator EfectosDelPlayer;

    public void EfectosDelCombo(int NumeroDeAtaque, bool AtacoAlEscudo)
    {
        if (!AtacoAlEscudo)
        {
            switch (NumeroDeAtaque)
            {
                case 1:
                    EfectosDelPlayer.SetBool("Golpe1", true);
                    break;
                case 2:
                    EfectosDelPlayer.SetBool("Golpe2", true);
                    break;
                case 3:
                    EfectosDelPlayer.SetBool("Golpe3", true);
                    break;
                default:
                    Debug.Log("El efecto de golpe no pudo ser procesado!, el numero es incorrecto");
                    break;
            }
        }
        EfectosDelPlayer.SetBool("GolpeAEscudo", true);
    }

    public void CancelarEfectosDelCombo()
    {
        EfectosDelPlayer.SetBool("Golpe1", false);
        EfectosDelPlayer.SetBool("Golpe2", false);
        EfectosDelPlayer.SetBool("Golpe3", false);
        EfectosDelPlayer.SetBool("GolpeAEscudo", false);
    }
}
