using UnityEngine;

public class Target : MonoBehaviour
{

    private Collider m_collider;
    private Camera cam;

    private Plane[] frustrum;
    private bool visible;
    private bool inPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_collider = GetComponent<Collider>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        frustrum = GeometryUtility.CalculateFrustumPlanes(cam);

        visible = GeometryUtility.TestPlanesAABB(frustrum, m_collider.bounds);

        if (visible && !inPool)
        {
            TargetFinder.AddToPool(this.transform);
            inPool = true;
        }
        else {

            //either we're not visible - or we are in the pool

            //remove ourselves from the target pool
            if (!visible) {
                TargetFinder.RemoveFromPool(this.transform);
                inPool = false;
            }

            
        }
    }

    //Tutorial Addendum 1 - Ensure target removes itself from target pool when target disabled or destroyed
    private void OnDisable()
    {
        TargetFinder.RemoveFromPool(this.transform);
    }

    private void OnDestroy()
    {
        TargetFinder.RemoveFromPool(this.transform);
    }
    //End TA
}
