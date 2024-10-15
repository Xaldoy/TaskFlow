using Service.DTOs.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskFlow.Service.DTOs.Result
{
    public class ApiResponse<T>
    {
        T? Data { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        MessageResponse? ErrorResponse { get; set; }

    }
}
