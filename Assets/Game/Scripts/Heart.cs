using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] Image heartImage;
    [SerializeField] ParticleSystem vfx;
    Material fadeMat;

    private void Start()
    {
        // Yeni bir materyal kopyasý oluþturuyoruz, böylece sadece bu objeye özel olur
        fadeMat = Instantiate(heartImage.material);
        heartImage.material = fadeMat;
        fadeMat.SetFloat("_FadeAmount", 0);
    }

    public void Broke()
    {
        vfx.Simulate(0, true, true);
        vfx.Play();
        float value = 0;
        DOTween.To(() => value, x => value = x, 1, 1)
            .SetDelay(.5f)
            .OnUpdate(() => fadeMat.SetFloat("_FadeAmount", value));
    }
}
