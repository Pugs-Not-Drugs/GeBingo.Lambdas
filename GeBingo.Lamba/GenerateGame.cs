using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;

namespace GeBingo.Lamba
{

    public class GenerateGame
    {

        public async Task<Game> FunctionHandler(string username, ILambdaContext context)
        {
            var game = new Game
            {
                Id = Guid.NewGuid(),
                Username = username
            };

            using (var client = new AmazonS3Client(Amazon.RegionEndpoint.EUWest1))
            {
                var jsonObject = JsonConvert.SerializeObject(game);
                var byteArray = Encoding.UTF8.GetBytes(jsonObject);
                var stream = new MemoryStream(byteArray);

                var request = new PutObjectRequest
                {
                    BucketName = "gebingo.co.uk",
                    Key = $"Games/{game.Id}.json",
                    InputStream = stream
                };

                await client.PutObjectAsync(request);

                return game;
            }
        }
    }
}