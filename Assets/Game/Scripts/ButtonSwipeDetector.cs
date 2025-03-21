using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSwipeDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float triggerThreshold = 100f; // X birim, ne kadar yukarı çekerse void çalışsın
    private Vector2 startTouchPos;
    private bool isPressing = false;
    [SerializeField] GameObject character;
    [SerializeField] DragRagdoll dragRagdoll;
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressing = true;
        startTouchPos = eventData.position; // Parmağın başlangıç noktasını al
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isPressing) return;

        float deltaY = eventData.position.y - startTouchPos.y; // Ne kadar yukarı kaydırdı
        if (deltaY >= triggerThreshold)
        {
            TriggerFunction();
            isPressing = false; // Bir kere çalıştırıp durdur
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressing = false; // Parmağı kaldırınca sıfırla
    }

    private void TriggerFunction()
    {
        GameObject character = Instantiate(this.character);
        dragRagdoll.SetCharacter(character.GetComponent<CharacterBehaviour>());
    }
}
