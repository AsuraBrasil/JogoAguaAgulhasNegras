using UnityEngine;
using System.Collections;
using System;

public class BackgroundMovement : MonoBehaviour {

    public Transform parte1;
    public Transform parte2;
    public Transform parte3;
    public Transform parte4;
    public float posFinal= -50f; //Primeira Fase= -50f
    public float moveSpeed= 0.36f; //0.36 = Leva 1 minuto e meio para acabar nesta velocidade;

    void Start()
    {
        StartCoroutine(ChecaProgresso());
    }
	
	void FixedUpdate ()
    {
        if (GameManager.gm.liberaFundo)
        {
            //Parte 1 + Collider que encerra a Fase
            if (parte1.position.x > posFinal)
            {
                MoveFundo(parte1, moveSpeed);
                //Parte 2
                //MoveFundo(parte2, moveSpeed/2); Esta camada causa um efeito melhor quando estatica
                //Parte 3
                MoveFundo(parte3, moveSpeed / 3);
                //Parte 4
                MoveFundo(parte4, moveSpeed / 4);
            }
            else
            {
                GameManager.gm.EncerraFase(); //Se a posi��o final j� foi alcan�ada, encerra a fase.
            }
        }
    }

    void MoveFundo(Transform parte, float speed)
    {
        Vector3 finalPos = new Vector3(posFinal, parte.position.y, parte.position.z);
        Vector3 novaPos = Vector3.MoveTowards(parte.position, finalPos, speed * Time.deltaTime);
        parte.position = novaPos;
    }

    IEnumerator ChecaProgresso()
    {
        while (GameManager.gm.liberaFundo)
        {
            float diff = Mathf.Abs(posFinal + 9f) - Mathf.Abs(parte1.position.x + 9f); //9f � por causa da posi��o do objeto Pai
            float percent = diff / (Mathf.Abs(posFinal + 9f));
           // Debug.Log(Mathf.Abs(posFinal + 9f) + "-" + Mathf.Abs(parte1.position.x + 9f) + "=" + diff + ", " + percent);
            GameManager.gm.ui.progressSlider.value = percent;   
            yield return new WaitForSeconds(.5f);
        }
    }

}//FIM
