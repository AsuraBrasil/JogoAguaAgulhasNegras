using UnityEngine;
using System.Collections;

public class Coletavel : MonoBehaviour {

    //GameManager
    GameManager gm = GameManager.gm;

    public float points = 10;
    public float speed = 0.2f;
    private float speedMod = 1f;

    void Start()
    {
        speedMod = Random.Range(0.3f, 1f); //Isso faz com que nem todos os lixos possuam a mesma velocidade, dando um dinamismo ao jogo.
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Vara")
        {
            gm.Pontuar(points);
            DestroyObject(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(transform.position.x + speedMod * -speed, transform.position.y, transform.position.z);
        transform.position = newPos;

        if (this.gameObject.transform.position.x < -11)
        {
            if(gm.dificuldade == GameManager.Dificuldade.normal) //Na dificuldade normal (Ens. Medio), ao deixar um lixo passar voc� perde pontos.
            {
                gm.Pontuar(-5);
            }
            DestroyObject(this.gameObject);
        }
    }

}//FIM
