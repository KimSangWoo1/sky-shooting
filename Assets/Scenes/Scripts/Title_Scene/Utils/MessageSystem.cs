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
        void OnReceiver_InteractMessage(MessageType type, object msg);
        void OnReceiver_DamageMessage(MessageType type, object msg);
    }
}