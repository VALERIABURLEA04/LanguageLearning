namespace sa.Models;

public class CourseCategory
{
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ShortTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string LongDescription { get; set; } = string.Empty;
    public string BackgroundColor { get; set; } = "#ffc41a";
    public string TextColor { get; set; } = "#ffffff";
    public string Image { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string Icon { get; set; } = "👤";
    public string Frequency { get; set; } = string.Empty;
    public string DurationLabel { get; set; } = "Durata nivelului";
    public string PriceLabel { get; set; } = "Prețuri";
    public int Price { get; set; }
    public int OldPrice { get; set; }
    public string PriceUnit { get; set; } = "MDL";
    public string PriceNote { get; set; } = string.Empty;
    public int DiscountPercent => OldPrice > 0 ? (int)Math.Round((1 - (double)Price / OldPrice) * 100) : 0;
    public List<string> Features { get; set; } = new();

    public static readonly List<CourseCategory> All = new()
    {
        new CourseCategory
        {
            Slug = "adulti",
            Title = "Engleză pentru adulți",
            ShortTitle = "Engleză\npentru adulți",
            Description = "Cursuri intensive pentru adulți, de la începători la avansați.",
            LongDescription = "Programul nostru pentru adulți este construit în jurul comunicării reale — situații de la birou, călătorii, interviuri și conversații cotidiene. Învățați cu profesori certificați CELTA/DELTA, în grupuri mici de maximum 6 persoane, cu feedback săptămânal 1:1.",
            BackgroundColor = "#ff7a45",
            Image = "/assets/img/course/1.png",
            Icon = "👨",
            Level = "A1 — C2",
            Duration = "12 săptămâni",
            Frequency = "2 lecții pe săptămână",
            Price = 5490,
            OldPrice = 6990,
            PriceNote = "Opțiune de plată în rate",
            Features = new List<string>
            {
                "Grupuri mici (max 6 cursanți)",
                "Profesori certificați CELTA / DELTA",
                "Feedback 1:1 săptămânal",
                "Materiale Cambridge aliniate CEFR",
                "Certificat de absolvire"
            }
        },
        new CourseCategory
        {
            Slug = "copii",
            Title = "Engleză pentru copii",
            ShortTitle = "Engleză\npentru copii",
            Description = "Lecții jucăușe și interactive pentru copii (6–11 ani).",
            LongDescription = "Copiii învață engleza prin jocuri, cântece, povești și activități creative. Metoda noastră dezvoltă vocabularul, pronunția și încrederea în vorbire într-un mediu vesel și sigur. Lecții adaptate pentru fiecare grupă de vârstă.",
            BackgroundColor = "#ffc41a",
            Image = "/assets/img/course/2.png",
            Icon = "👦",
            Level = "Începător — A2",
            Duration = "11 săptămâni",
            Frequency = "3 lecții pe săptămână",
            Price = 4990,
            OldPrice = 6240,
            PriceNote = "Opțiune de plată în rate",
            Features = new List<string>
            {
                "Grupe de 4–8 copii, după vârstă",
                "Jocuri, cântece și storytelling",
                "Profesori specializați pe vârste mici",
                "Rapoarte lunare pentru părinți",
                "Activități online + offline"
            }
        },
        new CourseCategory
        {
            Slug = "adolescenti",
            Title = "Engleză pentru adolescenți",
            ShortTitle = "Engleză\npentru adolescenți",
            Description = "Programe pentru elevi (12–17 ani) — școală, examene, conversație.",
            LongDescription = "Cursuri concepute pentru adolescenți, alinându-se cu programa școlară și cu nevoile reale de comunicare modernă: rețele sociale, filme, muzică, vlogging. Pregătim elevii și pentru olimpiade, BAC și examene Cambridge (KET, PET, FCE).",
            BackgroundColor = "#9b30dd",
            Image = "/assets/img/course/3.png",
            Icon = "🧑",
            Level = "A2 — B2",
            Duration = "11 săptămâni",
            Frequency = "3 lecții pe săptămână",
            Price = 4990,
            OldPrice = 6240,
            PriceNote = "Opțiune de plată în rate",
            Features = new List<string>
            {
                "Suport pentru tema de la școală",
                "Pregătire olimpiade și examene",
                "Proiecte tematice (filme, muzică, gaming)",
                "Conversation Club săptămânal",
                "Acces la platforma online"
            }
        },
        new CourseCategory
        {
            Slug = "individuale",
            Title = "Lecții individuale de engleză",
            ShortTitle = "Lecții individuale\nde engleză",
            Description = "1:1 cu profesor dedicat, program flexibil.",
            LongDescription = "Lecții personalizate complet pe nevoile tale — fie că vrei să te pregătești pentru un interviu, o prezentare, sau pur și simplu să accelerezi învățarea. Orar flexibil, materiale alese împreună, progres rapid.",
            BackgroundColor = "#4ec9e9",
            Image = "/assets/img/course/4.png",
            Icon = "👩‍🏫",
            Level = "Orice nivel",
            Duration = "90 min",
            DurationLabel = "Durata lecției",
            PriceLabel = "Prețul lecției",
            Frequency = "Flexibilă",
            Price = 600,
            OldPrice = 750,
            PriceNote = "Program personalizat",
            Features = new List<string>
            {
                "Profesor dedicat, ales de tine",
                "Orar 100% flexibil",
                "Plan personalizat de studiu",
                "Online sau față în față",
                "Plată per lecție sau pachet"
            }
        },
        new CourseCategory
        {
            Slug = "bac",
            Title = "Pregătire pentru BAC",
            ShortTitle = "Pregătire\npentru BAC",
            Description = "Curs intensiv pentru examenul de Bacalaureat la engleză.",
            LongDescription = "Program structurat pe modelul subiectelor de BAC: înțelegerea textului, eseu, exercițiile de gramatică și proba orală. Simulări săptămânale, corectare detaliată, sfaturi practice pentru ziua examenului.",
            BackgroundColor = "#1f3a8a",
            Image = "/assets/img/course/5.png",
            Icon = "🎓",
            Level = "B1 — B2",
            Duration = "6 luni",
            Frequency = "2 lecții pe săptămână",
            Price = 4990,
            OldPrice = 6240,
            PriceNote = "Opțiune de plată în rate",
            Features = new List<string>
            {
                "Simulări săptămânale de BAC",
                "Toate variantele oficiale rezolvate",
                "Pregătire pentru proba orală",
                "Tehnici de redactare a eseului",
                "Garanție de progres sau bani înapoi"
            }
        },
        new CourseCategory
        {
            Slug = "toefl",
            Title = "Pregătire TOEFL",
            ShortTitle = "Pregătire\nTOEFL",
            Description = "Pregătire intensivă pentru testul TOEFL iBT.",
            LongDescription = "Curs intensiv care acoperă toate cele 4 secțiuni TOEFL: Reading, Listening, Speaking și Writing. Strategii dovedite, materiale oficiale ETS, simulări complete cronometrate și feedback detaliat pentru fiecare secțiune.",
            BackgroundColor = "#0ea27a",
            Image = "/assets/img/course/6.png",
            Icon = "📘",
            Level = "B2 — C1",
            Duration = "8 — 12 săptămâni",
            Frequency = "2 lecții pe săptămână",
            Price = 5990,
            OldPrice = 7490,
            PriceNote = "Opțiune de plată în rate",
            Features = new List<string>
            {
                "Toate 4 secțiuni (Reading/Listening/Speaking/Writing)",
                "Simulări TOEFL complete cronometrate",
                "Materiale oficiale ETS",
                "Strategii pentru scor 100+",
                "Feedback detaliat pe Speaking & Writing"
            }
        }
    };

    public static CourseCategory? FindBySlug(string slug) =>
        All.FirstOrDefault(c => c.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
}
