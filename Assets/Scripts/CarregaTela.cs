using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class CarregaTela : MonoBehaviour
{

    public bool destruirGM = false;

    public void CarregaPorIndex(int numeroTela)
    {
        if (destruirGM)
        {
            if (GameManager.gm != null)
            {
                Destroy(GameManager.gm.gameObject);
            }
        }
        SceneManager.LoadScene(numeroTela);
    }

    public void CarregaPorNome(string nomeTela)
    {
        if (destruirGM)
        {
            if (GameManager.gm != null)
            {
                Destroy(GameManager.gm.gameObject);
            }
        }
        SceneManager.LoadScene(nomeTela);
    }

    public void EsperaCarregaPorId(int numeroTela)
    {
        StartCoroutine(Esperar(2f, numeroTela));
    }

    public void CarregaMesma()
    {
        if (destruirGM)
        {
            if (GameManager.gm != null)
            {
                Destroy(GameManager.gm.gameObject);
            }
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CarregaSelecaoFases()
    {
        string nomeTela;
        if (GameManager.gm.dificuldade == GameManager.Dificuldade.normal)
        {
            nomeTela = "SelecaoDeFases";
        }
        else
        {
            nomeTela = "SelecaoDeFases_Fund";
        }

        if (destruirGM)
        {
            if (GameManager.gm != null)
            {
                Destroy(GameManager.gm.gameObject);
            }
        }
        SceneManager.LoadScene(nomeTela);
    }

    public void Fechar()
    {
        Debug.Log("Fechou!");
        Application.Quit();
    }

    private IEnumerator Esperar(float seg, int id)
    {
        yield return new WaitForSeconds(seg);
        if (destruirGM)
        {
            if (GameManager.gm != null)
            {
                Destroy(GameManager.gm.gameObject);
            }
        }
        SceneManager.LoadScene(id);
    }
}
