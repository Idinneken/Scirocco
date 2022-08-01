using Extensions;
using UnityEngine;

public class GeoCheck : MonoBehaviour
{
    public LayerMask layerMask;
    public bool inRangeOfLedge = false;
    public Transform rayStartTransform;    
    public CharacterController controller;

    private Vector3 rayStartPosition;
    private Vector3 ledgePoint;    
    private float ledgePointRayLength;
    private bool ledgePointIsValid;    

    void Update()
    {
        rayStartPosition = rayStartTransform.position;

        if (inRangeOfLedge && Input.GetKeyDown(KeyCode.G) && ledgePointIsValid)
        {      
            controller.ChangePos_(ledgePoint);            
        }

        if(inRangeOfLedge && ledgePointIsValid)
        {
            Debug.DrawLine(rayStartPosition, ledgePoint, Color.red, Time.deltaTime);
        }        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.IsOnLayer_(layerMask))
        {
            ledgePoint = other.ClosestPoint(rayStartPosition);
            inRangeOfLedge = true;   
        }

        RaycastHit hit = new();
        Physics.Linecast(rayStartPosition, ledgePoint, out hit);

        if (hit.normal.y > 0.5f){
            ledgePointIsValid = true;
        }
        else{
            ledgePointIsValid = false;
        }

        // print(hit.normal);

    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.IsOnLayer_(layerMask))
        {
            inRangeOfLedge = false;    
        }
    }

}
