using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace QLKho_NCKH.Localization
{
    public static class QLKho_NCKHLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(QLKho_NCKHConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(QLKho_NCKHLocalizationConfigurer).GetAssembly(),
                        "QLKho_NCKH.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
