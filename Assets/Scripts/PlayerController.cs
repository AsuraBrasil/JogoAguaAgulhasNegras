using UnityEngine;
using System.Collections;


[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour {

    public float movementSpeed = 0.5f;
    public Boundary boundary;
    public Rigidbody2D rigid2d;
    public GameObject vara;
    public Animator anim;
    public AudioSource sweepSFX;
    public AudioSource hurtSFX;

    public float fireWait = .2f; //Espera da Animação para poder Usar de Novo
    private float nextFire; //Armazena o tempo atual + o tempo de espera
    private bool imune = false;

    //Referencia ao Controlador do jogo
    private GameManager gm;

    void Start()
    {
        gm = GameManager.gm;
    }

    void Update()
    {
        if (!gm.bloqueiaAtaque)
        {
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + fireWait;
                anim.SetTrigger("Fire");
                vara.SetActive(true);
                sweepSFX.PlayDelayed(.2f);
                StartCoroutine(EsperaTempo(fireWait, vara));
            }
        }

        //Esc para aparecer a janela que perguntar se deseja Voltar a tela de Seleção de Fase (Não irei pausar)
        if (Input.GetButton("Cancel"))
        {
            //Mostra Janela de Escape se ela já não estiver habilitada
            if (!gm.ui.escapeWindow.activeInHierarchy)
            {
                gm.ui.MostrarJanela(gm.ui.escapeWindow);
            }
        }
    }

    //FixedUpdate é Melhor para se ter um Movimento de mesma velocidade independente da velocidade da m�quina
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 normalize = new Vector2(moveHorizontal, moveVertical);
        normalize = Vector2.ClampMagnitude(normalize, 1);
        Vector2 newPos = new Vector2(rigid2d.position.x + normalize.x * movementSpeed, rigid2d.position.y + normalize.y * movementSpeed);
        rigid2d.position = newPos;

        rigid2d.position = new Vector2
        (
            Mathf.Clamp(rigid2d.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(rigid2d.position.y, boundary.yMin, boundary.yMax)
        );

        rigid2d.MoveRotation(15*moveVertical);
    }

    //EsperarTempo para Encerrar a Animação da Vara
    IEnumerator EsperaTempo(float waitSecs, GameObject gO)
    {
        yield return new WaitForSeconds(waitSecs);
        if(gO != null)
        {
            gO.SetActive(false);
        }
    }

    //Tempo para sair do modo Imune e voltar a receber dano
    IEnumerator DesativaImune(float waitSecs)
    {
        yield return new WaitForSeconds(waitSecs);
        if (imune)
        {
            imune = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstaculo")
        {
            if(!imune)
            { 
                Debug.Log("Tomou Dano!");
                hurtSFX.Play();
                if (gm.dificuldade == GameManager.Dificuldade.normal) //Na dificuldade normal (Ens. Medio), ao bater em uma pedra você perde pontos.
                {
                    gm.Pontuar(-10);
                }
                anim.SetTrigger("Imune");
                imune = true;
                StartCoroutine(DesativaImune(2f));
                gm.AtualizaVida(-(other.GetComponent<Obstaculo>().dano));
            }
        }
    }

}//FIM
