using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    
    void Start()
    {
        skinnedMeshRenderer.material.color = Util.GetColorByCode(LocalStorage.KartColor);
#if UNITY_EDITOR
      //  LocalStorage.MyPlayerPrefs.DeleteAllKeys();//TODO
#endif
    }
   
}