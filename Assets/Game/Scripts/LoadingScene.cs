using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI text;
    private void Awake()
    {
        DOTween.To(() => slider.value, x => slider.value = x, 100, 2).OnUpdate(() =>
        {
            int roundedValue = Mathf.RoundToInt(slider.value);
            text.text = $"Loading... {roundedValue}%";
        }).OnComplete(() => SceneManager.LoadScene(PlayerPrefs.GetInt("_level", 1)));
    }
}
