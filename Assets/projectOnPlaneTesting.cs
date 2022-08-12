using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

public class projectOnPlaneTesting : MonoBehaviour
{
    // public int rayLength;
    public Transform eyeSourceTransform, topHeightSourceTransform;    
    public LayerMask layerMask;    
    
    public bool showEyeRays, showClosestPointRays;       
    
    void Update()
    {
        RaycastHit eyeLevelHit;
        Ray eyeLevelRay = new Ray(eyeSourceTransform.position, eyeSourceTransform.forward); 

        if (Physics.Raycast(eyeLevelRay, out eyeLevelHit, Mathf.Infinity, layerMask) && showEyeRays)       
        {
            Vector3 projectOnPlaneDir = Vector3.ProjectOnPlane(transform.forward, eyeLevelHit.normal); //ProjectOnPlane direction

            RaycastHit surfaceNormalHit;
            Ray surfaceNormalRay = new Ray(eyeSourceTransform.position, -eyeLevelHit.normal);
            Physics.Raycast(surfaceNormalRay, out surfaceNormalHit, Mathf.Infinity, layerMask);

            Debug.DrawLine(eyeSourceTransform.position, eyeLevelHit.point, Color.red); //Eye - Looking at
            Debug.DrawRay(eyeLevelHit.point, projectOnPlaneDir, Color.green); //Looking at - Project on Plane Direction
            Debug.DrawRay(eyeLevelHit.point, eyeLevelHit.normal * 3, Color.blue); //Looking at - Surface normal                                    
            Debug.DrawLine(eyeSourceTransform.position, surfaceNormalHit.point, Color.cyan, Time.deltaTime);// from looking at (with negative surface normal)
        }   
    }

    void OnTriggerStay(Collider other)
    {                         
        RaycastHit topHeightHit;  
        Vector3 closestPoint = other.ClosestPoint(topHeightSourceTransform.position);                

        if (Physics.Linecast(topHeightSourceTransform.position, closestPoint, out topHeightHit, layerMask))
        {               
            Vector3 projectOnEdge = Vector3.ProjectOnPlane(topHeightHit.point, topHeightHit.normal);
            Vector3 wallDirection = (topHeightHit.point - topHeightSourceTransform.position).normalized;            
            wallDirection = new Vector3(wallDirection.x, wallDirection.y - 0.1f, wallDirection.z);

            // RaycastHit 
            // Ray


            Debug.DrawLine(topHeightSourceTransform.position, topHeightHit.point, Color.black);  //Above head - closestPoint
            Debug.DrawRay(topHeightSourceTransform.position, wallDirection * 10, Color.magenta); //Above head - (a bit lower than closestPoint)
        }
                    
        // RaycastHit topHeightWallHit;
        // Vector3 topHeightPointWallDirection = new Vector3(topHeightHit.point.x, topHeightHit.point.y - 0.1f, topHeightHit.point.z);

        

        // if (Physics.Raycast(topHeightSourceTransform.position, topHeightPointWallDirection.normalized, out topHeightWallHit, Mathf.Infinity, layerMask))
        // {
        //     Debug.DrawLine(topHeightSourceTransform.position, topHeightWallHit.point, Color.magenta);
        // }


        // Debug.DrawLine(topHeightSourceTransform.position, topHeightPointWall, Color.black);      

        
        
        #region 
        
        // if (other.gameObject.IsOnLayer_(layerMask) && showClosestPointRays)
        // {       
        //     RaycastHit topHeightHit;  
        //     Vector3 topHeightPoint = other.ClosestPoint(topHeightSourceTransform.position);
            

        //     if (Physics.Linecast(topHeightSourceTransform.position, topHeightPoint, out topHeightHit, layerMask))
        //     {   
        //         // Vector3 projectOnEdge = Vector3.ProjectOnPlane(topHeightHit.point, topHeightHit.normal);

        //         Debug.DrawLine(topHeightSourceTransform.position, topHeightHit.point, Color.black);        
        //         // Debug.DrawLine(topHeightHit.point, projectOnEdge, Color.magenta);
        //     }
                      
        //     // RaycastHit topHeightWallHit;
        //     // Vector3 topHeightPointWallDirection = new Vector3(topHeightHit.point.x, topHeightHit.point.y - 0.1f, topHeightHit.point.z);

            

        //     // if (Physics.Raycast(topHeightSourceTransform.position, topHeightPointWallDirection.normalized, out topHeightWallHit, Mathf.Infinity, layerMask))
        //     // {
        //     //     Debug.DrawLine(topHeightSourceTransform.position, topHeightWallHit.point, Color.magenta);
        //     // }


        //     // Debug.DrawLine(topHeightSourceTransform.position, topHeightPointWall, Color.black);      

        // }

        #endregion

    }

}
