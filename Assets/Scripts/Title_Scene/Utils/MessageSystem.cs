namespace Message
{
    //메시지 Type
    public enum MessageType
    {
        HEALTH,
        DAMAGE,
        BULLET,
        TURBIN,
        MUZZLE,
        DOLLAR,
        CLASH
    }
    // Interface
    public interface IMessageReceiver
    {
        void OnReceiverMessage(MessageType type, object msg); 
    }
}