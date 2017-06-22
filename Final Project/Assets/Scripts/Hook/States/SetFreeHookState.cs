using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFreeHookState : AbstractHookState {

    public SetFreeHookState(hook pHook) : base(pHook)
    {
    }
    public override void Start()
    {
        GameManager.Hook.GetCone().SetActive(true);
        //Debug.Log("entered setfree state");
        GameManager.Scorehandler.BankScore();
        //basic.Scorehandler.ToggleHookScoreUI(false);
        for (int i = 0; i < _hook.FishOnHook.Count; i++)
        {

            GameManager.Fishspawner.QueueFishAgain(_hook.FishOnHook[i], false, true, true); //_hook.FishOnHook[i].SetState(fish.FishState.PiledUp);
        }
        _hook.FishOnHook.Clear();
        for (int i = 0; i < _hook.TrashOnHook.Count; i++) _hook.TrashOnHook[i].SetState(trash.TrashState.PiledUp);
        _hook.TrashOnHook.Clear();

        if (_hook.SpecialFish)
        {
            GameObject.Destroy(_hook.SpecialFish.gameObject);
            GameManager.GotSpecialFish = true;
        }

        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.Ocean);
        GameManager.Boat.SetState(boat.BoatState.Stationary);

        //GameManager.Radar.SetState(radar.RadarState.Pulse);
        SetState(hook.HookState.None);

        //This needs to be here otherwise the deployhookbutton only activates when you press the reel up button and the reel up doesn't deactivate
        if (TutorialUI.touchedScreen)
        {
            TutorialUI.DeployActive = true;
            TutorialUI.ReelUpActive = false;
        }
        
        
}
    public override void Update()
    {
    }

    public override void Refresh()
    {
        _hook.FishOnHook = null;
        _hook.FishOnHook = new List<fish>();
        _hook.TrashOnHook = null;
        _hook.TrashOnHook = new List<trash>();
    }
    public override hook.HookState StateType()
    {
        return hook.HookState.SetFree;
    }

}
