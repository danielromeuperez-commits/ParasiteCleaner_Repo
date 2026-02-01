using UnityEngine;

public class Buttonhow : MonoBehaviour
{
    public GameObject canvasApagar;
    public GameObject canvasEncender;

    public void Change()
    {
        canvasApagar.SetActive(false);
        canvasEncender.SetActive(true);
    }
}
