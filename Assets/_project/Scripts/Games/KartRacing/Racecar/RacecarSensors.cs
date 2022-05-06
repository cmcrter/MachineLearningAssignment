////////////////////////////////////////////////////////////
// File: RacecarSenses.cs
// Author: Charles Carter
// Date Created: 05/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 06/05/22
// Brief: The senses an ML agent uses to drive
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class RacecarSensors : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private float MaxRayDist = 15f;

    [SerializeField]
    private Transform rayFrom;

    [SerializeField]
    private LayerMask thisLayer;

    private float[] currentNormalizedDistances = new float[10];
    private float[] currentHitDistances = new float[10];

    public bool collisionThisFrame;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        currentHitDistances = new float[10];
        currentNormalizedDistances = new float[10];
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        // 1 backwards ray, 2 side rays, 4 forward diagonal rays, 1 forwards ray
        if(Physics.Raycast(rayFrom.position, -transform.forward, out RaycastHit backHit, MaxRayDist, ~thisLayer))
        {
            currentHitDistances[0] = backHit.distance;
        }
        else
        {
            currentHitDistances[0] = MaxRayDist;
        }

        if(Physics.Raycast(rayFrom.position, transform.right, out RaycastHit rightHit, MaxRayDist, ~thisLayer))
        {
            currentHitDistances[1] = rightHit.distance;
        }
        else
        {
            currentHitDistances[1] = MaxRayDist;
        }

        if(Physics.Raycast(rayFrom.position, -transform.right, out RaycastHit leftHit, MaxRayDist, ~thisLayer))
        {
            currentHitDistances[2] = leftHit.distance;
        }
        else
        {
            currentHitDistances[2] = MaxRayDist;
        }

        Vector3 forwardLeft = Quaternion.AngleAxis(-45, transform.up) * transform.forward;
        if(Physics.Raycast(rayFrom.position, forwardLeft, out RaycastHit forwardLeftHit, MaxRayDist, ~thisLayer))
        {
            currentHitDistances[3] = forwardLeftHit.distance;
        }
        else
        {
            currentHitDistances[3] = MaxRayDist;
        }

        Vector3 forwardRight = Quaternion.AngleAxis(45, transform.up) * transform.forward;
        if(Physics.Raycast(rayFrom.position, forwardRight, out RaycastHit forwardRightHit, MaxRayDist, ~thisLayer))
        {
            currentHitDistances[4] = forwardRightHit.distance;
        }
        else
        {
            currentHitDistances[4] = MaxRayDist;
        }

        Vector3 forwardLeftTwo = Quaternion.AngleAxis(-25, transform.up) * transform.forward;
        if(Physics.Raycast(rayFrom.position, forwardLeftTwo, out RaycastHit forwardLeftHitTwo, MaxRayDist, ~thisLayer))
        {
            currentHitDistances[5] = forwardLeftHitTwo.distance;
        }
        else
        {
            currentHitDistances[5] = MaxRayDist;
        }

        Vector3 forwardRightTwo = Quaternion.AngleAxis(25, transform.up) * transform.forward;
        if(Physics.Raycast(rayFrom.position, forwardRightTwo, out RaycastHit forwardRightHitTwo, MaxRayDist, ~thisLayer))
        {
            currentHitDistances[6] = forwardRightHitTwo.distance;
        }
        else
        {
            currentHitDistances[6] = MaxRayDist;
        }

        Vector3 forwardLeftThree = Quaternion.AngleAxis(-75, transform.up) * transform.forward;
        if(Physics.Raycast(rayFrom.position, forwardLeftThree, out RaycastHit forwardLeftHitThree, MaxRayDist, ~thisLayer))
        {
            currentHitDistances[7] = forwardLeftHitThree.distance;
        }
        else
        {
            currentHitDistances[7] = MaxRayDist;
        }

        Vector3 forwardRightThree = Quaternion.AngleAxis(75, transform.up) * transform.forward;
        if(Physics.Raycast(rayFrom.position, forwardRightThree, out RaycastHit forwardRightHitThree, MaxRayDist, ~thisLayer))
        {
            currentHitDistances[8] = forwardRightHitThree.distance;
        }
        else
        {
            currentHitDistances[8] = MaxRayDist;
        }

        if(Physics.Raycast(rayFrom.position, transform.forward, out RaycastHit forwardHit, MaxRayDist, ~thisLayer))
        {
            currentHitDistances[9] = forwardHit.distance;
        }
        else
        {
            currentHitDistances[9] = MaxRayDist;
        }

        if(Debug.isDebugBuild)
        {
            Debug.DrawLine(rayFrom.position, backHit.point, Color.red);
            Debug.DrawLine(rayFrom.position, rightHit.point);
            Debug.DrawLine(rayFrom.position, leftHit.point);
            Debug.DrawLine(rayFrom.position, forwardRightHit.point, Color.cyan);
            Debug.DrawLine(rayFrom.position, forwardLeftHit.point, Color.cyan);
            Debug.DrawLine(rayFrom.position, forwardRightHitTwo.point, Color.cyan);
            Debug.DrawLine(rayFrom.position, forwardLeftHitTwo.point, Color.cyan);
            Debug.DrawLine(rayFrom.position, forwardRightHitThree.point, Color.cyan);
            Debug.DrawLine(rayFrom.position, forwardLeftHitThree.point, Color.cyan);
            Debug.DrawLine(rayFrom.position, forwardHit.point, Color.blue);
        }
    }

    #endregion

    #region Public Methods

    public float[] returnNormalisedDistances() 
    {
        for(int i = 0; i < currentHitDistances.Length; ++i)
        {
            if(currentHitDistances[i] == 0)
            {
                currentNormalizedDistances[i] = 1f;
                continue;
            }

            currentNormalizedDistances[i] = currentHitDistances[i] / MaxRayDist;

        }

        return currentNormalizedDistances;
    }

    #endregion
}
