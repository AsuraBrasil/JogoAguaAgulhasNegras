using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

    public void Fechar()
    {
        Debug.Log("Fechou!");
        Application.Quit();
    }

}
