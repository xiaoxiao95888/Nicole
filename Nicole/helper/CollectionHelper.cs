using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Plastic.helper
{
    public static class CollectionHelper
    {
        public static string GetHtmlSourceCodeFromUrl(string pageUrl, string encode)
        {
            var encoding = Encoding.GetEncoding(encode);
            var htmlSourceCode = string.Empty;
            try
            {
                var wc = new WebClient { Credentials = CredentialCache.DefaultCredentials};
                wc.Headers.Set("User-Agent", "Microsoft Internet Explorer");
                wc.Headers.Set("Cookie", "ASP.NET_SessionId=xhynkxhdihx5lu4a4wuz2r43");
                var resStream = wc.OpenRead(pageUrl);
                if (resStream != null)
                {
                    var sr = new StreamReader(resStream, encoding);
                    htmlSourceCode = sr.ReadToEnd();
                    resStream.Close();
                }

            }
            catch (Exception ex)
            {
                //htmlSourceCode = ex.ToString();
            }
            return htmlSourceCode;
        }
        public static Dictionary<string, List<CollectionResult>> ProcessorValue(Condition[] condition, string url, string encode, string loop, string htmlCode = "")
        {
            htmlCode = string.IsNullOrEmpty(htmlCode) ? GetHtmlSourceCodeFromUrl(url, encode).Replace("\r\n", "") : htmlCode.Replace("\r\n", "");
            var sp = new[] { loop };
            var spValue = htmlCode.Split(sp, StringSplitOptions.RemoveEmptyEntries);
            var groupsId = 0;
            var needRemoveGroupsId = new List<string>();
            var dit = new Dictionary<string, List<CollectionResult>>();
            foreach (var spv in spValue)
            {
                foreach (var con in condition)
                {
                    var stringSeparators = new[] { con.Start, con.End };
                    var value = spv.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var v in value)
                    {
                        if (spv.IndexOf(con.Start + v + con.End, StringComparison.Ordinal) != -1)
                        {
                            var r = new CollectionResult
                            {
                                Value = v,
                                CreateTime = DateTime.Now,
                                ConditionName = con.Name,
                                Url = url
                            };
                            if (dit.ContainsKey(groupsId.ToString()) == false)
                            {
                                var rjs = new List<CollectionResult> {r};
                                dit.Add(groupsId.ToString(), rjs);
                            }
                            else
                            {
                                var rjs = dit[groupsId.ToString()];
                                rjs.Add(r);
                                dit[groupsId.ToString()] = rjs;
                            }

                            if (Filtered(v, con) == false)
                            {
                                needRemoveGroupsId.Add(groupsId.ToString());
                            }
                        }
                    }

                }
                groupsId++;
            }

            foreach (var i in needRemoveGroupsId.Distinct())
            {
                dit.Remove(i);
            }
            return dit;
        }
        public static bool Filtered(string value, Condition condition)
        {
            var reserved = true;
            int i;
            int j;
            var valueIsInt = int.TryParse(value, out i);
            var additionalIsInt = int.TryParse(condition.Addition, out j);

            switch (condition.Additional)
            {
                case "include":
                    reserved = value.Contains(condition.Addition);
                    break;
                case "not include":
                    reserved = !(value.Contains(condition.Addition));
                    break;
                case "larger":
                    if (valueIsInt && additionalIsInt)
                    {
                        if (i <= j)
                        {
                            reserved = false;
                        }
                    }
                    break;
                case "equal":
                    if (value != condition.Addition)
                    {
                        reserved = false;
                    }
                    break;
                case "smaller":
                    if (valueIsInt && additionalIsInt)
                    {
                        if (i >= j)
                        {
                            reserved = false;
                        }
                    }
                    break;
                case "unequal":
                    if (value == condition.Addition)
                    {
                        reserved = false;
                    }
                    break;
                case "notNull":
                    reserved = !(string.IsNullOrEmpty(value));
                    break;

            }
            return reserved;
        }
    }
}
