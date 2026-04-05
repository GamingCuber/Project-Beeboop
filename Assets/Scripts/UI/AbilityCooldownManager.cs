using System;
using System.Collections;
using UnityEngine;

public class AbilityCooldownManager : MonoBehaviour
{
    public static AbilityCooldownManager Instance;

    public AbilityCooldownIcon jump;
    public AbilityCooldownIcon dash;
    public AbilityCooldownIcon hook;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        StartCoroutine(waitForPlayerManager());
    }

    private AbilityCooldownIcon getIcon(String ability)
    {
        switch(ability)
        {
            case("jump"): 
                return jump;
            case("dash"): 
                return dash;
            case("hook"): 
                return hook;
            default:
                return null;
        }
    }
    public void abilityUnlocked(String ability)
    {
        AbilityCooldownIcon icon = getIcon(ability);
        icon.gameObject.SetActive(true);
        icon.reveal();
    }

    public void onCD(String ability)
    {
        AbilityCooldownIcon icon = getIcon(ability);
        icon.showCover();
        if (ability == "dash") Debug.Log("hi");
    }

    public void offCD(String ability)
    {
        AbilityCooldownIcon icon = getIcon(ability);
        icon.hideCover();
        if (ability == "dash") Debug.Log("bye");
    }

    private IEnumerator waitForPlayerManager()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (PlayerStateManager.Instance == null) yield return wait;

        PlayerStateManager psm = PlayerStateManager.Instance;

        if (psm.getState().canDoubleJump)
        {
            jump.gameObject.SetActive(true);
            jump.upgradeOwned();
        } 
        if (psm.getState().canDash)
        {   
            dash.gameObject.SetActive(true);
            dash.upgradeOwned();
        } 
        if (psm.getState().canHook)
        {
            hook.gameObject.SetActive(true);
            hook.upgradeOwned();
        }
    }
}
