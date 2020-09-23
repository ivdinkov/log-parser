using LogParser.models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogParser.Service
{
    public class LogRepository : ILogRepository
    {
        private List<Log> _logs;
        public LogRepository()
        {
            _logs = new List<Log>();
        }

        public async Task<IEnumerable<Log>> GetAllLogsAsync()
        {
            return await Task.FromResult(_logs.OrderBy(o => o.TimeStamp));
        }

        public async Task SaveAllLogsAsync(byte[] formFile)
        {
            await Task.Run(() => SaveFileToObj(formFile));
        }

        public async Task ClearLogsAsync()
        {
            await Task.Run(() => ClearLogs());
        }

        public void SaveFileToObj(byte[] formFile)
        {
            Log log = new Log();
            string pattern = @"([0-9]{3} [a-zA-Z]+)";

            using (var reader = new StreamReader(new MemoryStream(formFile)))
            {
                while (reader.Peek() >= 0)
                {
                    var line = reader.ReadLine();

                    if (line.Length == 0)
                        continue;

                    if (line.StartsWith("TraceToken"))
                    {
                        log.TokenGUID = line.Split(new[] { ':' }, 2)[1].Trim();
                    }
                    else if (line.StartsWith("Timestamp"))
                    {
                        var timeString = line.Split(new[] { ':' }, 2);
                        DateTime dt = DateTime.Parse(timeString[1].Trim());
                        log.TimeStamp = dt.Day + " " + dt.DayOfWeek + " " + " " + dt.Year + " - " + dt.Hour + ":" + dt.Minute.ToString("D2") + ":" + dt.Second.ToString("D2") + ":" + dt.Millisecond.ToString("D2");
                    }
                    else if (line.StartsWith("GET http") || line.StartsWith("POST http") || line.StartsWith("PUT http") || line.StartsWith("DELETE http"))
                    {
                        log.Action = line.Split(' ').First();
                        var url = line.Split(new[] { ' ' }, 2)[1].Split(new[] { '/' }, 2)[1].Split('/');
                        var endpoint = url.Skip(2).ToArray();
                        log.Endpoint = string.Join('/', endpoint);
                        log.Host = url[1].Split(new[] { ':' }, 2)[0].ToString();
                    }
                    else if (line.StartsWith("Authorization"))
                    {
                        log.Authorization = line.Split(new[] { ':' }, 2)[1].ToString();
                    }
                    else if (Regex.Match(line, pattern, RegexOptions.IgnoreCase).Success)
                    {
                        log.ResponseCode = line.ToString();
                    }
                    else if (log.RequestBody == null && (log.Action.Equals("POST") || log.Action.Equals("PUT"))
                        && ((line.StartsWith("{") && line.EndsWith("}")) || (line.StartsWith("[") && line.EndsWith("]"))))
                    {
                        try
                        {
                            log.RequestBody = JObject.Parse(line);
                        }
                        catch (Exception)
                        {
                            log.RequestBody = new JArray() { "Unable to parse request body!" };
                        }
                    }
                    else if (log.ResponseCode != null
                        && ((line.StartsWith("{") && line.EndsWith("}")) || (line.StartsWith("[") && line.EndsWith("]"))))
                    {
                        try
                        {
                            log.ResponseBody = JObject.Parse(line);
                        }
                        catch (Exception)
                        {
                            log.ResponseBody = new JArray() { "Unable to parse response body!" };
                        }
                    }
                }
            }
            log.ID = _logs.Count() + 1;
            log.FullBody = formFile;
            _logs.Add(log);
        }

        public void ClearLogs()
        {
            _logs.Clear();
        }

        public async Task<string> GetDataAsync(string param)
        {
            string errMessage = "Unable to retreive data!";
            var data = param.Split('_');
            bool success = int.TryParse(data[1], out int reqId);
            var type = data[0];
            var propName = char.ToUpper(type[0]) + type.Substring(1) + "Body";


            try
            {
                if (success && propName.Equals("RequestBody"))
                {
                    var a = await Task.FromResult(_logs.Where(a => a.ID == reqId).Select(s => s.RequestBody).FirstOrDefault().ToString());
                    return a;

                }
                else if (success && propName.Equals("ResponseBody"))
                {
                    var t = await Task.FromResult(_logs.Where(a => a.ID == reqId).Select(s => s.ResponseBody).FirstOrDefault().ToString());
                    return t;

                }
                else if (success && propName.Equals("FullBody"))
                {
                    var byteContent = await Task.FromResult(_logs.Where(a => a.ID == reqId).Select(s => s.FullBody).FirstOrDefault());

                    return System.Text.Encoding.UTF8.GetString(byteContent);
                }
                else
                    return errMessage;
            }
            catch (Exception)
            {
                return errMessage;
            }
        }
    }
}
