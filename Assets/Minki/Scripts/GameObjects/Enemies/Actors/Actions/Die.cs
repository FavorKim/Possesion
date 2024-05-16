using BehaviourTree;

namespace Enemy
{
    public class Die : Node
    {
        // 적(Enemy) 클래스
        private Enemy _enemy;

        // 생성자
        public Die(Enemy enemy)
        {
            _enemy = enemy;
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            state = NodeState.SUCCESS;
            return state;
        }

        // 피격을 담당하는 함수
        private void DoDie()
        {
            // 피격 함수를 실행한다.
            _enemy.Die();
        }
    }
}