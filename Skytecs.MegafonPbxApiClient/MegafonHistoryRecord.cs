using System;

namespace Skytecs.MegafonPbxApiClient
{
    public class MegafonHistoryRecord
    {
        /// <summary>
        /// уникальный идентификатор звонка
        /// </summary>
        public string UID { get; set; }
        /// <summary>
        /// тип вызова: in / out / missed
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// номер клиента
        /// </summary>
        public string Client { get; set; }
        /// <summary>
        /// логин сотрудника, который разговаривал с клиентом или имя группы или код: 
        /// ivr / fax, если звонок не дошел до сотрудника
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// номер телефона, через который пришел входящий звонок или АОН для исходящего вызова
        /// </summary>
        public string Via { get; set; }
        /// <summary>
        /// время начала звонка в UTC
        /// </summary>
        public DateTime? Start { get; set; }
        /// <summary>
        /// время ожидания на линии (секунд)
        /// </summary>
        public int? Wait { get; set; }
        /// <summary>
        /// длительность разговора (секунд)
        /// </summary>
        public int? Duration { get; set; }
        /// <summary>
        /// ссылка на запись разговора
        /// </summary>
        public string Record { get; set; }
    }
}