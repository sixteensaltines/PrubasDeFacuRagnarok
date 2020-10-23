using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flecha : MonoBehaviour
{
    public Vector3 OriginalPosition;
    private Vector3 playerLastPosition;
    public float VelocidadFlecha;

    private GameObject player;
    private RaunerCombate raunerCombate;

    private bool flechaRebota;

    //Tienen el tag "LlegaDanio" //Automaticamente meten daño al player si este no bloquea. 

    void Start()
    {
        OriginalPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        raunerCombate = player.GetComponent<RaunerCombate>();
        playerLastPosition = player.transform.position;

    }

    void Update()
    {
        if (!raunerCombate.ActiveParry && !flechaRebota)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerLastPosition.x, playerLastPosition.y + 2f, playerLastPosition.z), VelocidadFlecha * Time.deltaTime);
        }
        else
        {
            flechaRebota = true;
        }

        if (flechaRebota)
        {
            transform.position = Vector3.MoveTowards(transform.position, OriginalPosition, VelocidadFlecha * Time.deltaTime * 2f);
        }

        if (transform.position == OriginalPosition)
        {
            Destroy(gameObject);
        }

        if(transform.position == new Vector3(playerLastPosition.x, playerLastPosition.y + 2f, playerLastPosition.z)) Destroy(gameObject);

    }
}
