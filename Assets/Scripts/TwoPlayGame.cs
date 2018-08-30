using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TwoPlayGame : MonoBehaviour
{

    private const int CardSize = 26;

    public ArrayList PlayerCard;
    public ArrayList EnemyCard;
    public ArrayList FieldCardPos;
    private GameObject Game;
    private ArrayList PlayerCardList;
    private ArrayList EnemyCardList;
    private bool[] PlayerFieldState;
    private bool[] EnemyFieldState;

    // Use this for initialization
    void Start()
    {
        Game = GameObject.Find("Game");

        PlayerCard = new ArrayList();
        EnemyCard = new ArrayList();
        FieldCardPos = new ArrayList();
        PlayerCardList = new ArrayList();
        EnemyCardList = new ArrayList();
        PlayerFieldState = new bool[5];
        EnemyFieldState = new bool[5];


        Init(PlayerCard);
        //Disp(PlayerCard);
        Shuffle(PlayerCard);
        //Disp(PlayerCard);
        Init(EnemyCard);
        Shuffle(EnemyCard);

        CreateCardObject();
        InitCardPos();
        InitField();

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCard.Count == 0) { }
        if (EnemyCard.Count == 0) { }


        Distribute(PlayerCard, PlayerFieldState, PlayerCardList, (ArrayList)FieldCardPos[0]);
        Distribute(EnemyCard, EnemyFieldState, EnemyCardList, (ArrayList)FieldCardPos[1]);

        //PlayerCard = PutOut(PlayerCard, PlayerFieldState, PlayerCardList, (ArrayList)FieldCardPos[0]);
        //EnemyCard = PutOut(EnemyCard, EnemyFieldState, EnemyCardList, (ArrayList)FieldCardPos[1]);
    }



    private void Distribute(ArrayList HandCard, bool[] FieldState, ArrayList Card, ArrayList ArrangePos)
    {
        GameObject obj;

        for (int i = 0; i < 5; ++i)
        {
            if (FieldState[i] == false)
            {
                FieldState[i] = true;
                obj = Instantiate((GameObject)Card[(int)HandCard[0]], (Vector3)ArrangePos[5], new Quaternion(-1, 0, 0, 1.0f));
                HandCard.RemoveAt(0);
                obj.transform.parent = transform;
                CreateAnimationClip(obj, (Vector3)ArrangePos[5], (Vector3)ArrangePos[i], new Vector2(0, 1.0f));
            }
        }
    }

    private void InitField()
    {
        GameObject obj;

        obj = Instantiate((GameObject)PlayerCardList[0], (Vector3)((ArrayList)FieldCardPos[0])[5], Quaternion.identity);
        obj.GetComponent<Transform>().transform.Rotate(90, 0, 0);
        obj.transform.parent = transform;
        obj = Instantiate((GameObject)PlayerCardList[0], (Vector3)((ArrayList)FieldCardPos[1])[5], Quaternion.identity);
        obj.GetComponent<Transform>().transform.Rotate(90, 0, 0);
        obj.transform.parent = transform;
    }

    private void InitCardPos()
    {
        ArrayList PlayerCardPos = new ArrayList();
        ArrayList EnemyCardPos = new ArrayList();
        ArrayList PlaceCardPos = new ArrayList();


        PlayerCardPos.Add(new Vector3(0, 0.75f, -0.3f));
        PlayerCardPos.Add(new Vector3(-0.1f, 0.75f, -0.3f));
        PlayerCardPos.Add(new Vector3(-0.2f, 0.75f, -0.3f));
        PlayerCardPos.Add(new Vector3(0.1f, 0.75f, -0.3f));
        PlayerCardPos.Add(new Vector3(0.2f, 0.75f, -0.3f));
        PlayerCardPos.Add(new Vector3(-0.35f, 0.75f, -0.3f));


        EnemyCardPos.Add(new Vector3(0, 0.75f, 0.1f));
        EnemyCardPos.Add(new Vector3(-0.1f, 0.75f, 0.1f));
        EnemyCardPos.Add(new Vector3(-0.2f, 0.75f, 0.1f));
        EnemyCardPos.Add(new Vector3(0.1f, 0.75f, 0.1f));
        EnemyCardPos.Add(new Vector3(0.2f, 0.75f, 0.1f));
        EnemyCardPos.Add(new Vector3(0.35f, 0.75f, 0.1f));

        PlaceCardPos.Add(new Vector3(-0.07f, 0.75f, -0.1f));
        PlaceCardPos.Add(new Vector3(0.07f, 0.75f, -0.1f));


        FieldCardPos.Add(PlayerCardPos);
        FieldCardPos.Add(EnemyCardPos);
        FieldCardPos.Add(PlaceCardPos);

    }


    private void CreateCardObject()
    {
        string str;
        GameObject obj;

        for (int i = 1; i <= CardSize; ++i)
        {
            str = "Cards/" + i.ToString();
            obj = (GameObject)Resources.Load(str);
            PlayerCardList.Add(obj);
        }

        for (int i = CardSize + 1; i <= CardSize * 2; ++i)
        {
            str = "Cards/" + i.ToString();
            obj = (GameObject)Resources.Load(str);
            EnemyCardList.Add(obj);
        }
    }

    private void CreateAnimationClip(GameObject obj, Vector3 startPos, Vector3 targetPos, Vector2 time)
    {
        Animation animation = obj.GetComponent<Animation>();

        // もしもアニメーションコンポーネントがなければ追加.
        if (!animation)
        {
            obj.AddComponent<Animation>();
        }

        // AnimationClipの生成.
        AnimationClip clip = new AnimationClip();

        // AnimationCurveの生成.
        AnimationCurve curvex = new AnimationCurve();
        AnimationCurve curvey = new AnimationCurve();
        AnimationCurve curvez = new AnimationCurve();

        // Keyframeの生成.
        Keyframe startxKeyframe = new Keyframe(time.x, startPos.x);
        Keyframe endxKeyframe = new Keyframe(time.y, targetPos.x);
        Keyframe startyKeyframe = new Keyframe(time.x, startPos.y);
        Keyframe endyKeyframe = new Keyframe(time.y, targetPos.y);
        Keyframe startzKeyframe = new Keyframe(time.x, startPos.z);
        Keyframe endzKeyframe = new Keyframe(time.y, targetPos.z);

        // Keyframeの追加.
        curvex.AddKey(startxKeyframe);
        curvex.AddKey(endxKeyframe);
        curvey.AddKey(startyKeyframe);
        curvey.AddKey(endyKeyframe);
        curvez.AddKey(startzKeyframe);
        curvez.AddKey(endzKeyframe);

        // AnimationCurveの追加.
        clip.legacy = true;
        clip.SetCurve("", typeof(Transform), "localPosition.x", curvex);
        clip.SetCurve("", typeof(Transform), "localPosition.y", curvey);
        clip.SetCurve("", typeof(Transform), "localPosition.z", curvez);
        // AnimationClipの追加.
        obj.GetComponent<Animation>().AddClip(clip, "Move");
        // AnimationClipの再生.
        obj.GetComponent<Animation>().Play("Move");
    }

    public void Disp(ArrayList ary)
    {
        for (int i = 0; i < CardSize; ++i)
            Debug.Log(ary[i]);
    }

    private void Shuffle(ArrayList ary)
    {
        //Fisher-Yatesアルゴリズムでシャッフルする
        System.Random rng = new System.Random();
        int n = (int)ary.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int tmp = (int)ary[k];
            ary[k] = (int)ary[n];
            ary[n] = tmp;
        }
    }

    private void Init(ArrayList ary)
    {

        for (int i = 0; i < CardSize; ++i)
        {
            ary.Add(i + 1);
        }
    }
}
