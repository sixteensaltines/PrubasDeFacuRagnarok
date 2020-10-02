using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueoV2 : MonoBehaviour
{
    public Inputs En_Inputs;

    [HideInInspector]
    public bool ActivarEscudo;

    private void Update()
    {
        DondeEstaElEscudo();
    }
    void DondeEstaElEscudo()
    {
        if (En_Inputs.BH_Block) ActivarEscudo = true;
        else ActivarEscudo = false;
    }
}


