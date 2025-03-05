using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static List<Transform> targetPool;
    public static DistanceClass dc;

    [SerializeField]
    private string hitName;

    //SO TARGETS DO ADD THEMSELVES
    Camera cam;
    Plane[] cameraFrustrum;
    Collider m_collider;
    Bounds bounds;

    [SerializeField]
    bool visible;

    //a delegate here??
    //target removed...
    //Human subscribes to it so that what, when a target is popped, the dTarget can be nullified

    //only ever 1 target right??
    public delegate void OnTargetRemove();
    public static OnTargetRemove onTargetRemove;

    // Start is called before the first frame update
    void Start()
    {
        //maybe each target should just add itself to the target pool
        //then the Human script can deduce distances and things like that...
        cam = Camera.main;
        m_collider = GetComponent<Collider>();

        if (targetPool == null) {
            targetPool = new List<Transform>();
        }

        if (dc == null) {
            dc = new DistanceClass();
        }

        targetPool.Clear();
    }

    private void Update()
    {
        bounds = m_collider.bounds;
        cameraFrustrum = GeometryUtility.CalculateFrustumPlanes(cam);

        visible = GeometryUtility.TestPlanesAABB(cameraFrustrum, bounds);

        //ISSUE: visible is true even when collider behind a wall
        //Raycast could be a simple fix...

        //Raycast from camera position to collider

        //was not marked visible AND is visible - become visible
        if (visible)
        {
            //visible in frustrum
            //make sure no obstacles in the way
            Vector3 camDirection = cam.transform.position - transform.position;

            if (Physics.Raycast(transform.position, camDirection, out RaycastHit hitInfo, camDirection.magnitude))
            {

                //i guess being here means you hit something
                hitName = hitInfo.collider.name;

                //only remove a target if the collider is NOT another target
                //the collider does NOT contain a Target scipt, IS NOT A TARGET SO REMOVE
                if (!hitInfo.transform.GetComponent<Target>() && !hitInfo.collider.CompareTag("Player")) {
                    RemoveTarget();
                }

                

            }
            else {

                hitName = "";
                //nothing in the way
                if (!targetPool.Contains(this.transform))
                {
                    targetPool.Add(this.transform);
                    onTargetRemove?.Invoke();
                }
            }

        }
        else{
            RemoveTarget();
        }
    }

    private void RemoveTarget() {
        if (targetPool.Contains(this.transform))
        {
            targetPool.Remove(this.transform);
            onTargetRemove?.Invoke();
        }
    }

    public class DistanceClass : IComparer <Transform> {

        //public static Transform GetNearestToCenter(Transform x, Transform y) {
        //    Camera camCache = Camera.main; //cache camera for a bit faster processing

        //    //take position of target X and get it's relative distance to center of screen
        //    Vector3 viewPortPointX = camCache.WorldToViewportPoint(x.position);
        //    Vector2 viewPortLateralX = new Vector2(viewPortPointX.x, viewPortPointX.y);
        //    //float targetViewDistanceX = Vector2.Distance((Vector2.one * 0.5f), viewPortLateralX); //NOTE: previously compared distancec to Vector2.one * 0.5 > (0.5f, 0.5f) i.e. the middle of the screen
        //    float targetViewDistanceX = Vector2.Distance((Vector2.one * 0.5f), viewPortLateralX); //NOW: comparing against left side of screen to order targets from left to right i.e. (Vector3.up * 0.5f) > (0.0f, 0.5f)

        //    //take position of target Y and get it's relative distance to center of screen
        //    Vector3 viewPortPointY = camCache.WorldToViewportPoint(y.position);
        //    Vector2 viewPortLateralY = new Vector2(viewPortPointY.x, viewPortPointY.y);
        //    //float targetViewDistanceY = Vector2.Distance((Vector2.one * 0.5f), viewPortLateralY);
        //    float targetViewDistanceY = Vector2.Distance((Vector2.one * 0.5f), viewPortLateralY);

        //    //OK check it's distance to 0 and not from center..
        //    //if a distance is shorter to the left/0 side of the screen 
        //    if (targetViewDistanceX < targetViewDistanceY)
        //        return x;
        //    else
        //        return y;
        //}

        public int Compare(Transform x, Transform y) {

            Camera camCache = Camera.main; //cache camera for a bit faster processing

            //take position of target X and get it's relative distance to center of screen
            Vector3 viewPortPointX = camCache.WorldToViewportPoint(x.position);
            //Vector2 viewPortLateralX = new Vector2(viewPortPointX.x, viewPortPointX.y);
            ////float targetViewDistanceX = Vector2.Distance((Vector2.one * 0.5f), viewPortLateralX); //NOTE: previously compared distancec to Vector2.one * 0.5 > (0.5f, 0.5f) i.e. the middle of the screen
            //float targetViewDistanceX = Vector2.Distance((Vector2.up * 0.5f), viewPortLateralX); //NOW: comparing against left side of screen to order targets from left to right i.e. (Vector3.up * 0.5f) > (0.0f, 0.5f)

            //take position of target Y and get it's relative distance to center of screen
            Vector3 viewPortPointY = camCache.WorldToViewportPoint(y.position);
            //Vector2 viewPortLateralY = new Vector2(viewPortPointY.x, viewPortPointY.y);
            ////float targetViewDistanceY = Vector2.Distance((Vector2.one * 0.5f), viewPortLateralY);
            //float targetViewDistanceY = Vector2.Distance((Vector2.up * 0.5f), viewPortLateralY);

            //OK check it's distance to 0 and not from center..
            //if a distance is shorter to the left/0 side of the screen 
            //if (targetViewDistanceX < targetViewDistanceY)
            //    return 0;
            //else
            //    return 1;

            if (viewPortPointX.x < viewPortPointY.x)
                return -1;
            else if (viewPortPointX.x == viewPortPointY.x)
                return 0;
            else
                return 1;

            #region PlayerDistanceCompare
            //so an object closer to center is put in front right?
            //but between two objects, the closer to center wins out...
            //always?? u know what, let's just make cnter priority and see what happens

            //if (GameController.instance && GameController.instance.human) {

            //    check distance from player, whichever is closer to player gets returned
            //    if (GameController.instance.human.m_player)
            //    {

            //        check distance of player to target
            //        float xDist = Vector3.Distance(x.position, GameController.instance.human.m_player.transform.position);
            //        float yDist = Vector3.Distance(y.position, GameController.instance.human.m_player.transform.position);

            //        if (xDist < yDist)
            //            return 0;
            //        else
            //            return 1;
            //    }
            //    else
            //        return 0;

            //}

            //return 0;
            #endregion PlayerDistanceCompare

        }

    }

    public static Transform GetDesiredTarget() {

        //so this is called the moment you try and focus
        //this is where we sort and return middle index...
        //scrolling through targets is done after this too, where the targets are sorted, but since it pulls from the same static target pool, that means targets no longer in view are not accessible...
        //new targets entering are added to the end though, and are not sorted..
        //so before a flick, re-sort OK
        if (Target.targetPool != null) {


            Target.targetPool.Sort(Target.dc);

            Transform nearestTarget = null;
            float nearestDistance = 100.0f;
            float furthestDistance = 100.0f;

            foreach (Transform t in Target.targetPool)
            {
                //check angle between camera forward and 

                //i mean, really ur looking for closest target to center, followed by closest to range and/or in range
                Vector3 viewPortPoint = Camera.main.WorldToViewportPoint(t.position);

                Vector2 viewPortLateral = new Vector2(viewPortPoint.x, viewPortPoint.y);

                float viewPortDistance = viewPortPoint.z; //this is how far away they are from the frustrum

                float targetViewDistance = Vector2.Distance((Vector2.one * 0.5f), viewPortLateral);

                //additional logic required for when a target is closer to player but further from center...
                //IDK YET, A DOUBLE MIN CHECK??
                //LIKE HERE ARE two values: closest to center AND closest to player
                //whoever is the min in both...
                //no idk, like track the closest and if it's within a certain angle from camera than it becomes the desired target

                if (targetViewDistance < nearestDistance)
                {
                    nearestDistance = targetViewDistance;
                    nearestTarget = t;
                }

                //this tracks the closest target yeh? and this should supersede the above set as long as the target is within a certain range
                //you can have a target closer but have it be not the center object so as long as the viewPortPoint is within this range, set it...
                if (viewPortDistance < furthestDistance)
                {
                    furthestDistance = viewPortDistance;

                    //Gonna focus on fixing the camera Lock before testing this
                    //if (targetViewDistance < 0.1f) //this says that this object is the closest to player, but it only becomes the desired Target if it's within a certain range from the center
                    //    nearestTarget = t;
                }
            }

            return nearestTarget;

    }
    return null;

}
