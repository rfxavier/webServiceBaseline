namespace ViewT.Crediario.Domain.Main.Interfaces
{
    public interface IPushService
    {
        void SendPush(string deviceToken, string title, string body);
    }
}
