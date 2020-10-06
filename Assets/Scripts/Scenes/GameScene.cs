using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    class Test
    {
        public int id = 0;
    }

    class CoroutineTest : IEnumerable
    {
        // 1. 함수의 상태를 저장/복원 가능
        // -> 엄청 오래 걸리는 작업을 잠시 끊거나
        // -> 원하는 타이밍에 함수를 잠시 Stop/Start 하는 경우
        // 2. return -> 우리가 원하는 Type으로 가능
        public IEnumerator GetEnumerator()
        {
            yield return new Test() { id = 1 };
            yield return new Test() { id = 2 };
            yield return new Test() { id = 3 };
            yield return new Test() { id = 4 };
        }
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;
        Managers.UI.ShowSceneUI<UI_Inven>();

        CoroutineTest test = new CoroutineTest();
        foreach(int t in test)
        {
            Debug.Log(t);
        }
    }

    public override void Clear()
    {

    }

}
