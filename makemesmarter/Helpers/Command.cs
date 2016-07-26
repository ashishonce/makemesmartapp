using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace makemesmarter.Helpers
{
    // Classes to store the key phrases through text analysis
    public class Command
    {
        public static Task<string> GetContactEntity(string query)
        {
            return Task.FromResult<string>("Contact:" + query);
        }
    }
}