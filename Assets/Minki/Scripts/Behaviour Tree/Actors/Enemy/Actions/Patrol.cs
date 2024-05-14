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
            DoPatrol();

            // 공격을 시전한 후, 이 시퀀스 행동을 종료한다.
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
