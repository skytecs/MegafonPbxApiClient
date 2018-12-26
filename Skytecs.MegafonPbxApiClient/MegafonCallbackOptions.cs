using System;
using System.Threading.Tasks;

namespace Skytecs.MegafonPbxApiClient
{
    /// <summary>
    /// Настройки обрабочиков команд
    /// </summary>
    public class MegafonCallbackOptions
    {
        /// <summary>
        /// Вызывается при получении команды history <see cref="IMegafonHistoryRequest"/>
        /// </summary>
        public Func<IMegafonHistoryRequest, Task> OnHistory { get; set; }
        /// <summary>
        /// Вызывается при получении команды event <see cref="IMegafonEventRequest"/>
        /// </summary>
        public Func<IMegafonEventRequest, Task> OnEvent { get; set; }
        /// <summary>
        /// Вызывается при получении команды contact <see cref="IMegafonContactRequest"/>
        /// </summary>
        public Func<IMegafonContactRequest, Task<MegafonContact>> OnContact { get; set; }

        internal async Task InvokeOnHistory(IMegafonHistoryRequest request)
        {
            if (OnHistory != null)
            {
                await OnHistory(request);
            }
        }

        internal async Task InvokeOnEvent(IMegafonEventRequest request)
        {
            if (OnEvent != null)
            {
                await OnEvent(request);
            }
        }

        internal async Task<MegafonContact> InvokeOnContact(IMegafonContactRequest request)
        {
            if (OnContact != null)
            {
                return await OnContact(request);
            }

            return null;
        }

    }
}