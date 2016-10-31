using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class PerguntasManager : MonoBehaviour
{

    public float score; //Pontuação
    public int perguntaAtual; //Número da Pergunta Atual
    public Pergunta[] listaPerguntas; //Lista com as Perguntas
    public AudioSource acertouAudio;
    public AudioSource errouAudio;
    public AudioSource completionAudio;

    //UI
    public Text pgtNumeroText;
    public Text pgtTextoText;
    public Text respAText;
    public Text respBText;
    public Text respCText;
    public Text respDText;
    public Button btnConfirma; //Botão de Responder() e ProximaPergunta();
    public GameObject respBlock;
    public GameObject janelaDeConclusao;
    public Text scoreText;

    [HideInInspector]
    public string respEscolhida;
    private List<Text> listaRespostas;
    private Pergunta escolhida;
    private string respCerta;
    private int numDePerguntas; //Numero de perguntas


    void Start()
    {
        if (GameManager.gm != null)
        {
            score = GameManager.gm.score;
        }
        else
        {
            score = 0;
        }

        numDePerguntas = listaPerguntas.Length;
        perguntaAtual = 0;
        listaRespostas = new List<Text> { respAText, respBText, respCText, respDText };
        AtualizaPergunta();
    }

    public void AtualizaPergunta()
    {
        int rnd = UnityEngine.Random.Range(0, listaPerguntas.Length);
        escolhida = listaPerguntas[rnd];
        //
        List<Pergunta> temp = new List<Pergunta>(listaPerguntas);
        temp.Remove(listaPerguntas[rnd]); //Remove a pergunta escolhida da Lista e passa para UI
        listaPerguntas = temp.ToArray();
        //
        perguntaAtual++;
        pgtNumeroText.text = "Pergunta " + perguntaAtual.ToString();
        pgtTextoText.text = escolhida.pergunta;
        respCerta = escolhida.respostaCerta;
        respEscolhida = "";
        //
        ReiniciaCor();
        EmbaralhaRespostas();
        //
        btnConfirma.interactable = false; //Desabilita o Botão
        btnConfirma.gameObject.GetComponentInChildren<Text>().text = "Selecione uma Resposta";
        btnConfirma.onClick.RemoveAllListeners();
        btnConfirma.onClick.AddListener(() => Responder());
    }

    public void EmbaralhaRespostas()
    {
        List<string> listaRespostasP = new List<string> { escolhida.respostaCerta, escolhida.respostaErradaA, escolhida.respostaErradaB, escolhida.respostaErradaC };
        for (int i = 3; i >= 0; i--)
        {
            int rand = UnityEngine.Random.Range(0, i+1);
            Debug.Log(i + "e" + rand);
            listaRespostas[i].text = listaRespostasP[rand];
            listaRespostasP.Remove(listaRespostasP[rand]);
        }
    }

    public void ReiniciaCor()
    {
        foreach (Text t in listaRespostas)
        {
            t.gameObject.GetComponentInParent<Image>().color = Color.white;
        }
    }

    public void EscolherResposta(Text txt)
    {
        ReiniciaCor();
        txt.gameObject.GetComponentInParent<Image>().color = Color.yellow;
        respEscolhida = txt.text;
        //Botão da Resposta Escolhida fica amarelo
        btnConfirma.interactable = true; //Habilita o botão para responder
        btnConfirma.gameObject.GetComponentInChildren<Text>().text = "Confirmar";
    }

    public void Responder()
    {
       if(respEscolhida == "" || respEscolhida == null)
        {
            Debug.Log("Erro: Nenhuma resposta foi escolhida.");
        }
       else
        {
            if (respEscolhida == respCerta)
            {
                Debug.Log("Acertou!"); //Resposta escolhida fica verde
                acertouAudio.Play();
                foreach (Text t in listaRespostas)
                {
                    if(t.gameObject.GetComponentInParent<Image>().color == Color.yellow)
                    t.gameObject.GetComponentInParent<Image>().color = Color.green;
                }
                score += 100;
            }
            else
            {
                Debug.Log("Errou!"); //Resposta escolhida fica vermelha e Resposta certa verde
                errouAudio.Play();
                foreach (Text t in listaRespostas)
                {
                    if (t.gameObject.GetComponentInParent<Image>().color == Color.yellow)
                        t.gameObject.GetComponentInParent<Image>().color = Color.red;
                    if (t.text == respCerta)
                        t.gameObject.GetComponentInParent<Image>().color = Color.green;
                }

            }
            respBlock.SetActive(true);
            //Muda o texto do botão de "Responder" para "Próxima" e a sua função de "Responder()" para "ProximaPergunta()";
            btnConfirma.gameObject.GetComponentInChildren<Text>().text = "Próxima Pergunta";
            if (listaPerguntas.Length == 0)
            {
                btnConfirma.gameObject.GetComponentInChildren<Text>().text = "Ver Pontuação";
            }
            btnConfirma.onClick.RemoveAllListeners();
            btnConfirma.onClick.AddListener(() => ProximaPergunta());
        }
    }

    public void ProximaPergunta()
    {
        if(perguntaAtual < numDePerguntas)
        {
            AtualizaPergunta();
        }
        else
        {
            Debug.Log("Acabou");
            completionAudio.Play();
            //Chama Janela de Conclusão com a mensagem "Parabéns, sua pontuação foi: " e um botão para Seleção de Fases da Dificuldade
            janelaDeConclusao.SetActive(true);
            scoreText.text = score.ToString();

        }
        respBlock.SetActive(false);
    }

}//FIM
