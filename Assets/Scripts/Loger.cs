using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loger : MonoBehaviour
{
    public static Text text;



    public  static void Log(string _vlaue)
    {
        if(text!=null)
        {
            text.text = _vlaue;
        }

    }
}
