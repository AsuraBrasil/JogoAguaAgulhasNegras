using UnityEngine;
using System.Collections;

public class FireButton : MonoBehaviour {

    public void Ataque()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Ataque();
    }

}
