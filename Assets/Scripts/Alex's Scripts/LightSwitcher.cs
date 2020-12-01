using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitcher : MonoBehaviour
{
    /// <summary>
    /// Alexander
    /// Updated: 12-1-2020
    /// 
    /// Variables used to store the current light component, Light colors, and the change duration between the colors
    /// </summary>
    public float duration;
    public Color blue;
    public Color orange;
    Light light;

    /// <summary>
    /// Alexander
    /// Updated: 12-1-2020
    /// 
    /// Start is called before the first frame update
    /// When the program starts it will set the game component light to a variable to adjust
    /// </summary>
    void Start()
    {
        light = GetComponent<Light>();
    }

    /// <summary>
    /// Alexander
    /// Updated: 12-1-2020
    /// 
    /// Update is called once per frame
    /// Adjusts the light color based off a Color.Lerp
    /// </summary>
    void Update()
    {
        float transferTime = Mathf.PingPong(Time.time, duration) / duration;
        light.color = Color.Lerp(blue, orange, transferTime);
    }
}
