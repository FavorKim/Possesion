using BehaviourTree;

namespace Enemy
{
    // 적의 공격을 구현하는 클래스
    public class Attack : Node
    {
        // 적(Enemy) 클래스
        private Enemy _enemy;

        // 생성자
        public Attack(Enemy enemy)
        {
            _enemy = enemy;
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            DoAttack();

            // 공격을 시전한 후, 이 시퀀스 행동을 종료한다.
            return NodeState.SUCCESS;
        }

        // 공격을 담당하는 함수
        public void DoAttack()
        {
            // 공격 함수를 실행한다.
            _enemy.Attack();
        }
    }
}
