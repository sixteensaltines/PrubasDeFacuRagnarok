using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pruebas : MonoBehaviour
{
    public float AnimationSpeed;

    public Animator Anim;
    void Start()
    {
        Anim = GetComponent<Animator>();
    }
    void Update()
    {
        Anim.speed = AnimationSpeed;
    }

}
