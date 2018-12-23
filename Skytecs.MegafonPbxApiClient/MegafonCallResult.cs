namespace Skytecs.MegafonPbxApiClient
{
    /// <summary>
    /// status статус входящего звонка
    /// </summary>
    public enum MegafonCallResult
    {
        /// <summary>
        /// Success - успешный входящий/исходящий звонок
        /// </summary>
        Success,
        /// <summary>
        /// missed – пропущенный входящий звоно
        /// </summary>
        Missed,
        /// <summary>
        /// Busy - мы получили ответ Занято
        /// </summary>
        Busy,
        /// <summary>
        /// NotAvailable - мы получили ответ Абонент недоступен
        /// </summary>
        NotAvailable,
        /// <summary>
        /// NotAllowed - мы получили ответ Звонки на это направление запрещен
        /// </summary>
        NotAllowed
    }
}