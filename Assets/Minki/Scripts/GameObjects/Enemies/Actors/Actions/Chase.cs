using BehaviourTree;

namespace Enemy
{
    // 적의 추적을 구현하는 클래스
    public class Chase : Node
    {
        // 적(Enemy) 클래스
        private Enemy _enemy;

        // 생성자
        public Chase(Enemy enemy)
        {
            _enemy = enemy;
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            // 추적한다.
            DoChase();

            // 성공 상태를 반환한다.
            return NodeState.SUCCESS;
        }

        // 추적을 담당하는 함수
        public void DoChase()
        {
            // 추적 함수를 실행한다.
            _enemy.Chase();
        }
    }
}
