using FinancasFlow.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FinancasFlow.Services;

namespace FinancasFlow
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Configuração do banco de dados SQLite
            string caminhoBanco = Path.Combine(
                FileSystem.AppDataDirectory,
                "financasflow.db3"
            );

            builder.Services.AddDbContextFactory<FinancasFlowDbContext>(
                options =>
                    options.UseSqlite($"Data Source={caminhoBanco}")
            );

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<BancoDadosInicializador>();

            builder.Services.AddSingleton<MensagemChatService>();

            return builder.Build();
        }
    }
}
