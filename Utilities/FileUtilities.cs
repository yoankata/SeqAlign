﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace SeqAlign.Utilities
{
    public static class FileUtilities
    {
        public static async Task<string> ReadFileContent(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var fileContent = await reader.ReadToEndAsync();

                //var content = new MultipartFormDataContent
                //{ { new ByteArrayContent(Encoding.ASCII.GetBytes(fileLines)), "\"upload\"", file.Name }};

                //await client.PostAsync("upload", content);
                return fileContent;
            }
        }


        public static async Task WriteFileContents(ICollection<string> sequences, string fileName)
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/TestFolder").Empty();
            using (var writer = new StreamWriter(Directory.GetCurrentDirectory() + $"/TestFolder/{fileName}"))
            {
                foreach (var line in sequences)
                {
                    await writer.WriteLineAsync(line);
}
            }
        }

    public static ValueTask<object> SaveAs(this IJSRuntime js, string filename, byte[] data)
        => js.InvokeAsync<object>(
            "saveAsFile",
            filename,
            Convert.ToBase64String(data));
    }
}
