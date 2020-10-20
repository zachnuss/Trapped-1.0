using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pulsing_Visual : MonoBehaviour
{

    public Image effect;
    public float startAlpha;
    public float alphaMax;
    public float alphaMin;
    public float timeFactor = 1;

    private bool switchD;

    // Start is called before the first frame update
    void Start()
    {
        effect.color = new Color(effect.color.r, effect.color.g, effect.color.b, startAlpha);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (switchD)
        {
            if(effect.color.a < alphaMax)
            {

                effect.color = new Color(effect.color.r, effect.color.g, effect.color.b, effect.color.a+Time.deltaTime*timeFactor);

            }
            else
            switchD = !switchD;

        }
        if (!switchD)
        {
            if (effect.color.a > alphaMin)
            {

                effect.color = new Color(effect.color.r, effect.color.g, effect.color.b, effect.color.a - Time.deltaTime *timeFactor);

            }
            else
            switchD = !switchD;

        }
        
    }
}
