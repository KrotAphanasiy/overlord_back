using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flash.Central.Foundation.Options
{
    public class S3ClientOptions
    {
        public string BucketName { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string ServiceURL { get; set; }
        public string RegionEndpoint { get; set; }
        public bool ForcePathStyle { get; set; }
    }
}
