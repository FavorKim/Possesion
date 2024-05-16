using BehaviourTree;

namespace Enemy
{
    public class CheckIsDead : Node
    {
        // 필드(Field)
        private bool _isDead; // 죽음 여부를 판별하는 변수

        // 생성자
        public CheckIsDead(Enemy enemy)
        {
            _isDead = enemy._isDead;
        }

        public override NodeState Evaluate()
        {
            // 죽음 상태일 경우 Success를, 아닐 경우 Failure를 반환한다.
            state = (_isDead) ? NodeState.SUCCESS : NodeState.FAILURE;
            return state;
        }
    }
}
