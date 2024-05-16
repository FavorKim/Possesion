using BehaviourTree;

namespace Enemy
{
    // 적의 죽음을 구현하는 클래스
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
            // 죽는다.
            DoDie();

            // 성공 상태를 반환한다.
            return NodeState.SUCCESS;
        }

        // 죽음을 담당하는 함수
        private void DoDie()
        {
            // 죽음 함수를 실행한다.
            _enemy.Die();
        }
    }
}