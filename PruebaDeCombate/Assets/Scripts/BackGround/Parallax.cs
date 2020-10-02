using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float length; 
    public float startpos;
    public GameObject Camara;
    public float EfectoParallax;
    public float DesfasajeObjetoACamara;
    public float dist;
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {

        //Cuanto se puede desfasar la camara del objeto. //Velocidad a la que el objeto corre a la camara. 
        DesfasajeObjetoACamara = (Camara.transform.position.x * (1 - EfectoParallax));

        //Distancia que recorre de camara 
        dist = (Camara.transform.position.x * EfectoParallax);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (DesfasajeObjetoACamara > startpos + length)
        {
            startpos += length;
        }
        else 
        if (DesfasajeObjetoACamara < startpos - length)
        {
            startpos -= length;
        }
    }
}
