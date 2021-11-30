using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;
        Managers.UI.ShowSceneUI<UI_Inven>();
    }

    public override void Clear()
    {

    }
}
