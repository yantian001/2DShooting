using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class JoystickPlane : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	public enum AxisOption
	{
		// Options for which axes to use
		Both, // Use both
		OnlyHorizontal, // Only horizontal
		OnlyVertical // Only vertical
	}

	public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
	public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
	public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

    public float moveRange = 100f;

    Vector2 m_LastPos;
    bool isFirstPressed = false;
	bool m_UseX; // Toggle for using the x axis
	bool m_UseY; // Toggle for using the Y axis
	CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
	CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

	void OnEnable()
	{
		CreateVirtualAxes();
	}

    void Start()
    {
    }

	void UpdateVirtualAxes(Vector3 delta)
	{
        delta /= moveRange;
		if (m_UseX)
		{
			m_HorizontalVirtualAxis.Update(delta.x);
		}

		if (m_UseY)
		{
			m_VerticalVirtualAxis.Update(delta.y);
		}
	}

	void CreateVirtualAxes()
	{
		// set axes to use
		m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
		m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

		// create new axes based on axes to use
		if (m_UseX)
		{
			m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
		}
		if (m_UseY)
		{
			m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
		}
	}


	public void OnDrag(PointerEventData data)
	{
        //if (!isFirstPressed)
       // {
            Debug.Log(data.position);
            Vector2 delta =( data.position - m_LastPos ) ;
            Debug.Log(delta);
            UpdateVirtualAxes(delta);
       // }
        m_LastPos = data.position;
        isFirstPressed = false;
        
	}


	public void OnPointerUp(PointerEventData data)
	{ 
        isFirstPressed = false;
        m_LastPos = Vector2.zero;
        UpdateVirtualAxes(Vector2.zero);
    }


	public void OnPointerDown(PointerEventData data)
    {
        m_LastPos = data.position;
        isFirstPressed = true;
    }

	void OnDisable()
		{
		if (m_UseX)
		{
			m_HorizontalVirtualAxis.Remove();
		}
		if (m_UseY)
		{
			m_VerticalVirtualAxis.Remove();
		}
	}
}