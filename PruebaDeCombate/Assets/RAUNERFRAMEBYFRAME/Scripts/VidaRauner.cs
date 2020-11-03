using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaRauner : RaunerAnims
{
    public int Vida;
    public Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void LlegaDanio()
    {
        if (gameObject.layer == 12) //Player
        {
            DescuentaVida();
        }
    }

    public void DescuentaVida()
    {
        Vida--;
        if (Vida <= 0)
        {
            //Muerte
            anim.SetBool("RaunerMuerte", true);
        }
        else { }//Danio
    }




    /*
    public Transform PosicionDisparaFlecha; 
    public GameObject FlechaPrefab;

    private float randomTimeDefault;
    private float randomTime;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        randomTimeDefault = Random.Range(2f, 4f);
        randomTime = randomTimeDefault;
    }

    void Update()
    {
        if (randomTime < 0)
        {
            if (player.GetComponent<GeneralPlayer>().DetectaSuelo())
            { LanzaFlecha(); }
        }
        else { randomTime -= Time.deltaTime; }
    }

    void LanzaFlecha()
    {
        Instantiate(FlechaPrefab, PosicionDisparaFlecha.position, Quaternion.identity);
        randomTime = randomTimeDefault;
    }*/
}
