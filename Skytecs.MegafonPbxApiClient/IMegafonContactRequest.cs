namespace Skytecs.MegafonPbxApiClient
{
    /// <summary>
    /// Команда для получения информации о названии клиента
    /// и ответственном за него сотруднике по номеру его телефона.
    /// Команда вызывается при поступлении нового входящего звонка.
    /// </summary>
    public interface IMegafonContactRequest
    {
        /// <summary>
        /// phone номер телефона клиента, с которого произошел звонок
        /// </summary>
        string Phone { get; }

        /// <summary>
        /// callid уникальный id звонка, 
        /// совпадает для всех связанных звонков
        /// </summary>
        string CallId { get; }

    }
}