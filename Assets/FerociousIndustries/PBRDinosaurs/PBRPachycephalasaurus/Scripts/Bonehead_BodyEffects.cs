using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonehead_BodyEffects : MonoBehaviour
{
    [SerializeField] BlendShapeTrigger throatTrigger;
    [SerializeField] BlendShapeTrigger gutTrigger;
    [SerializeField] BlendShapeTrigger legTwitchTrigger;

    public void ThroatEffect()
    {
        if (throatTrigger != null)
            throatTrigger.BlendA(false);
    }
    public void GutEffect()
    {
        if (gutTrigger != null)
            gutTrigger.BlendA(false);
    }
    public void LegTwitchEffect()
    {
        if (legTwitchTrigger != null)
            legTwitchTrigger.BlendA(false);
    }
}
