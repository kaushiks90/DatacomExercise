using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DatacomConsole
{
    interface IRestUtility
    {
        /// <summary>
        /// Post request generic method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestContent"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<T> PostRequestAsync<T>(object requestContent, string url);

        Task<HttpStatusCode> GetAsync<T>(string url);
    }
}
