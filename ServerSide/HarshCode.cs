using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;


namespace ServerSide
{
    public class HarshCode
    {
        static string abc1 = "C:\\publicKey.xml";
        static string abc2 = "C:\\privatekey.xml";
        static string publicKeyFiles = File.ReadAllText(abc1);
        static string privateKeyFiles = File.ReadAllText(abc2);



        public static string RSAEncrypt(string content)
        {
            // string publickey = @"<RSAKeyValue><Modulus>0wE26IHp4U9OLtPhJ+fT8ej6aWORFP8pd++MjUuhkQQm/zhcImbxQbjxtSAftz+kkDwGDFJpSldQPyigOGcUx7PofTc6VhiFik9E9SsxV9n0iEEtqUndDfmBJfPAWt+4UDMwKakgZqFoapDuwjKlTErFvKCyKCs+qN9OZvZwKWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            string publickey = publicKeyFiles;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

            return Convert.ToBase64String(cipherbytes);

        }

        public static string RSADecrypt(string content)
        {
            // string privatekey = @"<RSAKeyValue><Modulus>0wE26IHp4U9OLtPhJ+fT8ej6aWORFP8pd++MjUuhkQQm/zhcImbxQbjxtSAftz+kkDwGDFJpSldQPyigOGcUx7PofTc6VhiFik9E9SsxV9n0iEEtqUndDfmBJfPAWt+4UDMwKakgZqFoapDuwjKlTErFvKCyKCs+qN9OZvZwKWk=</Modulus><Exponent>AQAB</Exponent><P>8Ei6NIsZtgV3DQjuGHfGLS6o1O+IUXxzjqLxdMm77yhEPUxR9YPIxODJ2VVTddXSAHxViJJt30yJ7JhVz6cpQw==</P><Q>4M49NrmalgVQFMsea2RMB1qN8fAPfIw5G9q9hzsLcWSCmkeRRIQlvPYflVEKAYKiDVVzENETbnnduFXWBABx4w==</Q><DP>t+JQbemN0Zi5FQaif6MZzHYKynpNTl75aE0Wj5Pa+RlNr8N6bXNe8Bw/HM2Jw4HQ5oJASvYUk3DVlHS4JuP8VQ==</DP><DQ>lT62iv9brp9mU/epgVh71SH8PJPIZEJfo6tryjyb0zMMNcqvmZI1z6aCv0mm3+vPFBUXqCF1yhFj7n4l8FAvSw==</DQ><InverseQ>flrvgxHvf4l+fdymEVDgKjsfGqshOpppoNgZj9kpeWBto3o8z++Ki6eSLQT3nVnpx2QCZeTWkxTED4nhSLKscw==</InverseQ><D>cQTCg1Eqk7sltmFYxUYgOP/AOPjSufteG9acYwYymPkvZh6rAuY+rSRBmvGE62NUYskzuB/gM6iG2/2HrA5SixfNgCvZ+nsK+kX5pzQRsYdD71ViQW0hOanXwj45I2zHRgBiuTtCUP0fs5pISmQkaeJkDL5pO2l+wvlgl+wunj0=</D></RSAKeyValue>";
            string privatekey = privateKeyFiles;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(privatekey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);

            return Encoding.UTF8.GetString(cipherbytes);
        }




    }
}
