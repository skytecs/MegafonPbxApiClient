using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Skytecs.MegafonPbxApiClient
{
    class ApiClient : IMegafonApiClient
    {
        private readonly MegafonApiOptions _options;
        private readonly ILogger<IMegafonApiClient> _logger;

        public ApiClient(MegafonApiOptions options, ILogger<IMegafonApiClient> logger = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? new VoidLogger<IMegafonApiClient>();
        }

        public async Task<ICollection<MegafonAccount>> Accounts()
        {
            var args = new[]
            {
                new KeyValuePair<string, string>("cmd", "accounts"),
                new KeyValuePair<string, string>("token", _options.ApiToken),
            };

            return await SendRequest(args, text =>
            {
                using (var json = new JsonTextReader(text))
                {
                    return new JsonSerializer().Deserialize<MegafonAccount[]>(json);
                }
            });
        }

        public async Task<bool> GetDnd(string user)
        {
            var args = new[]
            {
                new KeyValuePair<string, string>("cmd", "get_dnd"),
                new KeyValuePair<string, string>("token", _options.ApiToken),
                new KeyValuePair<string, string>("user", user),
            };

            return await SendRequest(args, text =>
            {
                using (var json = new JsonTextReader(text))
                {
                    var result = new JsonSerializer().Deserialize<JObject>(json);

                    return result.Value<bool>("state");
                }
            });
        }

        public async Task<ICollection<MegafonGroup>> Groups()
        {
            var args = new[]
            {
                new KeyValuePair<string, string>("cmd", "groups"),
                new KeyValuePair<string, string>("token", _options.ApiToken),
            };

            return await SendRequest(args, text =>
            {
                using (var json = new JsonTextReader(text))
                {
                    return new JsonSerializer().Deserialize<MegafonGroup[]>(json);
                }
            });
        }

        public async Task<ICollection<MegafonHistoryRecord>> History(DateTime? start = null, DateTime? end = null,
            MegafonHistoryPeriod? period = null, MegafonHistoryRecordType? type = null, int? limit = null)
        {
            var args = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("cmd", "history"),
                new KeyValuePair<string, string>("token", _options.ApiToken),
            };

            if(start != null)
            {
                args.Add(new KeyValuePair<string, string>("start", start.Value.ToUniversalTime().ToString("yyyyMMddThhmmssZ")));
            }
            if (end != null)
            {
                args.Add(new KeyValuePair<string, string>("end", end.Value.ToUniversalTime().ToString("yyyyMMddThhmmssZ")));
            }
            if (limit != null)
            {
                args.Add(new KeyValuePair<string, string>("limit", limit.Value.ToString()));
            }
            if(period != null)
            {
                var value = "";
                switch(period.Value)
                {
                    case MegafonHistoryPeriod.Today:
                        value = "today";
                        break;
                    case MegafonHistoryPeriod.Yesterday:
                        value = "yesterday";
                        break;
                    case MegafonHistoryPeriod.ThisWeek:
                        value = "this_week";
                        break;
                    case MegafonHistoryPeriod.LastWeek:
                        value = "last_week";
                        break;
                    case MegafonHistoryPeriod.ThisMonth:
                        value = "this_month";
                        break;
                    case MegafonHistoryPeriod.LastMonth:
                        value = "last_month";
                        break;
                }
                args.Add(new KeyValuePair<string, string>("period", value));
            }
            if (type != null)
            {
                var value = "";
                switch (type.Value)
                {
                    case MegafonHistoryRecordType.All:
                        value = "all";
                        break;
                    case MegafonHistoryRecordType.Incoming:
                        value = "in";
                        break;
                    case MegafonHistoryRecordType.Missed:
                        value = "missed";
                        break;
                    case MegafonHistoryRecordType.Outgoing:
                        value = "out";
                        break;
                }
                args.Add(new KeyValuePair<string, string>("type", value));

            }

            return await SendRequest(args, text =>
            {
                var result = new List<MegafonHistoryRecord>();

                while (!text.EndOfStream)
                {
                    var line = text.ReadLine();

                    var parts = line.Split(',');

                    result.Add(new MegafonHistoryRecord
                    {
                        UID = parts.Length > 0 ? parts[0] : null,
                        Type = parts.Length > 1 ? parts[1] : null,
                        Client = parts.Length > 2 ? parts[2] : null,
                        Account = parts.Length > 3 ? parts[3] : null,
                        Via = parts.Length > 4 ? parts[4] : null,
                        Start = parts.Length > 5 && DateTime.TryParse(parts[5], out var recordStart) ? recordStart : default(DateTime?),
                        Wait = parts.Length > 6 && int.TryParse(parts[6], out var recordWait) ? recordWait : default(int?),
                        Duration = parts.Length > 7 && int.TryParse(parts[7], out var recordDuration) ? recordDuration : default(int?),
                        Record = parts.Length > 8 ? parts[8] : null,

                    });
                }

                return result;
            });
        }

        public string MakeCall(string phone, string user)
        {
            throw new NotImplementedException();
        }

        public void SetDnd(string user, bool state)
        {
            throw new NotImplementedException();
        }

        public void SubscribeOnCalls(string user, string groupId, bool enable)
        {
            throw new NotImplementedException();
        }

        public bool SubscriptionStatus(string user, string groupId)
        {
            throw new NotImplementedException();
        }

        private async Task<TResult> SendRequest<TResult>(ICollection<KeyValuePair<string, string>> args, Func<StreamReader, TResult> processResults = null)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, _options.PbxEndpoint))
            {
                request.Headers.CacheControl = CacheControlHeaderValue.Parse("no-cache");
                request.Content = new FormUrlEncodedContent(args);

                using (var response = await client.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new InvalidOperationException(await response.Content.ReadAsStringAsync());
                    }

                    if (processResults != null)
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        using (var text = new StreamReader(stream))
                        {

                            return processResults(text);
                        }
                    }

                    return default(TResult);
                }
            }
        }

    }
}
