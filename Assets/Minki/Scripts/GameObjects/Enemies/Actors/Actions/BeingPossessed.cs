using BehaviourTree;

namespace Enemy
{
    // 적이 플레이어에게 빙의되었을 때의 상태를 구현하는 클래스
    public class BeingPossessed : Node
    {
        // 적(Enemy) 클래스
        private Enemy _enemy;

        // 생성자
        public BeingPossessed(Enemy enemy)
        {
            _enemy = enemy;
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            // 빙의 상태가 된다.
            DoBeingPossessed();

            // 성공 상태를 반환한다.
            return NodeState.SUCCESS;
        }

        // 빙의 상태를 담당하는 함수
        public void DoBeingPossessed()
        {
            // 빙의 함수를 실행한다.
            //_enemy.BeingPossessed();
        }
    }
}
