using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StubState
{
    None = 0,
    Locked = 1,
    Open = 2,
    Select = 3,
    Stubed = 4,
}

public class StubNodeView : MonoBehaviour {

    [HideInInspector]
    public GameObject StubOpen_obj = null;
    [HideInInspector]
    public GameObject StubSelect_obj = null;
    [HideInInspector]
    public GameObject StubEffect_obj = null;
    [HideInInspector]
    public int StubPos = 0;

    private StubState _oldState = StubState.None;
    private StubState _curState = StubState.None;

    void Awake () {
        Transform mTransform = this.transform;
        StubOpen_obj = mTransform.Find("StubEffect_obj").gameObject;
        StubSelect_obj = mTransform.Find("StubSelect_obj").gameObject;
        StubEffect_obj = mTransform.Find("StubEffect_obj").gameObject;
    }

    public void SetStubPos(int pos)
    {
        this.StubPos = pos;
    }

    public void RevertState()
    {
        if (_oldState != StubState.None)
            SetStubState(_oldState);
    }

    public void SetStubState(StubState state)
    {
        switch (state)
        {
            case StubState.Locked:
                {
                    StubOpen_obj.SetActive(false);
                    StubSelect_obj.SetActive(false);
                    StubEffect_obj.SetActive(false);
                }
                break;
            case StubState.Open:
                {
                    StubOpen_obj.SetActive(true);
                    StubSelect_obj.SetActive(false);
                    StubEffect_obj.SetActive(false);
                }
                break;
            case StubState.Select:
                {
                    StubOpen_obj.SetActive(true);
                    StubSelect_obj.SetActive(true);
                    StubEffect_obj.SetActive(false);
                }
                break;
            case StubState.Stubed:
                {
                    StubSelect_obj.SetActive(false);
                    StubEffect_obj.SetActive(true);
                }
                break;
            default:
                break;
        }
        if (state != StubState.None)
        {
            _oldState = _curState;
            _curState = state;
        }
    }
}
