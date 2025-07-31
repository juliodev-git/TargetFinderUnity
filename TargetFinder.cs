using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class TargetFinder : MonoBehaviour
{

    public static List<Transform> pool;
    public static DistanceClass dc;

    private Transform currentTarget;
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera targetCamera;
    [SerializeField] private Animator targetAnimator;

    public List<Transform> poolView;

    private bool lockedOn;
    private bool coolDown;

    #region Input
    private DefaultInputActions input;

    private void Awake()
    {
        input = new DefaultInputActions();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    #endregion Input

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pool == null) {
            pool = new List<Transform>();
        }

        if (dc == null) {
            dc = new DistanceClass();
        }
    }

    // Update is called once per frame
    void Update()
    {
        poolView = pool;

        if (input.Player.Fire.IsPressed() && !lockedOn)
        {
            LockOn();
        }
        else {
            if (!input.Player.Fire.IsPressed()) {
                LockOff();
            }
        }

        if (lockedOn) {
            if (input.Player.Look.ReadValue<Vector2>().x > 5.0f)
            {
                //select the target on the right
                SelectTarget(1);
            }
            else if (input.Player.Look.ReadValue<Vector2>().x <= -5.0f)
            {
                //select target on left
                SelectTarget(-1);
            }
            else
            {
                //nothing happens
            }
        }

       
    }

    private void LockOn() {

        currentTarget = NearestTarget();

        if (currentTarget) {
            targetCamera.LookAt = currentTarget;
            targetCamera.gameObject.SetActive(true);

            playerCamera.gameObject.SetActive(false);
            lockedOn = true;
            targetAnimator.SetBool("lockedOn", true);
        }

    }

    private void LockOff() {

        currentTarget = null;

        targetCamera.LookAt = null;
        targetCamera.gameObject.SetActive(false);

        playerCamera.gameObject.SetActive(true);
        lockedOn = false;
        targetAnimator.SetBool("lockedOn", false);
    }

    //next is either 1 or -1 > represents moving to the left or right of our pool
    private void SelectTarget(int next) {
        if (!coolDown && pool != null && pool.Count > 1) {
            System.Predicate<Transform> predicate = FindTransform;
            int currentIndex = pool.FindIndex(predicate);

            int nextIndex = currentIndex + next; // 0 1 2

            if (nextIndex >= 0 && nextIndex < pool.Count) {
                currentTarget = pool[nextIndex];
                targetCamera.LookAt = currentTarget;
                coolDown = true;
                StartCoroutine("ResetCoolDown");
            }

        }
    }

    private bool FindTransform(Transform t) { 
        return t.Equals(currentTarget);
    }

    IEnumerator ResetCoolDown() {
        yield return new WaitForSeconds(0.5f);
        coolDown = false;
    }

    //Go through pool and return the target nearest the center of the screen
    private Transform NearestTarget() {


        if (pool != null && pool.Count > 0) {
            Vector3 center = new Vector3(0.5f, 0.5f, 0.0f);
            Camera cam = Camera.main;

            int minIndex = 0;
            float shortestDistance = 1000.0f;

            for (int i = 0; i < pool.Count; i++)
            {
                Vector3 targetViewport = cam.WorldToViewportPoint(pool[i].position);
                targetViewport -= Vector3.forward * targetViewport.z;

                float targetDistanceFromCenter = Vector3.Distance(targetViewport, center);

                if (targetDistanceFromCenter < shortestDistance)
                {
                    shortestDistance = targetDistanceFromCenter;
                    minIndex = i;
                }

            }

            return pool[minIndex];
        }

        return null;
    }

    public static void AddToPool(Transform target) {
        if (pool != null && !pool.Contains(target)) {
            pool.Add(target);
            pool.Sort(dc);
        }
    }

    public static void RemoveFromPool(Transform target) {
        if (pool != null && pool.Contains(target))
        {
            pool.Remove(target);
            pool.Sort(dc);
        }
    }
}

public class DistanceClass : IComparer<Transform> {
    public int Compare(Transform x, Transform y) {

        Camera cam = Camera.main;

        Vector3 xViewport = cam.WorldToViewportPoint(x.position);
        Vector3 yViewport = cam.WorldToViewportPoint(y.position);

        if (xViewport.x < yViewport.x)
        {
            return -1;
        }
        else if (xViewport.x == yViewport.x)
        {
            return 0;
        }
        else {
            return 1;
        }

    }
}
