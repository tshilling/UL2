using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderWater : MonoBehaviour {
    //Define variable
    public float underwaterLevel = 26.2f;

    //The scene's default fog settings
    private bool defaultFog;
    private Color defaultFogColor;
    private float defaultFogDensity;
    private Material defaultSkybox;
    private Material noSkybox;
    // Use this for initialization
    void Start () {
        GetComponent<Camera>().backgroundColor = new Color(0, 0.3f, 0.5f, 1);
        defaultFog = RenderSettings.fog;
    defaultFogColor = RenderSettings.fogColor;
    defaultFogDensity = RenderSettings.fogDensity;
    defaultSkybox = RenderSettings.skybox;
}

    // Update is called once per frame
    public bool Under = false;
    void Update()
    {
        if (Under)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0, 0.1f, 0.4f, 0.4f);
            RenderSettings.fogDensity = 0.1f;
            RenderSettings.skybox = noSkybox;
        }
        else
        {
            RenderSettings.fog = defaultFog;
            RenderSettings.fogColor = defaultFogColor;
            RenderSettings.fogDensity = defaultFogDensity;
            RenderSettings.skybox = defaultSkybox;
        }
    }

}
