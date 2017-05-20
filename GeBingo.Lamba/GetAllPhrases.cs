using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.S3;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GeBingo.Lamba
{
    public class GetAllPhrases
    {
        private static IAmazonS3 _client;
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Phrase>> FunctionHandler(ILambdaContext context)
        {
            _client = new AmazonS3Client(Amazon.RegionEndpoint.EUWest1);
            using (var response = await _client.GetObjectAsync("gebingo.co.uk", "bingo.json"))
            {
                
                var arr = new byte[response.ContentLength];
                await response.ResponseStream.ReadAsync(arr, 0, Convert.ToInt32(response.ContentLength));
                return JsonConvert.DeserializeObject<IEnumerable<Phrase>>(new System.Text.ASCIIEncoding().GetString(arr));
            }
        }
    }
}
