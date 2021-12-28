using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PS.Notification.Abstractions.Models
{
    public abstract class BaseMailModel
    {
        protected string _pattern = "@[A-Za-z0-9\\.]+";
        protected string _templatePath = "Templates/Mail/";
        protected string _templateName;

        public BaseMailModel(string templateName) => _templateName = templateName;
        protected async Task<string> ReadContentAsync() => await File.ReadAllTextAsync($"{Directory.GetCurrentDirectory()}/{_templatePath}/{_templateName}");
        public async Task<string> GetMailContentAsync() => Regex.Replace(await ReadContentAsync(), _pattern, (match) => GetType().GetProperty(match.Value?.TrimStart('@') ?? "Undefined")?.GetValue(this)?.ToString() ?? "");

    }
}
