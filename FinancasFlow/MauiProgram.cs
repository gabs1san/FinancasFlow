
using FinancasFlow.Data;
using FinancasFlow.Models;
using FinancasFlow.Services;
using FinancasFlow.ViewModels;
using FinancasFlow.Views;
using FinancasFlow.Views.Conteudos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinancasFlow;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont(
                    "OpenSans-Regular.ttf",
                    "OpenSansRegular");

                fonts.AddFont(
                    "OpenSans-Semibold.ttf",
                    "OpenSansSemibold");
            });

        // Banco de dados SQLite
        string caminhoBanco = Path.Combine(
            FileSystem.AppDataDirectory,
            "financasflow.db3"
        );

        builder.Services.AddDbContextFactory<FinancasFlowDbContext>(
            options =>
                options.UseSqlite($"Data Source={caminhoBanco}")
        );

        // Serviços
        builder.Services.AddScoped<TransacaoService>();
        builder.Services.AddSingleton<BancoDadosInicializador>();
        builder.Services.AddSingleton<MensagemChatService>();
        builder.Services.AddTransient<CartaoService>();

        // Agente financeiro
        builder.Services.AddTransient<Orquestrador>();

        // Memória financeira compartilhada
        builder.Services.AddSingleton(provider =>
        {
            var memoria = new MemoriaFinanceira();

            memoria.Cartoes.Add(new CartaoDeCredito
            {
                Id = 1,
                Nome = "Meu Cartão",
                Limite = 3000,
                DiaFechamento = 25,
                DiaVencimento = 5
            });

            return memoria;
        });

       

        // ViewModels
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<ResumoViewModel>();
        builder.Services.AddTransient<AdicionarViewModel>();
        builder.Services.AddTransient<AdicionarContent>();

        // views
        builder.Services.AddTransient<MainContent>();
        builder.Services.AddTransient<ResumoContent>();
        builder.Services.AddTransient<AdicionarContent>();
        builder.Services.AddTransient<PerfilContent>();

        //Navegação
        builder.Services.AddTransient<MainContainerPage>();
        builder.Services.AddTransient<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
