using BehaviourTree;

namespace Enemy
{
    // 적의 순찰을 구현하는 클래스
    public class Patrol : Node
    {
        // 적(Enemy) 클래스
        private Enemy _enemy;

        // 생성자
        public Patrol(Enemy enemy)
        {
            _enemy = enemy;
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            // 순찰한다.
            DoPatrol();

            // 성공 상태를 반환한다.
            return NodeState.SUCCESS;
        }

        // 순찰을 담당하는 함수
        public void DoPatrol()
        {
            // 순찰 함수를 실행한다.
            _enemy.Patrol();
        }
    }
}
