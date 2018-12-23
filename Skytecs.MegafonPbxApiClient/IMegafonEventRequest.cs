namespace Skytecs.MegafonPbxApiClient
{
    /// <summary>
    /// Облачная АТС отправляет в вашу CRM уведомления о событиях входящих звонков 
    /// пользователям: появлении, принятии или завершении звонка.Команда может быть использована для
    /// отображения всплывающей карточки клиента в интерфейсе CRM.
    /// Команда доступна сразу после включения API.
    /// </summary>
    public interface IMegafonEventRequest
    {
        /// <summary>
        /// type - это тип события, связанного со звонком
        /// </summary>
        MegafonEventType Type { get; }

        /// <summary>
        /// user идентификатор пользователя облачной АТС
        /// (необходим для сопоставления на стороне CRM)
        /// </summary>
        string User { get; }

        /// <summary>
        /// ext внутренний номер пользователя облачной АТС, если есть string нет
        /// </summary>
        string Ext { get; }

        /// <summary>
        /// groupRealName название отдела, если входящий звонок прошел через отдел
        /// </summary>
        string GroupRealName { get; }

        /// <summary>
        /// telnum прямой телефонный номер пользователя облачной АТС, если есть
        /// </summary>
        string TelNum { get; }

        /// <summary>
        /// phone номер телефона клиента, с которого или 
        /// на который произошел звонок
        /// </summary>
        string Phone { get; }

        /// <summary>
        /// diversion ваш номер телефона, через который 
        /// пришел входящий вызов
        /// </summary>
        string Diversion { get; }

        /// <summary>
        /// callid уникальный id звонка, 
        /// совпадает для всех связанных звонков
        /// </summary>
        string CallId { get; }

    }
}