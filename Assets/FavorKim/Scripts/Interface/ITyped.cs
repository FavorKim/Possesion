using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITyped
{
    public enum Type
    {
        NONE, AQUA , LEAF = 10, FIRE = 20, CUTTER
    }
    Type type { get; }

    /// <summary>
    /// 속성공격을 받았을 시의 행동
    /// </summary>
    /// <param name="type">공격자 속성</param>
    public void OnTypeAttacked(Obstacles attacker);
}
