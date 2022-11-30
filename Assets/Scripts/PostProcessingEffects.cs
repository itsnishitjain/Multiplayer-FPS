using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class PostProcessingEffects : MonoBehaviour
{
    public PostProcessVolume volume;
    

    private ChromaticAberration Chrome;
    private Vignette Vig;
    private DepthOfField Depth;
    
    void Start()
    {
        volume.profile.TryGetSettings(out Chrome);
        volume.profile.TryGetSettings(out Vig);

        Chrome.intensity.value = 0;
        Vig.intensity.value = 0;
    }

    void FixedUpdate()
    {
        bool isChrome = Input.GetKey(KeyCode.LeftShift);
        bool isChromeMovement = Input.GetKey(KeyCode.W);
        bool isVig = Input.GetKey(KeyCode.LeftShift);
        bool isVigMovement = Input.GetKey(KeyCode.W);  

       

        if(isChrome && isChromeMovement)
        {
            Chrome.intensity.value = Mathf.Lerp(Chrome.intensity.value, 1.5f, Time.deltaTime * 0.09f);
        }
        else
        {
            Chrome.intensity.value = Mathf.Lerp(Chrome.intensity.value, 0, Time.deltaTime - (Time.deltaTime * 0.1f));
        }

        if(isVig && isVigMovement)
        {
            Vig.intensity.value = Mathf.Lerp(Vig.intensity.value, 0.6f, Time.deltaTime * 0.08f);
        }
        else
        {
            Vig.intensity.value = Mathf.Lerp(Vig.intensity.value, 0, Time.deltaTime - (Time.deltaTime * 0.9f));
        }  
    }
}
