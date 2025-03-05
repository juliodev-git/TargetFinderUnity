using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetViewer : MonoBehaviour
{
    [SerializeField]
    private List<Transform> targets;
    // Start is called before the first frame update
    void Start()
    {
        targets = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targets != null) {
            targets.Clear();

            if (targets.Count > 0) {
                foreach (Transform t in Target.targetPool)
                {
                    if (!targets.Contains(t))
                        targets.Add(t);
                }
            }
            
        }
    }
}
