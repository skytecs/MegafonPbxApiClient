namespace Skytecs.MegafonPbxApiClient
{
    /// <summary>
    /// Период, за который надо выгрузить данные.
    /// </summary>
    public enum MegafonHistoryPeriod
    {
        /// <summary>
        /// звонки за сегодня
        /// </summary>
        Today,
        /// <summary>
        /// звонки за вчера
        /// </summary>
        Yesterday,
        /// <summary>
        /// звонки за текущую неделю
        /// </summary>
        ThisWeek,
        /// <summary>
        /// звонки за прошедшую неделю
        /// </summary>
        LastWeek,
        /// <summary>
        /// звонки за текущий месяц
        /// </summary>
        ThisMonth,
        /// <summary>
        /// звонки за прошедший месяц
        /// </summary>
        LastMonth,
    }
}