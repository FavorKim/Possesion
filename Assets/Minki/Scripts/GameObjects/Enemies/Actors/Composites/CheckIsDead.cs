using BehaviourTree;

namespace Enemy
{
    // 적이 죽었는지를 판별하는 클래스
    public class CheckIsDead : Node
    {
        // 적(Enemy) 클래스
        private Enemy _enemy;

        // 생성자
        public CheckIsDead(Enemy enemy)
        {
            _enemy = enemy;
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            // 죽음 상태일 경우 성공 상태를, 아닐 경우 실패 상태를 반환한다.
            state = (_enemy.IsDead) ? NodeState.SUCCESS : NodeState.FAILURE;
            return state;
        }
    }
}
