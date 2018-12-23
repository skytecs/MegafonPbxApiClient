namespace Skytecs.MegafonPbxApiClient
{
    /// <summary>
    /// Определяет ответственного сотрудника
    /// </summary>
    public class MegafonContact
    {
        /// <summary>
        /// Имя контакта
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// В поле responsible может быть передан логин, внутренний номер или прямой телефонный
        /// номер пользователя Облачной АТС.
        /// </summary>
        public string Responsible { get; set; }
    }
}