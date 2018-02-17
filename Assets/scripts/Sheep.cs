using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {
    private Vector2 targetPoint;
    private float degree;
    private Rect originResolution;
    private Rect resizeResolution;
    private Rect rotationSize; // 회전 반경
    public GameObject CathCircle;
    public Transform Group;
    public float Speed;

    public GameObject Manager;

    private bool triger; //해당 위치에 눌렀는지?
    public bool Triger { get { return triger; } }

    // Use this for initialization
    private void Start () {
        Transform view = GameObject.Find("Canvas").transform;
        Rect originSize = view.GetComponent<RectTransform>().rect;
        Rect catchTargetSize = CathCircle.transform.GetComponent<RectTransform>().rect;

        targetPoint = new Vector2(0, 0);

        resizeResolution = new Rect(0, 0, originSize.width * view.localScale.x, originSize.width * view.localScale.x);

        Rect spriteSize = this.GetComponent<RectTransform>().rect;
        Rect viewSize = view.GetComponent<RectTransform>().rect;

        float rotationWidth = -(resizeResolution.width + ((viewSize.width * view.localScale.x) / 2));
        float rotationHeight = -(catchTargetSize.y - (spriteSize.height/2));

        rotationSize = new Rect(Vector2.zero, new Vector2(rotationWidth, rotationHeight));

        degree = 0;
        triger = false;
    }

    // Update is called once per frame
    private void Update () {
         Move();
    }

    private void Move()
    {   
        float rx = 0.0f, ry = 0.0f;
        float rad = 0.0f;

        degree += Speed;

        rad = degree * (Mathf.PI / 180);
        rx = rotationSize.width * Mathf.Cos(rad);
        ry = rotationSize.height * Mathf.Sin(rad);

        this.transform.localPosition = new Vector2(rx, ry);

        if (degree >= 180.0f) {
            newLife();
        }
    }

    public void newLife() {
        Transform newSheep = Instantiate(this.gameObject, Group).transform;
        newSheep.name = "Sheep";
        Manager.GetComponent<TimingSheep_Manager>().sheepCountUp();
        Destroy(this.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        Rect trigerRect = collision.transform.GetComponent<RectTransform>().rect;
        Rect myRect = this.transform.GetComponent<RectTransform>().rect;

        float basicPointLeft = (collision.transform.position.x - (trigerRect.width / 2));
        float basicPointRight = (collision.transform.position.x + (trigerRect.width / 2));

        float myPointLeft = (this.transform.position.x - (trigerRect.width / 2));
        float myPointRight = (this.transform.position.x + (trigerRect.width / 2));

        float inWidth = trigerRect.width - (Mathf.Abs(basicPointLeft - myPointLeft) + Mathf.Abs(myPointRight - basicPointRight));
        // 충돌범위 밖으로 벗어난 정도를 구하고 본래 width에 빼주어 정확도를 검사합니다.

        float accuracy = (inWidth / trigerRect.width) * 100;

        triger = accuracy >= 77 ? true : false;

        Debug.Log(accuracy);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        triger = false;
    }
}
