﻿//
// Copyright 2015 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.IO;

using Xunit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Pivotal.Extensions.Configuration.ConfigServer.Test
{
    public class TestHelpers
    {
        public static string CreateTempFile(string contents)
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, contents);
            return tempFile;
            
        }
        public static Stream StringToStream(string str)
        {
            var memStream = new MemoryStream();
            var textWriter = new StreamWriter(memStream);
            textWriter.Write(str);
            textWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }

        public static string StreamToString(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        public static void VerifyDefaults(ConfigServerClientSettings settings)
        {
            Assert.Equal(ConfigServerClientSettings.DEFAULT_PROVIDER_ENABLED, settings.Enabled);
            Assert.Equal(ConfigServerClientSettings.DEFAULT_FAILFAST, settings.FailFast);
            Assert.Equal(ConfigServerClientSettings.DEFAULT_URI, settings.Uri);
            Assert.Equal(ConfigServerClientSettings.DEFAULT_ENVIRONMENT, settings.Environment);
            Assert.Equal(ConfigServerClientSettings.DEFAULT_ACCESS_TOKEN_URI, settings.AccessTokenUri);
            Assert.Equal(ConfigServerClientSettings.DEFAULT_CLIENT_ID, settings.ClientId);
            Assert.Equal(ConfigServerClientSettings.DEFAULT_CLIENT_SECRET, settings.ClientSecret);
            Assert.Equal(ConfigServerClientSettings.DEFAULT_CERTIFICATE_VALIDATION, settings.ValidateCertificates);
            Assert.Null(settings.Name);
            Assert.Null(settings.Label);
            Assert.Null(settings.Username);
            Assert.Null(settings.Password);
        }
    }

    public class TestConfigServerStartup
    {

        private String _response;
        private int _returnStatus;

        private HttpRequest _request;

        public HttpRequest LastRequest
        {
            get
            {
                return _request;
            }
        }
        public TestConfigServerStartup(string response) :
            this(response, 200)
        {

        }

        public TestConfigServerStartup(string response, int returnStatus)
        {
            _response = response;
            _returnStatus = returnStatus;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Run(async context =>
            { 
                _request = context.Request;
                context.Response.StatusCode = _returnStatus;
                await context.Response.WriteAsync(_response);
            });
        }

    }
}
