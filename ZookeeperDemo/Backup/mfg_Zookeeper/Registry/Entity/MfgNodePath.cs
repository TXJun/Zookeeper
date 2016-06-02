namespace mfg_Zookeeper.Registry.Entity
{
    using System;
    using System.Text;

    public class MfgNodePath
    {
        public static readonly string DClusterCatlogString = "/Direct/{0}/{1}";
        public static readonly string DClusterString = "/Direct/{0}/{1}/{2}";
        public static readonly string VClusterCatlogString = "/Virtua/{0}/ClusterList/{1}";
        public static readonly string VClusterNodeString = "/Virtua/{0}/ClusterList/{1}/{2}/{3}";
        public static readonly string VClusterString = "/Virtua/{0}/ClusterList/{1}/{2}";
        public static readonly string VServiceString = "/Virtua/{0}/ServiceList";

        public static string GetCategory(string str, string value)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string str2 in str.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if ("{1}".Equals(str2, StringComparison.CurrentCultureIgnoreCase))
                {
                    return string.Format(builder.ToString(), value);
                }
                builder.Append("/" + str2);
            }
            return string.Empty;
        }

        public static string GetRoot(string str)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string str2 in str.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (str2.StartsWith("{") && str2.EndsWith("}"))
                {
                    break;
                }
                builder.Append("/" + str2);
            }
            return builder.ToString();
        }

        public static string GetSeviceClusterType(string str)
        {
            foreach (string str2 in VServiceString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries))
            {
                str = str.Replace(str2, "");
            }
            return str.Replace("/", "");
        }

        public static string getString(int _virtual)
        {
            switch (_virtual)
            {
                case 1:
                    return VClusterString;

                case 2:
                    return VServiceString;

                case 3:
                    return DClusterString;
            }
            return string.Empty;
        }
    }
}

