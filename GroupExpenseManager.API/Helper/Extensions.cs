using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GroupExpenseManager.API.Helper
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error",message);
            response.Headers.Add("Access-Control-Expose-Headers","Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin","*");
        }

        public static void AddPagination(this HttpResponse response, int currentPage,int itemsPerPage,
         int totalItems, int totalPages)
         {
             PaginationHeader paginationHeader =  new PaginationHeader(currentPage,itemsPerPage,totalItems,totalPages);
             var camelCaseFormatter = new JsonSerializerSettings();
             camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
             response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader,camelCaseFormatter));
             response.Headers.Add("Access-Control-Expose-Headers","Pagination");
         }

        public static int CalculateAge(this DateTime userDateTime)
        {
            var age = DateTime.Now.Year - userDateTime.Year;

            if(userDateTime.AddYears(age) > DateTime.Now)
            {
                return age--;
            }

            return age;
        }
    }
}