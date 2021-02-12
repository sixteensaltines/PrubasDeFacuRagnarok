using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    public Animator EfectosDelPlayer;


    public Transform PieCaminata;
    public Transform CentroDePies;

    public RaunerMovimiento generalPlayer_RaunerMov;

    public GameObject _AudioSource1;
    public GameObject _AudioSource2;
    public GameObject _AudioSource3;

    public AudioSource _AudioSource1_A;
    public AudioSource _AudioSource2_A;
    public AudioSource _AudioSource3_A;

    void Start()
    {
        generalPlayer_RaunerMov = GetComponentInParent<RaunerMovimiento>();
        _AudioSource1_A = _AudioSource1.GetComponent<AudioSource>();
        _AudioSource2_A = _AudioSource2.GetComponent<AudioSource>();
        _AudioSource3_A = _AudioSource3.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (generalPlayer_RaunerMov.GetInfoActivaSalto)
        {
            ParticulaSalto();
            generalPlayer_RaunerMov.GetInfoActivaSalto = false;
        }
    }

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

    public ParticleSystem Caminata_Tierra;
    public ParticleSystem Caminata_Agua;
    public ParticleSystem Caminata_Pasto;

    public void ParticulasCaminata() //PROVISIONAL
    {
       if (generalPlayer_RaunerMov.DetectaSuelo() && generalPlayer_RaunerMov.tipoDeSueloViejo == 17) //Tierra
       {
            var Tierra_Particle = Instantiate(Caminata_Tierra, PieCaminata.position, Quaternion.identity,transform.parent);
            Tierra_Particle.transform.rotation = transform.rotation;
            Tierra_Particle.transform.parent = null;
        }
       else if (generalPlayer_RaunerMov.DetectaSuelo() && generalPlayer_RaunerMov.tipoDeSueloViejo == 18) //Agua
       {
            var Agua_Particle = Instantiate(Caminata_Agua, PieCaminata.position, Quaternion.identity);
            Agua_Particle.transform.rotation = transform.rotation;
            Agua_Particle.transform.parent = null;
        }
       else if (generalPlayer_RaunerMov.DetectaSuelo() && generalPlayer_RaunerMov.tipoDeSueloViejo == 19) //Pasto
       {
            var Pasto_Particle = Instantiate(Caminata_Pasto, PieCaminata.position, Quaternion.identity);
            Pasto_Particle.transform.rotation = transform.rotation;
            Pasto_Particle.transform.parent = null;
        }
    }

    public ParticleSystem Salto_Agua_Particula;
    public ParticleSystem Salto_Tierra_Particula;
    public ParticleSystem Salto_Pasto_Particula;

    public void ParticulaSalto()
    {
        if (generalPlayer_RaunerMov.tipoDeSueloViejo == 17) //Tierra
        {
            var Particulas_SaltoTierra = Instantiate(Salto_Tierra_Particula, CentroDePies.position, Quaternion.identity);
            Particulas_SaltoTierra.transform.parent = null;
        }
        else if (generalPlayer_RaunerMov.tipoDeSueloViejo == 18) //Agua
        {
            var Particulas_SaltoAgua = Instantiate(Salto_Agua_Particula, CentroDePies.position, Quaternion.identity);
            Particulas_SaltoAgua.transform.parent = null;
        }
        else if (generalPlayer_RaunerMov.tipoDeSueloViejo == 19) //Pasto
        {
            var Particulas_SaltoPasto = Instantiate(Salto_Pasto_Particula, CentroDePies.position, Quaternion.identity);
            Particulas_SaltoPasto.transform.parent = null;
        }
    }

    public ParticleSystem Caida_Agua_Particula;
    public ParticleSystem Caida_Tierra_Particula;
    public ParticleSystem Caida_Pasto_Particula;

    public void Particula_Caida()
    {
        if (generalPlayer_RaunerMov.tipoDeSueloViejo == 17) //Tierra
        {
            Instantiate(Caida_Tierra_Particula, CentroDePies.position, Quaternion.identity);
            var ParticulaAlReves = Instantiate(Caida_Tierra_Particula, CentroDePies.position, Quaternion.identity);
            ParticulaAlReves.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (generalPlayer_RaunerMov.tipoDeSueloViejo == 18) //Agua
        {
            Instantiate(Caida_Agua_Particula, CentroDePies.position, Quaternion.identity);
            var ParticulaAlReves = Instantiate(Caida_Agua_Particula, CentroDePies.position, Quaternion.identity);
            ParticulaAlReves.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (generalPlayer_RaunerMov.tipoDeSueloViejo == 19) //Pasto
        {
            Instantiate(Caida_Pasto_Particula, CentroDePies.position, Quaternion.identity);
            var ParticulaAlReves = Instantiate(Caida_Pasto_Particula, CentroDePies.position, Quaternion.identity);
            ParticulaAlReves.transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }




    [SerializeField]
    private AudioClip[] clipsCaminata;
    private AudioClip AudioClipAnterior;
    private AudioClip NuevoClip;

    public void SonidosAlCaminar()
    {
        if (!_AudioSource1_A.isPlaying)
        {
            NuevoClip = GetRandomClip();

            while (NuevoClip == AudioClipAnterior)
            {
                NuevoClip = GetRandomClip();
            }
            AudioClipAnterior = NuevoClip;

            _AudioSource1_A.clip = NuevoClip;

            _AudioSource1_A.volume = Random.Range(0.8f, 1);
            _AudioSource1_A.pitch = Random.Range(0.90f, 1.10f);

            _AudioSource1_A.PlayOneShot(_AudioSource1_A.clip);
        }
        else if(!_AudioSource2_A.isPlaying)
        {
            NuevoClip = GetRandomClip();

            while (NuevoClip == AudioClipAnterior)
            {
                NuevoClip = GetRandomClip();
            }
            AudioClipAnterior = NuevoClip;

            _AudioSource2_A.clip = NuevoClip;

            _AudioSource2_A.volume = Random.Range(0.8f, 1);
            _AudioSource2_A.pitch = Random.Range(0.90f, 1.10f);

            _AudioSource2_A.PlayOneShot(_AudioSource2_A.clip);
        }
        else
        {
            NuevoClip = GetRandomClip();

            while (NuevoClip == AudioClipAnterior)
            {
                NuevoClip = GetRandomClip();
            }
            AudioClipAnterior = NuevoClip;

            _AudioSource3_A.clip = NuevoClip;

            _AudioSource3_A.volume = Random.Range(0.8f, 1);
            _AudioSource3_A.pitch = Random.Range(0.90f, 1.10f);

            _AudioSource3_A.PlayOneShot(_AudioSource3_A.clip);
        }
    }

    private AudioClip GetRandomClip()
    {
        return clipsCaminata[UnityEngine.Random.Range(0, clipsCaminata.Length)];
        
    }
}
