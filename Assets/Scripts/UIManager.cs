using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager ui = null;

    //Como cada fase possui uma UI similar por�m com dados diferentes (ex.: Fase 1, Fase 2, etc...). Esse script
    //cuida da UI de cada fase de maneira individual e depois passa para o Game Manager (Controlador).
    //UI:
    public Text scoreText; //Texto da Pontua��o
    public Text startText; //Texto de An�ncio da Fase
    public Slider progressSlider; //Slider da Barra de Progresso
    public GameObject completedWindow; //Janela de Conclus�o da Fase
    public GameObject escapeWindow; //Janela que pergunta se o jogador Deseja retornar a Seleção de Fase
    public Image[] hearts;

	void Start ()
    {
        if (ui == null) //Se n�o tiver nenhum elemento ainda referenciado a variavel estatica
        {
            ui = this; //Este se torna a referencia
        }
        else if (ui != this)
        {
            Destroy(gameObject); //Se n�o, destr�i este script
        }
        //
        if (GameManager.gm.ui == null)
        {
            GameManager.gm.ui = ui; //Conecta este script ao Script Controlador do Jogo
        }
        else if (GameManager.gm.ui != this)
        {
            Destroy(gameObject); //Se o script Controlador do Jogo j� possuir um Script de UI conectado, destr�i este
        }
    }

    //Função chamada por um botão para mostrar uma Janela
    public void MostrarJanela(GameObject janela)
    {
        if (janela != null)
        {
            janela.SetActive(true);
        }
    }

    //Função chamada por um botão para fechar uma Janela
    public void FecharJanela(GameObject janela)
    {
        if (janela != null)
        {
            janela.SetActive(false);
        }
    }

}
