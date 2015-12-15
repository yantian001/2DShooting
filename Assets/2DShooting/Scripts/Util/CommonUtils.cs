using UnityEngine;
using System.Collections;

public class CommonUtils  {

    /// <summary>
    /// 是否能正常联网
    /// </summary>
    /// <returns></returns>
    public static bool IsNetworkOk()
    {
        return !(Application.internetReachability == NetworkReachability.NotReachable);
    }
}
