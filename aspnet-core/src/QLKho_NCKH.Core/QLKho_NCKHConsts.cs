using QLKho_NCKH.Debugging;

namespace QLKho_NCKH
{
    public class QLKho_NCKHConsts
    {
        public const string LocalizationSourceName = "QLKho_NCKH";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "712acdfda51841a89e35b31c9880c4e3";
    }
}
