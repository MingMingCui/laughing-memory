//using UnityEngine;

//public class BuildHero : MonoBehaviour
//{
//    private Transform mTransform;

//    private GameObject go;

//    private void Awake()
//    {
//        mTransform = transform;
//        ZEventSystem.Register(EventConst.BuildHero,this, "BuildHeroEvent");
//        ZEventSystem.Register(EventConst.DragHero, this, "DragHeroEvent");
//    }

//    public void BuildHeroEvent(int heroId)
//    {

//        if (mTransform.childCount > 0)
//        {
//            for (int i = 0,length = mTransform.childCount; i < length; ++i)
//            {
//                Destroy(mTransform.GetChild(0).gameObject);
//            }
//        }
//        HeroData data = HeroMgr.GetSignleton().GetHeroData(heroId);
//        go = ResourceMgr.Instance.LoadResource(data.JsonData.resid) as GameObject;
//        if (go == null)
//            return;
//        go = Instantiate(go, mTransform, false);
//        go.transform.localPosition = Vector3.zero;
//        go.SetLayer("Hero");
//    }
//    private void OnDestroy()
//    {
//        ZEventSystem.DeRegister(EventConst.BuildHero);
//        ZEventSystem.DeRegister(EventConst.DragHero);
//    }

//    float StartX;
//    float previousX;
//    float offset;
//    float finalOffset;

//    bool isSlide;
//    float angle;

//    public float scale = 0.1f;

//    public void DragHeroEvent()
//    {
//        if (Input.GetMouseButton(0))
//        {
//            if (StartX == 0)
//                StartX = previousX = Input.mousePosition.x;
//            offset = previousX - Input.mousePosition.x;
//            previousX = Input.mousePosition.x;
//            go.transform.Rotate(Vector3.up, go.transform.localEulerAngles.y + offset * scale, Space.World);
//        }
//        if (Input.GetMouseButtonUp(0))
//        {
//            finalOffset = StartX - Input.mousePosition.x;
//            isSlide = true;
//            angle = finalOffset;
//        }
//        while (isSlide)
//        {
//            go.transform.Rotate(Vector3.up, angle * 2 * Time.deltaTime * scale, Space.World);
//            if (angle > 0)
//            {
//                angle -= 5;
//            }
//            else
//            {
//                angle = 0;
//                StartX = 0;
//                isSlide = false;
//            }
//        }
//    }
//}
