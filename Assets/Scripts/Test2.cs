using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 //: MonoBehaviour
{


static void ExecuteOnce() {
    Debug.Log("EXECUTE ONCE");
    }

[RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
private static void StartUp() {
    Debug.Log("EXECUTE ONCE 2");
}


}
