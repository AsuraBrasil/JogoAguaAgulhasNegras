using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossLife : MonoBehaviour
{
    public int bossLife = 100;
    public int points = 50; //Pontos dados quando ele apanha;
    public Animator bossAnim;
    public Slider lifeSlider;
    public Rigidbody2D player;
    public float velocidadeEmpurro = 2f;
    public bool imune = false;
    public bool acabou = false;

    private GameManager gm;
   

    void Start()
    {
        gm = GameManager.gm;
    }

    void Update()
    {
        if(bossLife <= 0 && !acabou)
        {
            gm.Pontuar(3000);
            gm.EncerraFaseBoss();
            acabou = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Vara" && !imune)
        {
            
            if(gm.dificuldade == GameManager.Dificuldade.facil)
            {
                gm.Pontuar(points);
                bossLife = bossLife - 25;
            }
            else
            {
                gm.Pontuar(points*2);
                bossLife = bossLife - 10;
            }
            lifeSlider.value = bossLife;
            //Animação de Boss apanhando
            bossAnim.SetTrigger("Imune");
            //Empurra Jogador
            StartCoroutine(EmpurraJogador());
        }
    }

    IEnumerator EmpurraJogador()
    {
        imune = true;
        yield return new WaitForSeconds(.2f);
        gm.bloqueiaMovimento = true;
        Vector2 newPos = new Vector2(-9f, player.position.y);
        while (player.position.x > -8f)
        {
            yield return new WaitForSeconds(.02f);
            player.position = Vector2.Lerp(player.position, newPos, velocidadeEmpurro);
        }
        imune = false;
        gm.bloqueiaMovimento = false;
    }

}//FIM
