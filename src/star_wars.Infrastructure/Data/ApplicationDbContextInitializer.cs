using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using star_wars.Core.Constants;
using star_wars.Core.Entities;
using star_wars.Infrastructure.Data.Identity;

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
    
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitializer(
        ILogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context, 
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
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
        Guid userId = Guid.NewGuid();
        
        if (!_context.Users.Any() && !_context.Roles.Any())
            userId = await SeedUserAndRole();
        
        if (!_context.Movies.Any())
            await SeedMovies();
        
        if (!_context.Characters.Any())
            await SeedCharacters(userId);
        
        await _context.SaveChangesAsync();
    }

    private async Task<Guid> SeedUserAndRole()
    {
        var userRole = new IdentityRole(Roles.User);

        if (_roleManager.Roles.All(r => r.Name != userRole.Name))
        {
            await _roleManager.CreateAsync(userRole);
        }
        
        var user = new ApplicationUser { UserName = "user@localhost", Email = "user@localhost" };

        if (_userManager.Users.All(u => u.UserName != user.UserName))
        {
            await _userManager.CreateAsync(user, "User1!");
            if (!string.IsNullOrWhiteSpace(userRole.Name))
            {
                await _userManager.AddToRolesAsync(user, new [] { userRole.Name });
            }
        }

        return Guid.Parse(user.Id);
    }

    private async Task SeedMovies()
    {
        List<Movie> movies = new List<Movie>
        {
            new Movie { Title = "Эпизод I: Скрытая угроза", },
            new Movie { Title = "Эпизод II: Атака клонов" },
            new Movie { Title = "Эпизод III: Месть ситхов" },
            new Movie { Title = "Эпизод IV: Новая надежда" },
            new Movie { Title = "Эпизод V: Империя наносит ответный удар" },
            new Movie { Title = "Эпизод VI: Возвращение джедая" },
            new Movie { Title = "Эпизод VII: Пробуждение Силы" },
            new Movie { Title = "Эпизод VIII: Последние джедаи" },
            new Movie { Title = "Эпизод IX: Восход Скайуокера" },
            new Movie { Title = "Звёздные войны: Мандалорец" },
            new Movie { Title = "Звёздные войны. Истории: Рог одного" },
            new Movie { Title = "Звёздные войны: Повстанцы" },
        };
        await _context.AddRangeAsync(movies);
        await _context.SaveChangesAsync();
    }

    private async Task SeedCharacters(Guid ownerId)
    {
        var movies = await _context.Movies.ToListAsync();

        List<Character> characters = new List<Character>
        {
            new Character
            {
                OwnerId = ownerId,
                Name = "Дарт Вейдер",
                OriginName = "Darth Vader",
                Birthdate = -42,
                Planet = "Татуин",
                Gender = "Мужской",
                Species = "Человек",
                Height = 2.02F,
                HairColor = "Русые",
                EyeColor = "Голубой",
                Description =
                    "Энакин Скайуокер (англ. Anakin Skywalker, сокращённо Эни) — легендарный чувствительный к " +
                    "Силе человек, мужчина, который служил Галактической Республике как рыцарь-джедай, позже " +
                    "служивший Галактической Империи и командовавший её войсками, как Лорд ситхов Дарт Вейдер. " +
                    "Рождённый Шми Скайуокер, в юности стал тайным мужем сенатора с Набу, Падме Амидалы Наберри. " +
                    "Он был отцом гранд-мастера Люка Скайуокера, рыцаря-джедая Леи Органы-Соло и дедом Бена Скайуокера. " +
                    "Далёкими потомками Энакина Скайуокера были Нат, Кол и Кейд Скайуокеры.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Эпизод IV: Новая надежда"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод V: Империя наносит ответный удар"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VI: Возвращение джедая"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод III: Месть ситхов"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод II: Атака клонов"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод I: Скрытая угроза")
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Йода",
                OriginName = "Yoda",
                Birthdate = -896,
                Planet = "Планета Йоды",
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
                    movies.FirstOrDefault(m => m.Title == "Эпизод I: Скрытая угроза"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод II: Атака клонов"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод III: Месть ситхов"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод V: Империя наносит ответный удар"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VI: Возвращение джедая"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Дин Джарин",
                OriginName = "Din Djarin",
                Birthdate = 20,
                Planet = "Ак Ветина",
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
                              "головами.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Звёздные войны: Мандалорец"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Люк Скайуокер",
                OriginName = "Luke Skywalker",
                Birthdate = -19,
                Planet = "Татуин",
                Gender = "Мужской",
                Species = "Человек",
                Height = 1.72F,
                HairColor = "Светлые",
                EyeColor = "Синие",
                Description = "Люк Скайуокер (англ. Luke Skywalker) — герой саги «Звёздные войны». " +
                              "Он является сыном Анакина Скайуокера и Шми Скайуокер, двоюродным братом Леи " +
                              "Органы-Соло и отцом Бена Скайуокера. Люк обучался рыцарю-джедаю Оби-Вану " +
                              "Кеноби и стал последним из джедаев. Он сыграл решающую роль в падении " +
                              "Империи и победе Новой Республики.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Эпизод IV: Новая надежда"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VII: Пробуждение Силы"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VIII: Последние джедаи"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод IX: Восход Скайуокера"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод V: Империя наносит ответный удар"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VI: Возвращение джедая"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Оби-Ван Кеноби",
                OriginName = "Obi-Wan Kenobi",
                Birthdate = -57,
                Planet = "Стор Скалинг",
                Gender = "Мужской",
                Species = "Человек",
                Height = 1.82F,
                HairColor = "Рыжие",
                EyeColor = "Голубые",
                Description = "Оби-Ван Кеноби (англ. Obi-Wan Kenobi) — легендарный джедай, обучавший Анакина " +
                              "Скайуокера и его сына Люка. Он сыграл ключевую роль в событиях Саги «Звёздные " +
                              "войны», особенно в битвах с Ситхами и участием в Гражданской войне. В конечном " +
                              "итоге, он смог покончить с Дартом Вейдером и вернуть мир и справедливость в " +
                              "галактику.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Эпизод I: Скрытая угроза"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод II: Атака клонов"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод III: Месть ситхов"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод V: Империя наносит ответный удар"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VI: Возвращение джедая"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод IV: Новая надежда"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Дарт Сидиус",
                OriginName = "Darth Sidious",
                Birthdate = -82,
                Planet = "Набу",
                Gender = "Мужской",
                Species = "Человек",
                Height = 1.73F,
                HairColor = "Отсутствует",
                EyeColor = "Синие",
                Description = "Дарт Сидиус (англ. Darth Sidious) — ситх, главный ведущий во всей истории Ордена " +
                              "ситхов, канцлер Галактической Республики под именем Палпатин. Он является основным " +
                              "злодеем в предыстории и в исходной трилогии «Звёздных войн», а также одним из главных " +
                              "злодеев в позднейшей трилогии.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Эпизод I: Скрытая угроза"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод II: Атака клонов"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод III: Месть ситхов"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VI: Возвращение джедая"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Хан Соло",
                OriginName = "Han Solo",
                Birthdate = -29,
                Planet = "Кореллия",
                Gender = "Мужской",
                Species = "Человек",
                Height = 1.8F,
                HairColor = "Коричневые",
                EyeColor = "Коричневые",
                Description = "Хан Соло (англ. Han Solo) — капитан межзвёздного корабля «Милениум Фалькон», " +
                              "который сыграл важную роль в событиях оригинальной трилогии «Звёздные войны». " +
                              "Хан был известен своим грубым характером и нестандартными методами, но глубоко " +
                              "проявил доброту и героизм в решающие моменты.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Эпизод IV: Новая надежда"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод V: Империя наносит ответный удар"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VI: Возвращение джедая"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VII: Пробуждение Силы"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VIII: Последние джедаи"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Лея Органа-Соло",
                OriginName = "Leia Organa-Solo",
                Birthdate = -19,
                Planet = "Альдераан",
                Gender = "Женский",
                Species = "Человек",
                Height = 1.5F,
                HairColor = "Тёмные",
                EyeColor = "Коричневые",
                Description = "Лея Органа-Соло (англ. Leia Organa-Solo) — политический лидер и генерал Республики, " +
                              "одна из главных героинь саги «Звёздные войны». Лея была членом Совета Сената, лидером " +
                              "Альянса повстанцев и генералом сопротивления. Она также была двоюродной сестрой Люка " +
                              "Скайуокера и мать Бена Скайуокера.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Эпизод IV: Новая надежда"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод V: Империя наносит ответный удар"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VI: Возвращение джедая"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VII: Пробуждение Силы"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VIII: Последние джедаи"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод IX: Восход Скайуокера"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Рей",
                OriginName = "Rey",
                Birthdate = 15,
                Planet = "Джакку",
                Gender = "Женский",
                Species = "Неизвестно",
                Height = 1.7F,
                HairColor = "Коричневые",
                EyeColor = "Серые",
                Description = "Рей (англ. Rey) — главная героиня сиквелов трилогии «Звёздные войны». Она является " +
                              "молодой женщиной, обладающей силой, которая проявилась в эпоху Новой Республики. Рей " +
                              "стала ключевой фигурой в сражении с Первым Орденом и поисках истины о себе и своём " +
                              "происхождении.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Эпизод VII: Пробуждение Силы"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VIII: Последние джедаи"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод IX: Восход Скайуокера"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Финн",
                OriginName = "Finn",
                Birthdate = -11,
                Planet = "Неизвестно",
                Gender = "Мужской",
                Species = "Человек",
                Height = 1.78F,
                HairColor = "Чёрные",
                EyeColor = "Тёмные",
                Description = "Финн (англ. Finn) — герой сиквелов трилогии «Звёздные войны». Ранее известный как " +
                              "FN-2187, Финн был солдатом Первого Ордена, но покинул его, чтобы присоединиться к " +
                              "сопротивлению против тирании Ордена. Он стал ключевой фигурой в борьбе с Первым Орденом " +
                              "и в поисках своего истинного места в галактике.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Эпизод VII: Пробуждение Силы"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VIII: Последние джедаи"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод IX: Восход Скайуокера"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "По Дамерон",
                OriginName = "Poe Dameron",
                Birthdate = -8,
                Planet = "Явин 4",
                Gender = "Мужской",
                Species = "Человек",
                Height = 1.78F,
                HairColor = "Чёрные",
                EyeColor = "Коричневые",
                Description = "По Дамерон (англ. Poe Dameron) — пилот и герой сиквелов трилогии «Звёздные войны». " +
                              "Он был членом сопротивления и лидером Эскадрильи Верховного Красного, специализирующейся " +
                              "на истребителях X-Wing. По сыграл важную роль в сражении против Первого Ордена и борьбе " +
                              "за свободу галактики.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Эпизод VII: Пробуждение Силы"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VIII: Последние джедаи"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод IX: Восход Скайуокера"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Кайло Рен",
                OriginName = "Kylo Ren",
                Birthdate = -5,
                Planet = "Чандрила",
                Gender = "Мужской",
                Species = "Человек",
                Height = 1.89F,
                HairColor = "Чёрные",
                EyeColor = "Тёмные",
                Description = "Кайло Рен (англ. Kylo Ren) — великий воитель силы и главный злодей сиквелов трилогии " +
                              "«Звёздные войны». Ранее известный как Бен Соло, сын Хана Соло и Леи Органа-Соло. Кайло " +
                              "стал приверженцем Первого Ордена и рыцарем Рена, под покровительством верховного лидера " +
                              "Сноук. Он играл важную роль в событиях борьбы за силу и своё место в галактике.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Эпизод VII: Пробуждение Силы"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод VIII: Последние джедаи"),
                    movies.FirstOrDefault(m => m.Title == "Эпизод IX: Восход Скайуокера"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Джин Эрсо",
                OriginName = "Jyn Erso",
                Birthdate = -21,
                Planet = "Валтира",
                Gender = "Женский",
                Species = "Человек",
                Height = 1.68F,
                HairColor = "Тёмные",
                EyeColor = "Коричневые",
                Description =
                    "Джин Эрсо (англ. Jyn Erso) — героиня фильма «Звёздные войны. Истории», которая сыграла " +
                    "важную роль в событиях, предшествующих оригинальной трилогии. Джин была частью " +
                    "повстанческого движения и вместе с командой отправилась на смертельно опасную миссию, " +
                    "чтобы похитить планы Звезды Смерти.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Звёздные войны. Истории: Рог одного"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Кассиан Андор",
                OriginName = "Cassian Andor",
                Birthdate = -26,
                Planet = "Фест",
                Gender = "Мужской",
                Species = "Человек",
                Height = 1.78F,
                HairColor = "Чёрные",
                EyeColor = "Коричневые",
                Description =
                    "Кассиан Андор (англ. Cassian Andor) — герой фильма «Звёздные войны. Истории», в котором " +
                    "показаны события, предшествующие оригинальной трилогии. Кассиан был разведчиком и " +
                    "шпионом в повстанческом движении. Он сыграл важную роль в миссии по похищению планов " +
                    "Звезды Смерти.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Звёздные войны. Истории: Рог одного"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Джаред",
                OriginName = "Saw Gerrera",
                Birthdate = -41,
                Planet = "Ондерон",
                Gender = "Мужской",
                Species = "Человек",
                Height = 1.88F,
                HairColor = "Белые",
                EyeColor = "Коричневые",
                Description = "Джаред (англ. Saw Gerrera) — герой фильма «Звёздные войны. Истории» и анимационного " +
                              "сериала «Звёздные войны: Повстанцы». Он был военным лидером в повстанческом движении " +
                              "и играл важную роль в сражении против Империи. Джаред также является наставником " +
                              "Джина Эрсо в её миссии по похищению планов Звезды Смерти.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Звёздные войны. Истории: Рог одного"),
                    movies.FirstOrDefault(m => m.Title == "Звёздные войны: Повстанцы"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Бодди Риггс",
                OriginName = "Bodhi Rook",
                Birthdate = -25,
                Planet = "Джедха",
                Gender = "Мужской",
                Species = "Человек",
                Height = 1.72F,
                HairColor = "Чёрные",
                EyeColor = "Тёмные",
                Description = "Бодди Риггс (англ. Bodhi Rook) — герой фильма «Звёздные войны. Истории», который " +
                              "показывает события, предшествующие оригинальной трилогии. Бодди был грузчиком в " +
                              "Имперском флоте, но потом перешёл на сторону повстанцев. Он сыграл важную роль " +
                              "в миссии по похищению планов Звезды Смерти.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Звёздные войны. Истории: Рог одного"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Чирру Имвэй",
                OriginName = "Chirrut Îmwe",
                Birthdate = -51,
                Planet = "Джедха",
                Gender = "Мужской",
                Species = "Человек",
                Height = 1.75F,
                HairColor = "Чёрные",
                EyeColor = "Коричневые",
                Description = "Чирру Имвэй (англ. Chirrut Îmwe) — герой фильма «Звёздные войны. Истории». Он был " +
                              "слепым воином и верующим в Силу. Чирру сыграл важную роль в миссии по похищению " +
                              "планов Звезды Смерти и поддерживал своих товарищей своей силой и мудростью.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Звёздные войны. Истории: Рог одного"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Гальен Эрсо",
                OriginName = "Galen Erso",
                Birthdate = -52,
                Planet = "Валтира",
                Gender = "Мужской",
                Species = "Человек",
                Height = 1.78F,
                HairColor = "Тёмные",
                EyeColor = "Коричневые",
                Description = "Гальен Эрсо (англ. Galen Erso) — герой фильма «Звёздные войны. Истории». Он был " +
                              "галактическим учёным и инженером. Гальен сыграл важную роль в строительстве " +
                              "Звезды Смерти, но позже перешёл на сторону повстанцев и помог им в борьбе против " +
                              "Империи.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Звёздные войны. Истории: Рог одного"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Квилл",
                OriginName = "Kuiil",
                Birthdate = -64,
                Planet = "Арволь",
                Gender = "Мужской",
                Species = "Угнот",
                Height = 1.00F,
                HairColor = "Отсутствует",
                EyeColor = "Коричневые",
                Description = "Квилл (англ. Kuiil) — герой сериала «Звёздные войны: Мандалорец». Он был угнотом, " +
                              "который жил на пустынной планете Арволь. Квилл стал союзником главного героя — " +
                              "мандалорца, и помогал ему в его миссиях.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Звёздные войны: Мандалорец"),
                }
            },
            new Character
            {
                OwnerId = ownerId,
                Name = "Гроугу",
                OriginName = "Grogu",
                Birthdate = -41,
                Planet = "Неизвестно",
                Gender = "Мужской",
                Species = "Неизвестно",
                Height = 0.50F,
                HairColor = "Зелёные",
                EyeColor = "Коричневые",
                Description = "Гроугу (англ. Grogu), также известный как Малыш Йода или Малыш, — главный герой " +
                              "сериала «Звёздные войны: Мандалорец». Он является представителем неизвестной " +
                              "видовой группы и обладает силой. Гроугу стал центральной фигурой в сериале и вызвал " +
                              "большой интерес у фанатов.",
                Movies = new List<Movie>
                {
                    movies.FirstOrDefault(m => m.Title == "Звёздные войны: Мандалорец"),
                }
            }
        };
        await _context.AddRangeAsync(characters);
    }
}