using UnityEngine.Analytics;
using System.Collections.Generic;
class AnalysticUtil
{
    public static void TrackEvent(string evtName,IDictionary<string,object> param)
    {
        Analytics.CustomEvent(evtName, param);
    }
}

