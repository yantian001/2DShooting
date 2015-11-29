using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public interface IJoyStickEventHandler : IEventSystemHandler{

     void OnJoystickMoved(Vector3 deltaDistance);
}
