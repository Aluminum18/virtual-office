using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneTransition : SceneTransitionObject
{
    [SerializeField]
    private MoveTo _moveTo;

    protected override void AnimateClose(float duration)
    {
        base.AnimateClose(duration);
        _moveTo.StartMove(duration);
    }

    protected override void AnimateOpen(float duration)
    {
        base.AnimateOpen(duration);
        _moveTo.RevertMove(duration);
    }
}
