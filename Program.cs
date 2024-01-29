namespace AukcioProjekt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Festmeny> festmenyek = new() {
            new Festmeny("Mona Lisa", "Leonardo da Vinci", "Reneszánsz"),
            new Festmeny("A Horatiák esküje", "Jacques-Louis David", "Klasszicizmus"),
         };
            Console.Write("Adj meg egy darabszámot: ");
            int darabszam = int.Parse(Console.ReadLine() ?? "0");

            for (int i = 0; i < darabszam; i++)
            {
                string cim = "";
                string festo = "";
                string stilus = "";
                while (cim == "" || festo == "" || stilus == "")
                {
                    Console.Clear();
                    Console.WriteLine("Add meg a festmény címét!");
                    cim = Console.ReadLine() ?? "";
                    Console.WriteLine("Add meg a festmény festőjét!");
                    festo = Console.ReadLine() ?? "";
                    Console.WriteLine("Add meg a festmény stílusát!");
                    stilus = Console.ReadLine() ?? "";
                }
                festmenyek.Add(new Festmeny(cim, festo, stilus));
            }
            Console.Clear();

            Random rnd = new();
            for (int i = 0; i < 20; i++)
            {
                int festmenyIndex = rnd.Next(festmenyek.Count);
                Festmeny festmeny = festmenyek[festmenyIndex];
                if (festmeny.Elkelt)
                {
                    i--;
                    continue;
                }
                if (festmeny.LicitekSzama == 0)
                {
                    festmeny.Licit();
                }
                else
                {
                    festmeny.Licit(rnd.Next(10, 101));
                }
            }

            int sorszam = -1;
            while (sorszam != 0)
            {
                Console.WriteLine("Add meg a festmény sorszámát! (0 kilépés)");
                Console.WriteLine("Festmények listája:");
                for (int i = 0; i < festmenyek.Count; i++)
                {
                    Festmeny f = festmenyek[i];
                    Console.WriteLine($"{i + 1}. {f.Festo}: {f.Cim} ({f.Stilus})");
                }

                bool sorszamIsInt = int.TryParse(Console.ReadLine() ?? "-1", out sorszam);
                if (!sorszamIsInt || sorszam < 0 || sorszam > festmenyek.Count)
                {
                    Console.WriteLine("Hibás sorszám!");
                    continue;
                }
                if (sorszam == 0) break;

                Festmeny festmeny = festmenyek[sorszam - 1];
                if (festmeny.Elkelt)
                {
                    Console.WriteLine("A festmény már elkelt!");
                    continue;
                }
                Console.WriteLine("Add meg a licit mértékét! (üresen hagyva 10%)");
                string licitMertekString = Console.ReadLine() ?? "10";
                if (licitMertekString == "") licitMertekString = "10";
                if (!int.TryParse(licitMertekString, out int licitMertek))
                {
                    Console.WriteLine("Hibás licit mérték!");
                    break;
                }
                if (licitMertek < 10 || licitMertek > 100)
                {
                    Console.WriteLine("A licit mértéke 10 és 100 közötti szám lehet.");
                    continue;
                }
                if (festmeny.LegutolsoLicitIdeje.AddMinutes(2) < DateTime.Now)
                {
                    festmeny.Elkelt = true;
                    Console.WriteLine("A festmény elkelt!");
                    continue;
                }
                if (licitMertek == 10) festmeny.Licit();
                else if (licitMertek > 10) festmeny.Licit(licitMertek);
            }
            festmenyek.ForEach(festmeny =>
            {
                if (festmeny.LicitekSzama > 0) festmeny.Elkelt = true;
            });

            Console.WriteLine("Festmények listája:");
            foreach (Festmeny festmeny in festmenyek)
            {
                Console.WriteLine(festmeny);
            }

            Console.WriteLine("\nLegdrágábban elkelt festmény:");
            Festmeny legdragabbanElkeltFestmeny = festmenyek[0];
            foreach (Festmeny festmeny in festmenyek)
            {
                if (festmeny.Elkelt && festmeny.LegmagasabbLicit > legdragabbanElkeltFestmeny.LegmagasabbLicit)
                {
                    legdragabbanElkeltFestmeny = festmeny;
                }
            }
            if (legdragabbanElkeltFestmeny.Elkelt) Console.WriteLine(legdragabbanElkeltFestmeny);
            else Console.WriteLine("Nincs elkelt festmény.");

            bool vanTobbMintTizLicit = festmenyek.Any(festmeny => festmeny.LicitekSzama > 10);
            if (vanTobbMintTizLicit)
            {
                Console.WriteLine("\nVan olyan festmény, amelyre 10-nél több alkalommal licitáltak.");
            }
            else
            {
                Console.WriteLine("\nNincs olyan festmény, amelyre 10-nél több alkalommal licitáltak.");
            }

            int nemElkeltFestmenyekSzama = festmenyek.Count(festmeny => !festmeny.Elkelt);
            Console.WriteLine($"\nNem elkelt festmények száma: {nemElkeltFestmenyekSzama}");

            Console.WriteLine("\nFestmények listája rendezve a licitek nagysága alapján:");
            List<Festmeny> rendezettFestmenyek = new(festmenyek.OrderByDescending(festmeny => festmeny.LegmagasabbLicit));
            foreach (Festmeny festmeny in rendezettFestmenyek)
            {
                Console.WriteLine(festmeny);
            }

            File.WriteAllLines("festmenyek_rendezett.csv", rendezettFestmenyek.Select(festmeny => $"{festmeny.Cim};{festmeny.Festo};{festmeny.Stilus};{festmeny.LicitekSzama};{festmeny.LegmagasabbLicit};{festmeny.LegutolsoLicitIdeje};{festmeny.Elkelt}"));

            Console.ReadKey();
        }
    }
}
