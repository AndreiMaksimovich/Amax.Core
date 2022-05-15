// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.Text;

#endregion

namespace Amax.Localization
{
    
    public static class LocalizedStringIdUtils
    {
        
        public static string Join(params string[] ids)
        {
            var sb = new StringBuilder();
            foreach (var id in ids)
            {
                if (string.IsNullOrEmpty(id)) continue;
                if (sb.Length != 0 && sb[sb.Length - 1] != '.' && id[0] != '.') sb.Append('.');
                sb.Append(id);
            }
            if (sb.Length == 0) return null;
            var result = sb[sb.Length - 1] == '.' ? sb.ToString(0, sb.Length - 1) : sb.ToString();
            return result;
        }
        
    }
}