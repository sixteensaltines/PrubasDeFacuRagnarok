using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanioYVidaRauner : GeneralPlayer
{
    public int Vida;
    public Animator anim;
    private RaunerInputs raunerInputs;
    private Rigidbody2D rb;

    public GameObject CollidersObject; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        raunerInputs = GetComponent<RaunerInputs>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (EmpujeOn) EjecutoDesplazamiento();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo") && DetectaSuelo() && !EmpujeActivado_derecha && !EmpujeActivado_izquierda)
        {
            if (CollidersObject.layer == LayerPlayer) Empuje(true);
        }
        if (collision.CompareTag("Enemigo") && !DetectaSuelo() && !EmpujeActivado_izquierda &&!EmpujeActivado_derecha)
        {
            LlegaDanio();
        }
    }

    public float FuerzaEmpuje;
    public float ContadorEmpujeDefault;
    public float CadenciaEmpuje;

    private float contadorEmpuje;
    private bool EmpujeActivado_izquierda;
    private bool EmpujeActivado_derecha;
    private bool puedenEmpujarlo = true;

    private bool flag1;

    [HideInInspector]
    public bool EmpujeOn;

    /// <summary>
    /// Empuje por colision o Empuje por danio!, si es por danio, el primer valor debe ser true, si no es false. 
    /// </summary>
    /// <param name="AQueLadoMiraElEnemigo"></param>
    public void Empuje(bool EsPorColisionDanio, string AQueLadoMiraElEnemigo="") //De que lado lo atacan? 
    {
        if (!GetMuere()) //Se muere con el proximo ataque?
        {
            EmpujeOn = true;

            if (!flag1)
            {
                contadorEmpuje = ContadorEmpujeDefault; //Variable de dash en deulfat
                flag1 = true;
            }

            if (puedenEmpujarlo && !EsPorColisionDanio)
            {
                LlegaDanio();

                if (AQueLadoMiraElEnemigo == "derecha")
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);

                    EmpujeActivado_izquierda = false;
                    EmpujeActivado_derecha = true;
                }
                if (AQueLadoMiraElEnemigo == "izquierda")
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);

                    EmpujeActivado_derecha = false;
                    EmpujeActivado_izquierda = true;
                }

            }
            if (puedenEmpujarlo && EsPorColisionDanio)
            {
                LlegaDanio();

                if (transform.eulerAngles.y == 0) //Mira a la derecha
                {
                    EmpujeActivado_derecha = false;
                    EmpujeActivado_izquierda = true;
                }
                if (transform.eulerAngles.y == 180) //Mira a la izquierda
                {
                    EmpujeActivado_izquierda = false;
                    EmpujeActivado_derecha = true;
                }
            }

            if (EmpujeActivado_derecha || EmpujeActivado_izquierda)
            {
                EjecutoDesplazamiento();
            }
        }
        else
        {
            LlegaDanio();
           
        }
    }
    void EjecutoDesplazamiento()
    {
        if (contadorEmpuje <= 0)
        {
            EmpujeOn = false; //Apago el empuje para el Update

            EmpujeActivado_izquierda = false;
            EmpujeActivado_derecha = false;
            rb.velocity = Vector2.zero;

            Invoke("In_SaleDelStun", CadenciaEmpuje);
        }
        else
        {
            raunerInputs.BlockButtons = true;
            raunerInputs.QuitForces();

            Physics2D.IgnoreLayerCollision(LayerEnemigo, LayerPlayer, true);

            anim.SetBool("Empuje", true);

            puedenEmpujarlo = false;

            contadorEmpuje -= Time.deltaTime;


            if (EmpujeActivado_izquierda)
            {
                rb.velocity = Vector2.left * FuerzaEmpuje;
            }
            if (EmpujeActivado_derecha)
            {
                rb.velocity = Vector2.right * FuerzaEmpuje;
            }
        }
    }
    private void In_SaleDelStun()
    {
        contadorEmpuje = ContadorEmpujeDefault;

        raunerInputs.BlockButtons = false;

        Physics2D.IgnoreLayerCollision(LayerEnemigo, LayerPlayer, false);

        puedenEmpujarlo = true;
    }

    public void LlegaDanio()
    {
        if (CollidersObject.layer == LayerPlayer) DescuentaVida();
    }


    public ParticleSystem Sangre;
    public void DescuentaVida()
    {
        Vida--;
        Sangre.Play(true);

        if (Vida <= 0)
        {
            anim.SetBool("RaunerMuerte", true);
        }
        else { }//Danio
    }

    private bool GetMuere() //Con el proximo danio muere! 
    {
        if (Vida == 1) return true;
        else return false;
    }
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

