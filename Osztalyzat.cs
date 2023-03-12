using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfOsztalyzas
{
    public class Osztalyzat
    {
        public String nev;
        String datum;
        String tantargy;
        int jegy;
        public String csaladiNev;

        public Osztalyzat(string nev, string datum, string tantargy, int jegy, string csaladiNev)
        {
            this.nev = nev;
            this.datum = datum;
            this.tantargy = tantargy;
            this.jegy = jegy;
            this.csaladiNev = csaladiNev;
        }

        public string Nev { get => nev;  }
        public string Datum { get => datum;  }
        public string Tantargy { get => tantargy; }
        public int Jegy { get => jegy; }
        public string CsaladiNev { get => csaladiNev;}

		public static List<Osztalyzat> ForditottNev(List<Osztalyzat> lista)
		{
            foreach (var item in lista)
            {
                string[] tomb = item.nev.Split(" ");
                string[] forditott = tomb.Reverse().ToArray();
                item.nev = forditott[0]+" "+forditott[1];
            }

            return lista;
		}

	}
	

	//todo Bővítse az osztályt! Készítsen CsaladiNev néven property-t, ami a névből a családi nevet adja vissza. Feltételezve, hogy a névnek csak az első tagja az.

	//todo Készítsen metódust ForditottNev néven, ami a két tagból álló nevek esetén megfordítja a névtagokat. Pld. Kiss Ádám => Ádám Kiss
}
