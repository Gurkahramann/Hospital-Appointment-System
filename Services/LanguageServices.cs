using System.Reflection;
using Microsoft.Extensions.Localization;

namespace AspWebProgram.Services
{
    public class LanguageServices
    {
        private readonly IStringLocalizer _localizer;
        public LanguageServices(IStringLocalizerFactory factory)
        {
            
            var type=typeof(SharedResource);
            var assemblyName=new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer=factory.Create(nameof(SharedResource),assemblyName.Name);
        }
        public LocalizedString GetKey(string key)
        {
            return _localizer[key];
        }
    }
}