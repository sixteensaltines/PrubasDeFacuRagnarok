using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectosAnimaciones : MonoBehaviour
{
    [HideInInspector]
    public bool EsPosibleParry;

    private Animator anim;

    private RaunerCombate raunerCombate;

    private void Start()
    {
        anim = GetComponent<Animator>();

        raunerCombate = GetComponentInParent<RaunerCombate>();
    }
    public void PosibleParry()
    {
        if (EsPosibleParry) EsPosibleParry = false;
        else EsPosibleParry = true;
    }

    public void Cancelar_AnimacionParry()
    {
        anim.SetBool("Parry", false);
        raunerCombate.ActiveParry = false;
        EsPosibleParry = false;
    }

}
