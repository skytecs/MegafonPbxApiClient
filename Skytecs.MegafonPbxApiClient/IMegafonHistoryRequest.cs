using System;

namespace Skytecs.MegafonPbxApiClient
{
    /// <summary>
    /// После успешного звонка в CRM отправляется запрос с данными о звонке и ссылкой на запись разговора.
    /// Команда может быть использована для сохранения в данных ваших клиентов истории и записей входящих и исходящих звонков.
    /// Команда доступна сразу после включения API.
    /// </summary>
    public interface IMegafonHistoryRequest
    {
        /// <summary>
        /// type тип звонка in/out (входящий / исходящий)
        /// </summary>
        MegafonCallType Type { get; }

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
        /// время начала звонка в формате YYYYmmddTHHMMSSZ
        /// </summary>
        DateTime Start { get; }

        /// <summary>
        /// duration общая длительность звонка в секундах
        /// </summary>
        int Duration { get; }

        /// <summary>
        /// callid уникальный id звонка
        /// </summary>
        string CallId { get; }

        /// <summary>
        /// link ссылка на запись звонка, если она включена в Облачной АТС
        /// </summary>
        string Link { get; }

        /// <summary>
        /// status статус входящего звонка
        /// </summary>
        MegafonCallResult Status { get; }
    }
}