using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class DragRagdoll : MonoBehaviour
{
    [SerializeField] Rigidbody selectedRb;
    [SerializeField] float followSpeed = 10f;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] internal CharacterBehaviour character;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Vector3 offset;
    [SerializeField] GameObject selectedSeat;
    private bool isDragging = false;

    public void SetCharacter(CharacterBehaviour characterBehaviour)
    {
        character = characterBehaviour;
        selectedRb = character.head;
        character.OpenRagdoll();
        lineRenderer.positionCount = 2;
        isDragging = true;
        selectedRb.isKinematic = true;
        selectedRb.transform.DORotate(new Vector3(-30, 180, 0), .2f);
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(AnimatorActivate());
            lineRenderer.positionCount = 0;
            isDragging = false;
            selectedRb.isKinematic = false;
        }
    }
    IEnumerator AnimatorActivate()
    {
        yield return new WaitForSeconds(0f);
        if (!selectedSeat)
            Destroy(character.gameObject);
        else
            StartCoroutine(character.BlendToAnimation(selectedSeat));
        selectedSeat = null;
    }
    void FixedUpdate()
    {
        if (isDragging)
        {
            Vector3 targetPosition = GetMouseWorldPosition();
            // Hips'i hareket ettir
            selectedRb.MovePosition(Vector3.Lerp(selectedRb.position, targetPosition, Time.fixedDeltaTime * followSpeed));

            // LineRenderer güncelle
            lineRenderer.SetPosition(0, selectedRb.position); // Çizginin baþlangýcý hips konumu
            lineRenderer.SetPosition(1, GetMouseRaycastHit()); // Çizginin ucu mouse pozisyonu
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(selectedRb.position).z; // Derinlik korunsun
        Vector3 returnedPos = Camera.main.ScreenToWorldPoint(mousePos);
        returnedPos.y = offset.y;
        returnedPos.z = returnedPos.z + offset.z;
        returnedPos.x = returnedPos.x + offset.x;
        return returnedPos;
    }
    Vector3 GetMouseRaycastHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            selectedSeat = hit.collider.gameObject;
            return hit.point; // Çarptýðý noktanýn dünya pozisyonu
        }

        return Vector3.zero; // Eðer çarpmazsa boþ bir deðer döndür
    }
}
