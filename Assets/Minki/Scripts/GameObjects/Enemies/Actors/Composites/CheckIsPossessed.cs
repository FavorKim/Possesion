using BehaviourTree;
using UnityEngine;

namespace Enemy
{
    public class CheckIsPossessed : Node
    {
        // 적(Enemy) 클래스
        private Enemy _enemy;

        // 필드(Field)
        private bool _isPossessed; // 빙의 상태 여부를 판별하는 변수

        // 생성자
        public CheckIsPossessed(Enemy enemy)
        {
            _enemy = enemy;
            _isPossessed = enemy.IsPossessed;
        }

        public override NodeState Evaluate()
        {
            // 빙의 상태일 경우 Success를, 아닐 경우 Failure를 반환한다.
            state = (_isPossessed) ? NodeState.SUCCESS : NodeState.FAILURE;
            return state;
        }
    }
}
