using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AgentCreator.PAT;
using QuickVR;

public class PATInitializer : MonoBehaviour
{

    [ButtonMethod]
    // Start is called before the first frame update
    public virtual void Init()
    {
        ProceduralAnimationToolsManager.CreateDefaultPAT(GetComponent<Animator>(), ProceduralAnimationToolsManager.AvatarType.Fuse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
