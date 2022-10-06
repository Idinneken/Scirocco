using Extensions;
using UnityEngine;

public class GeoCheck : MonoBehaviour
{
    public LayerMask layerMask;    
    public CharacterController controller;    
    public Transform rayTransform, playerViewTransform;    

    private Vector3 rayStartPosition, forwardLineEndpoint, closestPoint, ledgePoint;    
    private bool inRangeOfLedge, ledgePointIsValid, facingWall;
    private RaycastHit forwardLineHit;  
    private float angleAgainstWall;
    

    void Update()
    {
        rayStartPosition = transform.position;             

        if (Input.GetKeyDown(KeyCode.K) && inRangeOfLedge /*&& ledgePointIsValid*/ && facingWall)
        {
            controller.ChangePos_(ledgePoint);
        }
    }

    void FixedUpdate()
    {
        if (Physics.Linecast(transform.position, (new Vector3(transform.forward.x, 0, transform.forward.z) * 100f) + rayStartPosition, out forwardLineHit, layerMask)) //if it hits
        {
            // print("line is hitting: " + forwardLineHit.collider.gameObject.ToString());
            forwardLineEndpoint = forwardLineHit.point;
        }
        else
        {
            // print("line isn't hitting");
            forwardLineEndpoint = (new Vector3(transform.forward.x, 0, transform.forward.z) * 100f) + rayStartPosition;
        }

        angleAgainstWall = Vector3.Angle(closestPoint - rayStartPosition, transform.forward);
        
        
        // rayStartPosition.AngleBetweenPoints_(closestPoint, forwardLineEndpoint);

        print(Mathf.Round(angleAgainstWall));


        if (angleAgainstWall < 50)
        {
            facingWall = true;
        }
        else
        {
            facingWall = false;
        }

        Debug.DrawLine(rayStartPosition, closestPoint, Color.cyan, Time.fixedDeltaTime);                                        
        Debug.DrawLine(rayStartPosition, forwardLineEndpoint, Color.red, Time.fixedDeltaTime);  
    }

    void OnTriggerEnter(Collider collider)
    {
        inRangeOfLedge = true;
    }
    
    void OnTriggerStay(Collider collider)
    {        
        if (collider.gameObject.IsOnLayer_(layerMask))
        {                             
            closestPoint = collider.ClosestPoint(rayStartPosition);         
            // print(Mathf.Round(rayStartPosition.AngleBetweenPoints_(closestPoint, forwardLineEndpoint))); 

            RaycastHit closestPointHit;
            Physics.Linecast(rayStartPosition, closestPoint, out closestPointHit, layerMask, QueryTriggerInteraction.Ignore);

            // print(closestPointHit.collider?.gameObject.ToString() + closestPointHit.normal);

            ledgePoint = closestPoint;

            // if (closestPointHit.normal.y >
        }

    }

    void OnTriggerExit(Collider collider)
    {
        inRangeOfLedge = false;
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
