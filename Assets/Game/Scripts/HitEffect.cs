using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class HitEffect : MonoBehaviour
{
    string flashProperty = "_Flash_Amount";
    [SerializeField] float flashDuration = 0.1f;
    [SerializeField] int flashCount = 3;

    private List<Renderer> renderers = new List<Renderer>();

    void Start()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            Renderer rend = child.GetComponent<Renderer>();
            if (rend != null && child.gameObject.activeInHierarchy)
            {
                renderers.Add(rend);
            }
        }
    }

    public void PlayHitEffect()
    {
        foreach (var rend in renderers)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            rend.GetPropertyBlock(block);

            Sequence seq = DOTween.Sequence();
            for (int i = 0; i < flashCount; i++)
            {
                seq.Append(DOTween.To(() => block.GetFloat(flashProperty), x =>
                {
                    block.SetFloat(flashProperty, x);
                    rend.SetPropertyBlock(block);
                }, 1f, flashDuration));

                seq.Append(DOTween.To(() => block.GetFloat(flashProperty), x =>
                {
                    block.SetFloat(flashProperty, x);
                    rend.SetPropertyBlock(block);
                }, 0f, flashDuration));
            }
        }
    }
}
