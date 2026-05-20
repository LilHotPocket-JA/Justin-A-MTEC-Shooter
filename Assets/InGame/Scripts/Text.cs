using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Text : MonoBehaviour
{
   public TextMeshProUGUI fuck;
    public int councount;
    public FPSControl control;

    public void Update()
    {
        councount = control.GetBodyCount();
       fuck.text = "Kill Count: "  + councount.ToString();
    }
}
