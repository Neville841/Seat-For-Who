using DG.Tweening;
using UnityEngine;

public class DragRagdoll : MonoBehaviour
{
    [SerializeField] Rigidbody selectedRb;
    [SerializeField] float followSpeed = 10f;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] GameObject selectedObject;

    [SerializeField] LayerMask groundMask;
    [SerializeField] Vector3 offset;

    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lineRenderer.positionCount = 2;
            isDragging = true;
            selectedRb.isKinematic = true;
            selectedRb.transform.DORotate(new Vector3(-30, 180, 0), .2f);
        }
        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.positionCount = 0;
            isDragging = false;
            selectedRb.isKinematic = false;
            selectedObject = null;
        }
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
            selectedObject = hit.collider.gameObject;
            return hit.point; // Çarptýðý noktanýn dünya pozisyonu
        }

        return Vector3.zero; // Eðer çarpmazsa boþ bir deðer döndür
    }
}
