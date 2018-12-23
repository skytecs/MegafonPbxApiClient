using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Skytecs.MegafonPbxApiClient
{
    /// <summary>
    /// Клиент API Облачной АТС Мегафон
    /// </summary>
    public interface IMegafonApiClient
    {
        /// <summary>
        /// Запрос от CRM к Облачной АТС для получения сотрудников.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<MegafonAccount>> Accounts();
        /// <summary>
        /// Запрос от CRM к Облачной АТС для получения отделов.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<MegafonGroup>> Groups();
        /// <summary>
        /// Команда необходимая для того, чтобы инициировать звонок от менеджера клиенту.
        /// В результате успешного выполнения команды, Облачная АТС сделает сначала звонок на телефон 
        /// менеджера, а потом соединит его с клиентом.
        /// </summary>
        /// <param name="phone">номер, на который последует звонок</param>
        /// <param name="user">пользователь Облачной АТС, от которого 
        /// последует звонок (может быть передан логин, внутренний номер 
        /// или прямой телефонный номер пользователя)</param>
        /// <returns></returns>
        string MakeCall(string phone, string user);
        /// <summary>
        /// Команда необходима для того, чтобы получить из Облачной АТС историю звонков за нужный период времени.
        /// </summary>
        /// <param name="start">начало периода для выгрузки данных</param>
        /// <param name="end">окончание периода для выгрузки данных</param>
        /// <param name="period">Если в запросе установлено значение period,
        /// выгружаются данные за указанный период, независимо от 
        /// значения start и end.</param>
        /// <param name="type">Тип звонка. Если не задан, используется all</param>
        /// <param name="limit">Результат должен содержать не более, чем limit записей</param>
        /// <returns></returns>
        Task<ICollection<MegafonHistoryRecord>> History(DateTime? start = null, DateTime? end = null, MegafonHistoryPeriod? period = null, MegafonHistoryRecordType? type = null, int? limit = null);
        /// <summary>
        /// Запрос от CRM к Облачной АТС для включение / выключения приема звонков сотрудником 
        /// во всех его отделах или конкретном отделе.
        /// Может использоваться для того, чтобы временно выключить прием звонков сотрудником во
        /// всех его отделах или кокретном отделе.
        /// </summary>
        /// <param name="user">идентификатор пользователя CRM или аккаунт ВАТС, если при
        /// авторизации был установлен self_map = true, для которого надо
        /// выключить/включить прием звонков</param>
        /// <param name="groupId">идентификатор отдела ВАТС, в котором надо
        /// выключить/включить прием звонков</param>
        /// <param name="enable">true - чтобы включить прием звонков (status on), false - чтобы выключить 
        /// прием звонков (status off)</param>
        void SubscribeOnCalls(string user, string groupId, bool enable);
        /// <summary>
        /// Запрос от CRM к Облачной АТС для проверки факта приема звонков сотрудником в конкретном отделе.
        /// </summary>
        /// <param name="user">идентификатор пользователя CRM или аккаунт ВАТС, если при 
        /// авторизации был установлен self_map = true, для которого надо
        /// выполнить проверку</param>
        /// <param name="groupId">идентификатор отдела ВАТС, для которого надо выполнить проверку</param>
        /// <returns>true - status on, false - status of</returns>
        bool SubscriptionStatus(string user, string groupId);
        /// <summary>
        /// Запрос от CRM к Облачной АТС позволяет включить или выключить прием звонков
        /// сотрудником Облачной АТС.
        /// </summary>
        /// <param name="user">идентификатор сотрудника Облачной АТС</param>
        /// <param name="state"></param>
        void SetDnd(string user, bool state);

        /// <summary>
        /// Запрос от CRM к Облачной АТС позволяет узнать включен или 
        /// выключен прием звонков сотрудником Облачной АТС.
        /// </summary>
        /// <param name="user">идентификатор сотрудника Облачной АТС</param>
        /// <returns></returns>
        Task<bool> GetDnd(string user);
    }
}
