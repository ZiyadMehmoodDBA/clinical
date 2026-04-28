using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Hosting;

namespace MDVision.IEHR.Common
{
    public class Versioning
    {
        public static string Tag(string rootRelativePath)
        {
            if (HttpRuntime.Cache[rootRelativePath] == null)
            {
//                string absolute = HostingEnvironment.MapPath("~" + rootRelativePath);
                    string absolute = HostingEnvironment.MapPath("~"+rootRelativePath);

                DateTime date = File.GetLastWriteTime(absolute);
                int index = ("~" + rootRelativePath).LastIndexOf('/');

                string result = ("~" + rootRelativePath).Insert(index, "/v-" + date.Ticks);
                HttpRuntime.Cache.Insert("~"+rootRelativePath,   HostingEnvironment.ApplicationVirtualPath+ result, new CacheDependency(absolute));
            }

            //string test =  "~" + (HttpRuntime.Cache[("~"+rootRelativePath)] as string);
            string test =  (HttpRuntime.Cache[("~" + rootRelativePath)] as string);
            test = test.Replace('~', '/');
            return test;
        }
        public static string Version(string rootRelativePath)
        {
            if (HttpRuntime.Cache[rootRelativePath] == null)
            {
                var absolutePath = HostingEnvironment.MapPath(rootRelativePath);
                var lastChangedDateTime = File.GetLastWriteTime(absolutePath);

                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;

               
 
                if (rootRelativePath.StartsWith("~"))
                {
                    //rootRelativePath = rootRelativePath.Substring(1);
                }
 
                //var versionedUrl = rootRelativePath + "?v=" + lastChangedDateTime.Ticks;
                var versionedUrl = rootRelativePath + "?v=" + version;

                HttpRuntime.Cache.Insert(rootRelativePath, HostingEnvironment.ApplicationVirtualPath + versionedUrl.Substring(1), new CacheDependency(absolutePath));
            }
 
            return HttpRuntime.Cache[rootRelativePath] as string;
        
    }
    }
}