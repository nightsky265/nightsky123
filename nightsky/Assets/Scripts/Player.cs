using UnityEngine;
using UnityEngine.UI;   // 引用 介面 API

public class Player : MonoBehaviour
{
    #region 欄位區域
    //定義欄位 Field
    //欄位類型 欄位名稱 (指定 值)結尾
    //預設 private 私人(在屬性面板上隱藏) 預設
    //public 公開 (在屬性面板上公開)
    [Header("速度"), Range(0f, 100f)]
    public float speed = 3.5f;
    [Header("跳躍"), Range(100, 2000)]
    public int jump = 300;
    [Header("是否在地板上"), Tooltip("用來判定角色是否在地板上")]
    public bool isGround = false;
    [Header("角色名稱")]
    public string _name = "robot";
    [Header("元件")]
    public Rigidbody2D r2d;
    public Animator ani;
    [Header("音效區域")]
    public AudioSource aud;
    public AudioClip soundDiamond;
    [Header("鑽石區域")]
    public int diamondCurrent;
    public int diamondTotal;
    public Text textDiamond;
    #endregion
    //定義方法
    //語法:
    //修飾詞 傳回類型 方法名稱() {}
    //void 無傳回

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");   // 輸入.曲軸向("水平") 左右與 AD
        r2d.AddForce(new Vector2(speed * h, 0));
        ani.SetBool("跑步開關", h != 0);            // 動畫元件.設定布林值("參數",值)

        // 如果 按下A 或者 左方向鍵 角度 = (0, 180, 0)
        // 如果 按下D 或者 右方向鍵 角度 = (0, 0, 0)
        // transform.eulerAngles 角色變形元件.世界角度
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) transform.eulerAngles = new Vector3(0, 180, 0);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) transform.eulerAngles = new Vector3(0, 0, 0);
    }
    private void Jump()
    {
        // 如果 按下空白鍵 並且 在地板上 等於 勾選
        if(Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            isGround = false;                       // 在地板上 = 取消
            r2d.AddForce(new Vector2(0, jump));     // 剛體.推力(往上)
            ani.SetTrigger("跳躍開關");             // 動畫元件.設定觸發器("參數")
        }
    }
    private void Dead()
    {

    }
    // 開始事件：播放時執行一次
    private void Start()
    {
        // 鑽石總數 = 尋找所有指定標籤物件("指定標籤").數量
        diamondTotal = GameObject.FindGameObjectsWithTag("鑽石").Length;
        // 更新介面
        textDiamond.text = "鑽石：0 / "+ diamondTotal;
    }

    // 事件: 在特定時間點以指定次數執行
    // 更新事件: 一秒直行約60次(60FPS)
    private void Update()
    {
        Move();
        Jump();
    }

    // 碰撞事件:2D物件碰撞時執行一次
    // collision 參數:碰到物件的資訊
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果 碰撞.物件.名稱 等於 "地板"
        if (collision.gameObject.name == "地板")
        {
            isGround = true;
        }
    }
    //觸發事件：針對碰撞器有勾選 IsTrigger 的物件
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "鑽石")
        {
            aud.PlayOneShot(soundDiamond, 1.5f);    //音源.播放一次音效(音效，音量)
            Destroy(collision.gameObject);          //刪除(碰撞的物件)
            diamondCurrent++;                       //遞增
            textDiamond.text = "鑽石：" + diamondCurrent + " / " + diamondTotal;
        }
    }
}
