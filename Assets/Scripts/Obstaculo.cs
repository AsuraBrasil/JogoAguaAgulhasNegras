using UnityEngine;
using System.Collections;

public class Obstaculo : MonoBehaviour
{
    //GameManager
    //GameManager gm = GameManager.gm;

    public int dano = 1;
    public float speed = 0.1f;

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(transform.position.x + 1f * -speed, transform.position.y, transform.position.z);
        transform.position = newPos;

        if (this.gameObject.transform.position.x < -11)
        {
            DestroyObject(this.gameObject);
        }
    }
}