using UnityEngine;

public class MessageSender : MonoBehaviour
{
    public GameObject mono;
    private Message.IMessageReceiver onInteractionMessageReceiver;
    Interaction.InteractMessage msg;

    private void Start()
    {
        OnValidate();
    }

    // IMessageReceiver를 상속 받은 Script가 있는 Object를 받도록 한다.!!
    void OnValidate()
    {
        onInteractionMessageReceiver = mono.GetComponent<Message.IMessageReceiver>();
        if (onInteractionMessageReceiver == null) mono = null;
    }
    #region HP Method
    //HP 증가
    public void Apply_AddHealth(GameObject item)
    {
        if (item.transform.parent.name == "Health_Red")
        {
            Add_RedHealth();
        }
        else if (item.transform.parent.name == "Health_Yellow")
        {
            Add_YellowHealth();
        }
        else if (item.transform.parent.name == "Health_Green")
        {
            Add_GreenHealth();
        }
    }
    private void Add_RedHealth()
    {
        msg = new Interaction.InteractMessage
        {
            amount = 30, //HP 0.3증가
            upgrade = false
        };
        onInteractionMessageReceiver.OnReceiverMessage(Message.MessageType.HEALTH, msg);
    }

    private void Add_YellowHealth()
    {
        msg = new Interaction.InteractMessage
        {
            amount = 60, //HP 0.6 증가
            upgrade = false
        };
        onInteractionMessageReceiver.OnReceiverMessage(Message.MessageType.HEALTH, msg);
    }

    private void Add_GreenHealth()
    {
        msg = new Interaction.InteractMessage
        {
            amount = 100, //HP 1증가
            upgrade = false
        };
        onInteractionMessageReceiver.OnReceiverMessage(Message.MessageType.HEALTH, msg);
    }
    #endregion

    //총알 증가
    public void Apply_AddBullet()
    {
        msg = new Interaction.InteractMessage
        {
            amount = 0,
            upgrade = true
        };
        onInteractionMessageReceiver.OnReceiverMessage(Message.MessageType.BULLET, msg);
    }
    //스피드 증가
    public void Apply_AddTurbin()
    {
        msg = new Interaction.InteractMessage
        {
            amount = 5, //runSpeed 5 증가 , fireWaitTime  0.1증가
            upgrade = true
        };
        onInteractionMessageReceiver.OnReceiverMessage(Message.MessageType.TURBIN, msg);
    }
    //총구 증가
    public void Apply_AddMuzzle()
    {
        msg = new Interaction.InteractMessage
        {
            amount = 0,
            upgrade = true
        };
        onInteractionMessageReceiver.OnReceiverMessage(Message.MessageType.MUZZLE, msg);
    }
    //데미지 적용
    public void ApplyDamage()
    {
        msg = new Interaction.InteractMessage
        {
            amount = 10, // 대미지 0.1f
            upgrade = false
        };
        onInteractionMessageReceiver.OnReceiverMessage(Message.MessageType.DAMAGE, msg);
    }
    //자폭
    public void Self_Destruction()
    {
        //onInteractionMessageReceiver.OnReceiverMessage(Message.MessageType., msg);
    }
    //초기화
    public void Reset_Interaction()
    {

    }
}
