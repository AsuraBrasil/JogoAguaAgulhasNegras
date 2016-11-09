using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public static VirtualJoystick joystick = null;

    private Image bgImg;
    private Image analogImg;
    private Vector3 inputVector;

    void Awake()
    {
        if (joystick == null)
        {
            joystick = this;
        }
        else if (joystick != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
#if UNITY_STANDALONE_WIN
        //Destroy objetos relacionados ao Mobile
          foreach(GameObject g in GameObject.FindGameObjectsWithTag("Mobile"))
        {
            Destroy(g);
        }
#endif

        bgImg = GetComponent<Image>();
        analogImg = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform
                                                                     , ped.position
                                                                     , ped.pressEventCamera
                                                                     , out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 - 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            //Move Analog Image
            analogImg.rectTransform.anchoredPosition =
                new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3)
                            , inputVector.z * (bgImg.rectTransform.sizeDelta.y / 3)); //With only two parameters, Z axis becomes 0 automatically*

            //Debug.Log(inputVector + "//" + inputVector.magnitude); //Up (0,0,1) Down(0,0,-1) //Left (-1,0,0) Right(1,0,0)
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        analogImg.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        if (inputVector.z != 0)
            return inputVector.z;
        else
            return Input.GetAxis("Vertical");
    }

}//FIM
