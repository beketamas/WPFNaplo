using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace WpfOsztalyzas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fajlNev = "naplo.txt";
        //Így minden metódus fogja tudni használni.
        List<Osztalyzat> jegyek = new List<Osztalyzat>();

        public MainWindow()
        {
            InitializeComponent();
			// todo Fájlok kitallózásával tegye lehetővé a naplófájl kiválasztását!
			// Ha nem választ ki semmit, akkor "naplo.csv" legyen az állomány neve. A későbbiekben ebbe fog rögzíteni a program.

			// todo A kiválasztott naplót egyből töltse be és a tartalmát jelenítse meg a datagrid-ben!

			OpenFileDialog ofd = new OpenFileDialog();
			if (ofd.ShowDialog() == true)
			{
				foreach (var item in File.ReadAllLines(ofd.FileName).ToList())
				{
					string[] tomb = item.Split(';');
					Osztalyzat betoltott_jegyek = new Osztalyzat(tomb[0], tomb[1], tomb[2], int.Parse(tomb[3]), tomb[4]);
					jegyek.Add(betoltott_jegyek);
				}
				dgJegyek.ItemsSource = jegyek;
				double atlag = jegyek.Average(x => x.Jegy);
				lblJegyekSzama.Content = dgJegyek.Items.Count;
				dgJegyek.Items.Refresh();
				lblAtlag.Content = atlag;
				lblFajlHelye.Content = ofd.FileName;
			}

			sliJegy.Value = 3;
            datDatum.Text = DateTime.Now.ToString();
           
		}

        private void btnRogzit_Click(object sender, RoutedEventArgs e)
        {
			//todo Ne lehessen rögzíteni, ha a következők valamelyike nem teljesül!
			// a) - A név legalább két szóból álljon és szavanként minimum 3 karakterből!
			//      Szó = A szöközökkel határolt karaktersorozat.
			// b) - A beírt dátum újabb, mint a mai dátum

			//todo A rögzítés mindig az aktuálisan megnyitott naplófájlba történjen!


			//A CSV szerkezetű fájlba kerülő sor előállítása
			string[] nev = txtNev.Text.Split(" ");
			string csvSor = $"{txtNev.Text};{datDatum.Text};{cboTantargy.Text};{sliJegy.Value};{nev[0]}";
            //Megnyitás hozzáfűzéses írása (APPEND)

            //todo Az újonnan felvitt jegy is jelenjen meg a datagrid-ben!
            string[] tomb = csvSor.Split(";");
			

			if (nev.Length < 2)
            {
                MessageBox.Show("A névnek legalább két szóbol kell állnia");
            }
            else
            {
                if (nev[0].Length < 3 || nev[1].Length < 3 )
                {
                    MessageBox.Show("A neveknek szavanként minimum 3 karakterből kell állnia");
                }
                else
                {

					int eredmeny = DateTime.Compare((DateTime)datDatum.SelectedDate, DateTime.Now);

					if (eredmeny > 0)
					{
						MessageBox.Show("Nem lehet jövőbeli dátum");
					}
                    else
                    {
						StreamWriter sw = new StreamWriter(fajlNev, append: true);
						sw.WriteLine(csvSor);
						sw.Close();

						if (rbKeresztNev.IsChecked == true)
						{
							Osztalyzat ujJegy = new Osztalyzat(tomb[0], tomb[1], tomb[2], int.Parse(tomb[3]), nev[0]);
							jegyek.Add(ujJegy);
							Osztalyzat.ForditottNev(jegyek);
						}
						else
						{
							Osztalyzat ujJegy = new Osztalyzat(tomb[0], tomb[1], tomb[2], int.Parse(tomb[3]), nev[0]);
							jegyek.Add(ujJegy);
						}

						dgJegyek.ItemsSource = jegyek;
						double atlag = jegyek.Average(x => x.Jegy);
						lblJegyekSzama.Content = dgJegyek.Items.Count;
						dgJegyek.Items.Refresh();
						lblAtlag.Content = atlag;
					}


                }
			}

        }

        private void btnBetolt_Click(object sender, RoutedEventArgs e)
        {
            jegyek.Clear();  //A lista előző tartalmát töröljük
            StreamReader sr = new StreamReader(fajlNev); //olvasásra nyitja az állományt
            while (!sr.EndOfStream) //amíg nem ér a fájl végére
            {
                string[] mezok = sr.ReadLine().Split(";"); //A beolvasott sort feltördeli mezőkre
                //A mezők értékeit felhasználva létrehoz egy objektumot
                Osztalyzat ujJegy = new Osztalyzat(mezok[0], mezok[1], mezok[2], int.Parse(mezok[3]), mezok[4]); 
                jegyek.Add(ujJegy); //Az objektumot a lista végére helyezi
            }
            sr.Close(); //állomány lezárása

			//A Datagrid adatforrása a jegyek nevű lista lesz.
			//A lista objektumokat tartalmaz. Az objektumok lesznek a rács sorai.
			//Az objektum nyilvános tulajdonságai kerülnek be az oszlopokba.

			dgJegyek.Items.Refresh();


			dgJegyek.ItemsSource = jegyek;
			lblJegyekSzama.Content = dgJegyek.Items.Count;
			double atlag = jegyek.Average(x => x.Jegy);
			lblAtlag.Content = atlag;
		}

        private void sliJegy_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblJegy.Content = sliJegy.Value; //Több alternatíva van e helyett! Legjobb a Data Binding!
        }

		private void rbKeresztNev_Checked(object sender, RoutedEventArgs e)
		{
			Osztalyzat.ForditottNev(jegyek);
			dgJegyek.Items.Refresh();
		}

		private void rbVezetekNev_Checked(object sender, RoutedEventArgs e)
		{
			Osztalyzat.ForditottNev(jegyek);
			dgJegyek.Items.Refresh();
		}

		//todo Felület bővítése: Az XAML átszerkesztésével biztosítsa, hogy láthatóak legyenek a következők!
		// - A naplófájl neve
		// - A naplóban lévő jegyek száma
		// - Az átlag

		//todo Új elemek frissítése: Figyeljen rá, ha új jegyet rögzít, akkor frissítse a jegyek számát és az átlagot is!

		//todo Helyezzen el alkalmas helyre 2 rádiónyomógombot!
		//Feliratok: [■] Vezetéknév->Keresztnév [O] Keresztnév->Vezetéknév
		//A táblázatban a név azserint szerepeljen, amit a rádiónyomógomb mutat!
		//A feladat megoldásához használja fel a ForditottNev metódust!
		//Módosíthatja az osztályban a Nev property hozzáférhetőségét!
		//Megjegyzés: Felételezzük, hogy csak 2 tagú nevek vannak
	}
}

