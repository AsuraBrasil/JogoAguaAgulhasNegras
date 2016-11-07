using UnityEngine;
using System.Collections;

public class BossEvents : MonoBehaviour {

    private GameManager gm;
    private float intervaloAtaques;
    public Transform boca;
    public GameObject bolaLixo;


	// Use this for initialization
	void Start ()
    {
        gm = GameManager.gm;
        if(gm.dificuldade == GameManager.Dificuldade.facil)
        {
            intervaloAtaques = 10f;
        }
        else
        {
            intervaloAtaques = 5f;
        }
        StartCoroutine(Ataques());
    }
	
	// Update is called once per frame

    IEnumerator Ataques()
    {
        while (gm.continuaWaves)
        {
            yield return new WaitForSeconds(intervaloAtaques);
            Instantiate(bolaLixo, boca.position, Quaternion.identity);
            
        }
    }
}
