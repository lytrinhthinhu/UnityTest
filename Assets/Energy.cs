using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    // Start is called before the first frame updated 
    public Slider slider;
    //public Gradient gradient;
    //private Color color;
    public Image fill;
    public void SetEnergy(float value, Color color)
    {
        slider.value = value;
        if((slider.maxValue - value) <= 0.1f)
        {
            if(color == Color.red)
                fill.color = Color.red;
            else
                fill.color = Color.blue;                
        }            
        else
        {
            if(color == Color.red)
                fill.color = new Color(0.8f, 0.2f, 0.0f);
            else
                fill.color = new Color(0.0f, 0.0f, 0.6f);
        }
            //fill.color = Color.Lerp()
    }

    public float GetEnemy()
    {
        return slider.value;
    }
}
