using UnityEngine;

public class UICtrlBase<T> : IUICtrl where T : UIViewBase
{
    public T mView = null;
    //public bool isOpen { get; private set; }
    public override void OnInit()
    {
    }

    public override void OnOpen()
    {
        //isOpen = true;
    }

    public override bool OnClose()
    {
        //isOpen = false;
        return true;
    }

    public override void OnDestroy()
    {
        if (mView != null)
        {
            GameObject.DestroyImmediate(mView.gameObject);
            mView = null;
        }
    }

    public override bool IsOpen()
    {
        return isOpen;
    }

    public override void LoadUI()
    {
    }

    public override UIViewBase GetView()
    {
        return mView;
    }

    public override void SetView(UIViewBase view)
    {
        mView = (T)view;
    }

}
