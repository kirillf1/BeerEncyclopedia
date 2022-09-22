using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BeerEncyclopedia.Application.Helpers
{
    internal static class HttpHelper
    {
        /// <summary>
        /// Checks for successful status code if not throws an HttpRequestException
        /// </summary>
        /// <param name="httpStatusCode"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public static bool CheckStatusCode(HttpStatusCode httpStatusCode)
        {
            int statusCode = (int)httpStatusCode;
            if (statusCode > 299)
                throw new HttpRequestException($"Request failed with code {statusCode}");
            return true;
        }
    }
}
