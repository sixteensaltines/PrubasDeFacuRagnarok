using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectosAnimaciones : MonoBehaviour
{
    [HideInInspector]
    public bool EsPosibleParry;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void PosibleParry()
    {
        if (EsPosibleParry) EsPosibleParry = false;
        else EsPosibleParry = true;
    }

    public void Cancelar_AnimacionParry() => anim.SetBool("Parry", false);

}
