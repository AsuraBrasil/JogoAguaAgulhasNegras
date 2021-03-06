using UnityEngine;
using System.Collections;
using System;

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
    private float veloInicial = -1; //Velocidade inicial

    //Referencia ao Controlador do jogo
    private GameManager gm;

    void Start()
    {
        gm = GameManager.gm;
        if (movementSpeed > 0)
        {
            veloInicial = movementSpeed; //Variavel utilizada para restaurar a velocidade ao valor estipulado inicialmente
        }
        
    }

    void Update()
    {
        if (!gm.bloqueiaAtaque)
        {
            //Se for Windows irá captar o botão do Mouse
            #if UNITY_STANDALONE_WIN
            if (Input.GetButtonDown("Fire1"))
            {
                Ataque();
            }
            #endif
            //Se for Android, irá captar o botão na Tela, no canto inferior direito
        }

        #if UNITY_STANDALONE_WIN
        //Esc para aparecer a janela que perguntar se deseja Voltar a tela de Seleção de Fase (Não irei pausar)
        if (Input.GetButton("Cancel"))
        {
            //Mostra Janela de Escape se ela já não estiver habilitada
            if (!gm.ui.escapeWindow.activeInHierarchy)
            {
                gm.ui.MostrarJanela(gm.ui.escapeWindow);
            }
        }
        #endif

        //Se for no Android
        #if UNITY_ANDROID
                    if(Input.GetKey(KeyCode.Escape)) //Botão de Retorno do Celular
                    {
                        if (!gm.ui.escapeWindow.activeInHierarchy)
                        {
                            gm.ui.MostrarJanela(gm.ui.escapeWindow);
                        }
                    }
        #endif
    }//END UPDATE

    public void Ataque() //Chamado pelo clique do botão esquerdo do Mouse no Dektop ou pelo botão na Tela quando no Celular
    {
        if(Time.time > nextFire)
        {
            nextFire = Time.time + fireWait;
            anim.SetTrigger("Fire");
            vara.SetActive(true);
            sweepSFX.PlayDelayed(.2f);
            StartCoroutine(EsperaTempo(fireWait, vara));
        }
    }

    //FixedUpdate é Melhor para se ter um Movimento de mesma velocidade independente da velocidade da m�quina
    void FixedUpdate()
    {
        if(!gm.bloqueiaMovimento)
        {
#if UNITY_STANDALONE_WIN
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
#endif

#if UNITY_ANDROID
            float moveHorizontal = VirtualJoystick.joystick.Horizontal();//Input.GetAxis("Horizontal");
            float moveVertical = VirtualJoystick.joystick.Vertical();//Input.GetAxis("Vertical");
#endif

            Vector2 normalize = new Vector2(moveHorizontal, moveVertical);
            normalize = Vector2.ClampMagnitude(normalize, 1);
            Vector2 newPos = new Vector2(rigid2d.position.x + normalize.x * movementSpeed, rigid2d.position.y + normalize.y * movementSpeed);
            rigid2d.position = newPos;

            rigid2d.position = new Vector2
            (
                Mathf.Clamp(rigid2d.position.x, boundary.xMin, boundary.xMax),
                Mathf.Clamp(rigid2d.position.y, boundary.yMin, boundary.yMax)
            );

            rigid2d.MoveRotation(15 * moveVertical);
        }

    }

    //Função ativada quando algo entra no Collider2D da Jangada
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstaculo")
        {
            if (!imune && gm.continuaWaves) //"Continua Waves" fica Falso quando a Fase é encerrada. Por isso não queremos que o jogador tome dano quando este estiver falso
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
            if (other.GetComponent<Obstaculo>().lentidao)
            {
                movementSpeed = movementSpeed * .5f; //Diminui a velocidade pela metade;
                StartCoroutine(RestauraVelocidade(2f));
            }

        }
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

    //Tempo para a velocidade retornar ao normal após uma diminuição ou aumento nela
    IEnumerator RestauraVelocidade(float waitSecs)
    {
        yield return new WaitForSeconds(waitSecs);
        if (movementSpeed != veloInicial)
        {
            movementSpeed = veloInicial;
        }
    }


}//FIM
