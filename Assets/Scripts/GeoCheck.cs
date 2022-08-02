using Extensions;
using UnityEngine;

public class GeoCheck : MonoBehaviour
{
    public LayerMask layerMask;    
    public CharacterController controller;    
    public float maxHeightFromPlayer, minHeightFromPlayer, maxBreadthFromPlayer;
    
    internal bool inRangeOfLedge = false;

    private Vector3 rayStartPosition;    
    private Vector3 highestValidRayEndPosition, lowestValidRayEndPosition;
    private Vector3 closestPoint;    
    

    private float ledgePointRayLength;
    private bool ledgePointIsValid;

    void Update()
    {
        rayStartPosition = transform.position;
    }
    
    void OnTriggerStay(Collider geoCollider)
    {
        if (geoCollider.gameObject.IsOnLayer_(layerMask))
        {
            closestPoint = geoCollider.ClosestPoint(rayStartPosition);
            Vector3 angleRayStartPoint = new Vector3(transform.position.x, closestPoint.y, transform.position.z);            
            
            Ray angleRay = new Ray(angleRayStartPoint, transform.forward);
            RaycastHit angleRayhit = new();
            geoCollider.Raycast(angleRay, out angleRayhit, Mathf.Infinity);
            Vector3 angleRayEndPoint = angleRayhit.point;

            // float angleBetween = Vector3.SignedAngle(angleRayStartPoint, angleRayEndPoint, closestPoint);
            float angleBetween = Vector3.Angle(closestPoint.normalized, angleRayEndPoint.normalized);

            print(angleBetween);


            Debug.DrawLine(angleRayStartPoint, closestPoint, Color.blue, Time.deltaTime);
            Debug.DrawLine(angleRayStartPoint, angleRayEndPoint, Color.red, Time.deltaTime);
            

               
        }
    }
    
    
    
    #region 

    // void Update()
    // {
    //     rayStartPosition = rayStartTransform.position;

    //     if (inRangeOfLedge && Input.GetKey(KeyCode.G) && ledgePointIsValid)
    //     {
    //         controller.ChangePos_(ledgePoint);
    //     }

    //     if (inRangeOfLedge && ledgePointIsValid)
    //     {
    //         Debug.DrawLine(rayStartPosition, ledgePoint, Color.red, Time.deltaTime);
    //     }
    // }

    // void OnTriggerStay(Collider other)
    // {
    //     if (other.gameObject.IsOnLayer_(layerMask))
    //     {
    //         ledgePoint = other.ClosestPoint(rayStartPosition);
    //         inRangeOfLedge = true;
    //     }

    //     RaycastHit hit = new();

    //     Physics.Linecast(rayStartPosition, ledgePoint, out hit);

    //     // print(transform.position.Absolute_());

    //     Vector3 rayDiff = (rayStartPosition - ledgePoint).Absolute_();

    //     print(rayDiff);
    //     // print(Vector3.Distance(rayStartPosition, ledgePoint) < maxRayDistance);



    //     // print(hit.normal);

    // }

    #endregion

}
