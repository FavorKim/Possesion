using BehaviourTree;
using UnityEngine;

public class TaskPatrol : Node
{
    private Transform transform;
    private Transform[] wayPoints;

    private int currentWayPointIndex = 0;

    private float waitTime = 1.0f;
    private float waitCounter = 0.0f;
    private bool waiting = false;

    public TaskPatrol(Transform transform, Transform[] wayPoints)
    {
        this.transform = transform;
        this.wayPoints = wayPoints;
    }

    public override NodeState Evaluate()
    {
        if (waiting)
        {
            waitCounter += Time.deltaTime;

            if (waitCounter >= waitTime)
            {
                waiting = false;
            }
            else
            {
                Transform wayPoint = wayPoints[currentWayPointIndex];

                if (Vector3.Distance(transform.position, wayPoint.position) < 0.01f)
                {
                    transform.position = wayPoint.position;
                    waitCounter = 0.0f;
                    waiting = true;

                    currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Length;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, wayPoint.position, /*speed*/ 1 * Time.deltaTime);
                    transform.LookAt(wayPoint.position);
                }
            }
        }

        // 아래는 지워야 함.
        return NodeState.RUNNING;
    }
}
