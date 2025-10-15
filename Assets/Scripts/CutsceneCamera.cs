using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class CutsceneCamera : MonoBehaviour
{
    public Transform target;
    public Transform[] viewPoints;
    public float moveSpeed = 2f;
    public float rotateSpeed = 5f;
    public float pauseDuration = 2f;

    private int currentPoint = 0;
    private bool isMoving = false;

    void Start()
    {
        if (target == null || viewPoints.Length == 0)
        {
            Debug.LogError("CutsceneCamera: Missing target or viewpoints.");
            return;
        }

        transform.position = viewPoints[0].position;
        transform.LookAt(target);

        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        while (currentPoint < viewPoints.Length)
        {
            yield return StartCoroutine(MoveToPoint(viewPoints[currentPoint]));
            currentPoint++;
        }
        Debug.Log("Cutscene Finished");
    }

    IEnumerator MoveToPoint(Transform point)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        float t = 0;
        while (Vector3.Distance(transform.position, point.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(startPos, point.position, t);
            transform.rotation = Quaternion.Slerp(startRot, Quaternion.LookRotation(target.position - transform.position), t);

            t += Time.deltaTime * moveSpeed * 0.5f;
            yield return null;
        }
        transform.position = point.position;
        transform.LookAt(target);

        yield return new WaitForSeconds(pauseDuration);
        isMoving = false;
    }
}
