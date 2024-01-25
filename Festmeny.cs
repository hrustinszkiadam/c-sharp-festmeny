namespace AukcioProjekt {
   class Festmeny(string cim, string festo, string stilus) {
      string cim = cim;
      string festo = festo;
      string stilus = stilus;
      int licitekSzama = 0;
      int legmagasabbLicit = 0;
      DateTime legutolsoLicitIdeje;
      bool elkelt = false;

      public string Cim { get => cim; }
      public string Festo { get => festo; }
      public string Stilus { get => stilus; }
      public int LicitekSzama { get => licitekSzama; }
      public int LegmagasabbLicit { get => legmagasabbLicit; }
      public DateTime LegutolsoLicitIdeje { get => legutolsoLicitIdeje; }
      public bool Elkelt { get => elkelt; set => elkelt = value; }


      private void LicitHelper(int ujLicit) {
         string ujLicitString = ujLicit.ToString();
         string ujLicitSubString = ujLicitString[..2];
         for(int i = 2; i < ujLicitString.Length; i++) {
            ujLicitSubString += "0";
         }
         ujLicit = int.Parse(ujLicitSubString);

         legmagasabbLicit = ujLicit;
         licitekSzama++;
         legutolsoLicitIdeje = DateTime.Now;
      }

      public void Licit() {
         if(elkelt) {
            Console.WriteLine("A festmény már elkelt.");
            return;
         }
         if(licitekSzama == 0) {
            LicitHelper(100);
            return;
         }
         Licit(10);
      }

      public void Licit(int mertek) {
         if(mertek < 10 || mertek > 100) {
            Console.WriteLine("A licit mértéke 10 és 100 közötti szám lehet.");
            return;
         }
         if(elkelt) {
            Console.WriteLine("A festmény már elkelt.");
            return;
         }
         int ujLicit = (int)(legmagasabbLicit * (1 + (double)mertek / 100));
         LicitHelper(ujLicit);
      }

      override public string ToString() {
         string elkeltString = elkelt ? "Elkelt\n" : "";
         return $"{festo}: {cim} ({stilus})\n{elkeltString}{legmagasabbLicit} $ - {legutolsoLicitIdeje} (összesen: {licitekSzama} db)";
      }

   }
}