using UnityEngine;

public class KartBodyColorScript : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    void Start()
    {
        skinnedMeshRenderer.material.color = Util.GetColorByCode(LocalStorage.KartColor);
        Invoke("SetKartColor",1);
    }

    private void SetKartColor()
    {
        CancelInvoke("SetKartColor");
        skinnedMeshRenderer.material.color = Util.GetColorByCode(LocalStorage.KartColor);
    }
}