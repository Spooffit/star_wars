using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using star_wars.Core.Entities;

namespace star_wars.Infrastructure.Data;


public static class InitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

        await initializer.InitialiseAsync();
        await initializer.SeedAsync();
    }
}
public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    
    public ApplicationDbContextInitializer(
        ILogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task InitialiseAsync()
    {
        if ((await _context.Database.GetPendingMigrationsAsync()).Any())
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database");
                throw;
            }
        }
        await _context.Database.EnsureCreatedAsync();
    }
    
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!_context.Characters.Any())
        {
            SeedCharacters();

            await _context.SaveChangesAsync();
        }
    }
    
    private void SeedCharacters()
    {
        List<Character> characters = new List<Character>
        {
            new Character
            {
                Id = 1,
                Name = "Дарт Вейдер",
                OriginName = "Darth Vader",
                Birthdate = -42,
                Planet = new Planet{ Name = "Татуин", Id = 2},
                Gender = "Мужской",
                Species = "Человек",
                Height = 2.02F,
                HairColor = "Русые",
                EyeColor = "Голубой",
                Description = "Энакин Скайуокер (англ. Anakin Skywalker, сокращённо Эни) — легендарный чувствительный к " +
                              "Силе человек, мужчина, который служил Галактической Республике как рыцарь-джедай, позже " +
                              "служивший Галактической Империи и командовавший её войсками, как Лорд ситхов Дарт Вейдер. " +
                              "Рождённый Шми Скайуокер, в юности стал тайным мужем сенатора с Набу, Падме Амидалы Наберри. " +
                              "Он был отцом гранд-мастера Люка Скайуокера, рыцаря-джедая Леи Органы-Соло и дедом Бена Скайуокера. " +
                              "Далёкими потомками Энакина Скайуокера были Нат, Кол и Кейд Скайуокеры.",
                Movies = new List<Movie>
                {
                    new Movie{Title = "Новая надежда", Id = 3}
                }
            },
            new Character
            {
                Id = 2,
                Name = "Йода",
                OriginName = "Yoda",
                Birthdate = -896,
                Planet = new Planet{ Name = "Дагоба", Id = 1},
                Gender = "Мужской",
                Species = "Раса Йоды",
                Height = 0.66F,
                HairColor = "Белый",
                EyeColor = "Зелёный",
                Description = "Йода (англ. Yoda) — гранд-мастер Ордена джедаев, был одним из самых сильных и мудрых " +
                              "джедаев своего времени. Место в Совете получил спустя примерно сотню лет после рождения. " +
                              "Обладая долголетием, он достиг титула гранд-мастера в возрасте примерно 600 лет. Йода сумел " +
                              "выжить во время приказа 66. После неудачной дуэли с Дартом Сидиусом ушел в добровольное " +
                              "изгнание на планету Дагоба, где и умер естественной смертью в 4 ПБЯ. Родная планета и раса " +
                              "Йоды неизвестны.",
                Movies = new List<Movie>
                {
                    new Movie {Title = "Ученик джедая: В силу тесной связи", Id = 4}
                }
            },
            new Character
            {
                Id = 3,
                Name = "Дин Джарин",
                OriginName = "Din Djarin",
                Birthdate = 20,
                Planet = new Planet{ Name = "Ак Ветина", Id = 3},
                Gender = "Мужской",
                Species = "Человек",
                Height = 1.80F,
                HairColor = "Оливковая",
                EyeColor = "Карие",
                Description = "Дин Джарин (англ. Din Djarin), известный также под псевдонимом «Мандалорец» " +
                              "(англ. Mandalorian) или просто «Мандо» (англ. Mando) — мужчина-мандалорец, охотник " +
                              "за головами в эпоху Новой Республики, несколько лет после Галактической гражданской " +
                              "войны. Он носил традиционную мандалорскую броню и странствовал по Внешнему Кольцу, " +
                              "вдали от власти Республики. Он был стрелком-одиночкой и членом гильдии Охотников за " +
                              "головами. .",
                Movies = new List<Movie>
                {
                    new Movie{Title = "Глава 1: Мандалорец", Id = 2}
                }
            },
            new Character
            {
                Id = 4,
                Name = "Гален Эрсо",
                OriginName = "Galen Erso",
                Birthdate = 56,
                Planet = new Planet{ Name = "Грейндж", Id = 4},
                Gender = "Мужской",
                Species = "Человек",
                Height = 1.8F,
                HairColor = "Темно-рыжие",
                EyeColor = "Карие",
                Description = "Гален Уолтон Эрсо[2] (англ. Galen Walton Erso) — мужчина-человек, выдающийся учёный, " +
                              "искавший способы использования кристаллов в качестве мощного источника энергии. " +
                              "Уроженец захолустной сельскохозяйственной планеты Грейндж, Эрсо стал участником " +
                              "научно-исследовательской программы «Будущее Республики». Исследования Галена, " +
                              "вопреки его собственной воле, существенно помогли Галактической Империи в " +
                              "разработке и создании оружия массового террора — «Звезды Смерти». ",
                Movies = new List<Movie>
                {
                    new Movie{Title = "Звёздные войны: Изгой-один, часть 1", Id = 1}
                }
            }
        };

        _context.Characters.AddRange(characters);
    }
}