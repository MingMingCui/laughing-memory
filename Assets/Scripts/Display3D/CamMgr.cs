using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    None = 0,
    Forward = 1,
    Inverse = 2,
}

public class CamMgr : MonoBehaviour {
    public float ChangeTime = 3f;
    private Camera _cam = null;
    private Vector3 _oldPos = Vector3.zero;
    private Transform _pos1;
    private Transform _pos2;
    private CameraState _state = CameraState.None;
    private float _lerp = 0;
    private float _round = 1;
    private GameObject _activeSkillCameraObj = null;
    private Camera _activeSkillCamera = null;
    private ScreenDissolve _dissolve = null;
    private Camera _frontCamera = null;
    private bool _startDissolve = false;
    private readonly float _dissolveSpeed = 1f;
    private float _activeSkillCamDepth = 0;

    public const int FightCamEffect = 10514;

    // Use this for initialization
    void Start () {
        _cam = this.GetComponent<Camera>();
        _activeSkillCameraObj = GameObject.Find("ActiveSkillCamera");
        _frontCamera = GameObject.Find("FrontCamera").GetComponent<Camera>();
        _dissolve = _frontCamera.gameObject.GetComponent<ScreenDissolve>();
        if (_activeSkillCameraObj != null)
        {
            _activeSkillCamera = _activeSkillCameraObj.GetComponent<Camera>();
            _activeSkillCamDepth = _activeSkillCamera.depth;
        }
        _pos1 = GameObject.Find("CameraPos_01").transform;
        _pos2 = GameObject.Find("CameraPos_02").transform;

	}

    public void PlayStartEffect()
    {
        GameObject effect = GameObject.Instantiate(ResourceMgr.Instance.LoadResource(FightCamEffect) as GameObject, _frontCamera.transform);
        effect.transform.localPosition = new Vector3(0, 0, 2);
    }

    public void StartDissolve()
    {
        _startDissolve = true;
    }

    public void OnGamePause(bool pause)
    {
        _activeSkillCamera.depth = pause ? 0 : _activeSkillCamDepth;
    }
	
	// Update is called once per frame
	void Update () {
        if (_state != CameraState.None)
        {
            _lerp += Time.deltaTime / ChangeTime;
            if (_lerp >= 1)
            {
                _lerp = 0;
                ZEventSystem.Dispatch(EventConst.OnCameraChangeOver, _state == CameraState.Forward);
                _state = CameraState.None;
                return;
            }
            this.transform.position = Vector3.Lerp(new Vector3(_pos1.position.x + getOffsetByRound(), _pos1.position.y, _pos1.position.z),
                new Vector3(_pos2.position.x + getOffsetByRound(), _pos2.position.y, _pos2.position.z), _state == CameraState.Forward ? _lerp: 1 - _lerp);

            this._activeSkillCameraObj.transform.position = this.transform.position;

            this.transform.rotation = Quaternion.Lerp(_pos1.rotation, _pos2.rotation, _state == CameraState.Forward ? _lerp : 1 - _lerp);

            this._activeSkillCameraObj.transform.rotation = this.transform.rotation;
            
        }

        if (_startDissolve)
        {
            float dissolveDelta = Time.deltaTime * _dissolveSpeed;
            if (_dissolve.Dissolve + dissolveDelta >= 1)
            {
                _dissolve.Dissolve = 1;
                _startDissolve = false;
            }
            else
                _dissolve.Dissolve += dissolveDelta;
        }
	}

    public void ChangeCam(bool forward)
    {
        _state = (forward ? CameraState.Forward : CameraState.Inverse);
        _lerp = 0;
    }

    public void ChangeRound(int round)
    {
        this._round = round;
        this.transform.position = new Vector3(_pos1.position.x + getOffsetByRound(), _pos1.position.y, _pos1.position.z);
        this.transform.rotation = _pos1.rotation;
    }

    private float getOffsetByRound()
    {
        return (this._round - 1) * PathFinder.GRID_SIZE * PathFinder.V_GRID * 3;
    }
}
