using BehaviourTree;

namespace Enemy
{
    // 플레이어에게 공격을 받았는지를 판별하는 클래스
    public class CheckGetHit : Node
    {
        // 필드(Field)
        private bool _getHit;

        // 생성자
        public CheckGetHit(Enemy enemy)
        {
            _getHit = enemy._getHit;
        }

        public override NodeState Evaluate()
        {
            state = (_getHit) ? NodeState.SUCCESS : NodeState.FAILURE;
            return state;
        }
    }
}
