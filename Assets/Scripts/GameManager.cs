using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public enum Dificuldade
    {
        facil,
        normal
    }

    //referencia ao Game Manager
    public static GameManager gm = null;

    //Estas informações deveriam estar em outro script (no de UI por exemplo ou algum específico para configuração da fase), 
    //porém como este jogo não mantém as informações, 
    //este pode (deve) ser destruído após as perguntas serem feitas!
    //Game Settings:
    public Dificuldade dificuldade = Dificuldade.facil;
    public float score = 0;
    public int vida = 6;
    public AudioSource scoringAudio; //Som produzido quando o lixo é coletado
    public AudioSource negativeAudio; //Som produzido quando o lixo é deixado para trás sem ser pego
    public AudioSource completionAudio; //Som produzido quando o jogador completa a fase
    public Vector2 spawnVal = new Vector2(-5,2); //(Min,Max) EixoY
    public int hazardCount; //Quantidade máxima de lixos por Onda
    public float spawnWait; //Tempo de espera para o próximo Lixo da Onda aparecer
    public float startWait; //Tempo de Espera para a primeira onda
    public float waveWait; //Tempo de espera por Onda de Lixos
    public GameObject[] lixos;
    public Sprite[] heartsSprite;

    //Public Variables that doesn't need to be shown in Inspector
    [HideInInspector]
    public bool liberaFundo = false; //libera o movimento do fundo
    [HideInInspector]
    public bool continuaWaves = true; //libera o movimento do fundo
    //[HideInInspector]
    public UIManager ui = null;
    [HideInInspector]
    public bool bloqueiaAtaque = false; //libera o movimento do fundo
    [HideInInspector]
    public bool bloqueiaMovimento = false; //libera o movimento do fundo

    void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else if (gm != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(StartText());
        StartCoroutine(SpawnWaves());
    }

    IEnumerator StartText()
    {
        yield return new WaitForSeconds(1f);
        ui.startText.text = "Preparado?";
        yield return new WaitForSeconds(1f);
        ui.startText.text = "Vai!";
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while(continuaWaves)
        {
            //Se for pausar o jogo ao pressionar Esc, colocar um "if(pausa)" aqui!
            for (int i = Random.Range(0, hazardCount-1); i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(10, UnityEngine.Random.Range(spawnVal.x, spawnVal.y), -.1f);
                //Quaternion spawnRotation = Quaternion.identity;
                GameObject gObj = lixos[UnityEngine.Random.Range(0, lixos.Length)];
                Instantiate(gObj, spawnPosition, /*spawnRotation*/ gObj.transform.rotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }

    public void Pontuar(float pontos)
    {
        score += pontos;
        if(score < 0)
        {
            score = 0;
        }
        if(pontos > 0) //Se os pontos são positivos, provenientes da coleta, faz o som de aumentar pontos.
        {
            scoringAudio.Play();
        }
        //Atualiza UI;
        ui.scoreText.text = "Pontos: " + score.ToString();
    }

    public void AtualizaVida(int valor)
    {
        int temp = vida + valor;
        if (temp <= 6) //No caso de algum item recuperar a vida para mais do que 6
        {
            vida = temp;
            if (vida < 0)
                vida = 0;
            temp = 0;
        }
        else
        {
            vida = 6;
        }
            switch(vida)
            {
                case 0:
                    ui.hearts[0].sprite = heartsSprite[0];
                    ui.hearts[1].sprite = heartsSprite[0];
                    ui.hearts[2].sprite = heartsSprite[0];
                    break;
                case 1:
                    ui.hearts[0].sprite = heartsSprite[1];
                    ui.hearts[1].sprite = heartsSprite[0];
                    ui.hearts[2].sprite = heartsSprite[0];
                    break;
                case 2:
                    ui.hearts[0].sprite = heartsSprite[2];
                    ui.hearts[1].sprite = heartsSprite[0];
                    ui.hearts[2].sprite = heartsSprite[0];
                    break;
                case 3:
                    ui.hearts[0].sprite = heartsSprite[2];
                    ui.hearts[1].sprite = heartsSprite[1];
                    ui.hearts[2].sprite = heartsSprite[0];
                    break;
                case 4:
                    ui.hearts[0].sprite = heartsSprite[2];
                    ui.hearts[1].sprite = heartsSprite[2];
                    ui.hearts[2].sprite = heartsSprite[0];
                    break;
                case 5:
                    ui.hearts[0].sprite = heartsSprite[2];
                    ui.hearts[1].sprite = heartsSprite[2];
                    ui.hearts[2].sprite = heartsSprite[1];
                    break;
                case 6:
                    ui.hearts[0].sprite = heartsSprite[2];
                    ui.hearts[1].sprite = heartsSprite[2];
                    ui.hearts[2].sprite = heartsSprite[2];
                    break;
        }
        if (vida == 0)
        {
            EncerraFase(); //Seria legal enviar uma mensagem de "Você perdeu!", mas por enquanto Ok.
        }
    }

    internal void EncerraFase()
    {
        Debug.Log("Acabou!");
        liberaFundo = false;
        continuaWaves = false;
        //Podia colocar uma só variavel 'acabou' para gerenciar tudo isso, mas já era -q
        StartCoroutine(EsperaWaveAcabar(5f));
        //Chama UI de Pontuação e Finalização da Fase, com botão para Prosseguir as Perguntas.
    }

    internal void EncerraFaseBoss()
    {
        Debug.Log("Acabou!");
        liberaFundo = false;
        continuaWaves = false;
        //Podia colocar uma só variavel 'acabou' para gerenciar tudo isso, mas já era -q
        StartCoroutine(EsperaWaveAcabar(0.2f));
        //Chama UI de Pontuação e Finalização da Fase, com botão para Prosseguir as Perguntas.
    }

    IEnumerator EsperaWaveAcabar(float secs)
    {
       
        if (ui.escapeWindow.activeInHierarchy)
        {
            ui.FecharJanela(ui.escapeWindow);
        }
        if (vida > 0)
        {
            yield return new WaitForSeconds(secs);
            bloqueiaAtaque = true;
            foreach (AudioSource aud in GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>())
            {
                aud.Stop();
            }
            completionAudio.Play();
            ui.MostrarJanela(ui.completedWindow); //Venceu!
        } 
        else
        {
            bloqueiaAtaque = true;
            ui.MostrarJanela(ui.failWindow); //Perdeu!
        }
       
    }
    

}//FIM
