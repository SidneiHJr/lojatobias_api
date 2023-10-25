namespace LojaTobias.Core.Interfaces
{
    public interface INotifiable
    {
        bool HasNotification { get; }
        IEnumerable<object> GetNotificationsAsObject { get; }
        IEnumerable<string> GetNotifications { get; }
        void AddNotifications(IEnumerable<string> notifications);
        void AddNotification(string property, string message);
        void AddNotification(string message);
    }
}
