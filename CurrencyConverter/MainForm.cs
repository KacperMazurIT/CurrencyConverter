using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace CurrencyConverter
{
	public partial class MainForm : Form
	{
		string Plik; 
		struct S_Waluta
		{
			public string Symbol;
			public string Kraj;
			public decimal Kurs;
		}
		
		List<S_Waluta> Waluta = new List<S_Waluta>();
		
		void Wczytaj()
		{
            S_Waluta Rekord;
            if (File.Exists(Plik) == false) return;

            try
            {
                FileStream Strumien = new FileStream(Plik, FileMode.Open);
                StreamReader Dane = new StreamReader(Strumien);

                while (Dane.EndOfStream == false)
                {
                    Rekord.Symbol = Dane.ReadLine();
                    Rekord.Kraj = Dane.ReadLine();
                    Rekord.Kurs = Convert.ToDecimal(Dane.ReadLine());

                    Waluta.Add(Rekord);
                }

                Dane.Close();

            }catch(IOException)
            {
                MessageBox.Show("Błąd odczytu kursów walut !!!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
		}
		
		void Zapisz()
		{
            FileMode Tryb;
            if(File.Exists (Plik) == false)
            {
                Tryb = FileMode.CreateNew;
            }else
            {
                Tryb = FileMode.Create;
            }
            try
            {
                FileStream Strumien = new FileStream(Plik, Tryb);
                StreamWriter Dane = new StreamWriter(Strumien);

                for (int i = 1; i < Waluta.Count; i++)
                {
                    Dane.WriteLine(Waluta[i].Symbol);
                    Dane.WriteLine(Waluta[i].Kraj);
                    Dane.WriteLine(Convert.ToString(Waluta[i].Kurs));
                }
                Dane.Close();
                Strumien.Close();



            }catch(IOException)
            {
                MessageBox.Show("Błąd zapisu kursów walut !!!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
		
		void Wyliczaj()
		{
            int Indeks1, Indeks2;
            decimal Cena1, Cena2;
            decimal Ile;
            decimal Wynik;
            long Wynik2;

            Indeks1 = comboBox1.SelectedIndex;
            Indeks2 = comboBox2.SelectedIndex;

            if((Indeks1 >= 0) && (Indeks2 >= 0))
            {
                Cena1 = Waluta[Indeks1].Kurs;
                Cena2 = Waluta[Indeks2].Kurs;
                Ile = PobierzLiczbe(textBox1, 2);
                Wynik = (Cena1 * Ile) / Cena2;
                Wynik2 = (long)(Wynik * 100);
                Wynik = (decimal)Wynik2 / 100;
                label6.Text = "Wynik = " + Convert.ToString(Wynik);
            }

        }

        void OdczytDoEdycji()
		{
			int Indeks = listBox1.SelectedIndex;
			
			textBox2.Enabled = false;
			textBox3.Enabled = false;
			textBox4.Enabled = false;
			button2.Enabled = false;
			button3.Enabled = false;
			
			if ((Indeks >= 0) && (Indeks < Waluta.Count))
			{
				if(Indeks > 0)
				{
					textBox2.Enabled = true;
					textBox3.Enabled = true;
					textBox4.Enabled = true;
					button2.Enabled = true;
					button3.Enabled = true;
				}
				textBox2.Text = Waluta[Indeks].Symbol;
				textBox3.Text = Waluta[Indeks].Kraj;
				textBox4.Text = Convert.ToString(Waluta[Indeks].Kurs);
			}
			
		}

        decimal PobierzLiczbe(TextBox Pole, int Miejsca)
        {
            decimal Wynik = 0;
            bool Przecinek = false;
            string Calkowita = "0";
            string Ulamkowa = "";
            decimal Wart1, Wart2;
            int Liczba;
            char Znak;

            if(Pole.Text != "")
            {
                for ( int i = 0; i < Pole.Text.Length; i++)
                {
                    Znak = Pole.Text[i];
                    if ((Znak == ',') || (Znak == '.'))
                    {
                        if (Przecinek == false)
                        {
                            Przecinek = true;
                        }else
                        {
                            break;
                        }
                    }

                    if (Char.IsDigit(Znak) == true)
                    {
                        if (Przecinek == false)
                        {
                            Calkowita += Znak;
                        }else
                        {
                            Ulamkowa += Znak;
                        }
                    }
                }

                Wart1 = Convert.ToDecimal(Calkowita);
                Liczba = Miejsca - Ulamkowa.Length;
                for (int i = 0; i < Liczba; i++)
                {
                    Ulamkowa += "0";
                }
                Ulamkowa = Ulamkowa.Substring(0, Miejsca);

                decimal Dzielnik = 1;
                for (int i = 0; i<Miejsca; i++)
                {
                    Dzielnik  *= 10;
                }
                Wart2 = Convert.ToDecimal(Ulamkowa) / Dzielnik;
                Wynik = Wart1 + Wart2;
            }

            return Wynik;
        }
        bool DoAktualizacji(int Indeks)
        {
            bool Wynik = false;

            if ((Indeks >= 1) && (Indeks < Waluta.Count))
            {
                if (Waluta[Indeks].Symbol != textBox2.Text) Wynik = true;
                if (Waluta[Indeks].Kraj != textBox3.Text) Wynik = true;
                if (Waluta[Indeks].Kurs != PobierzLiczbe(textBox4,4)) Wynik = true;
            }
                return Wynik;
        }


        void Wprowadzanie()
        {
            if (DoAktualizacji (listBox1.SelectedIndex) == true)
            {
                button3.Enabled = true;
            }else
            {
                button3.Enabled = false;
            }
        }


		string Nazwa (int Indeks)
		{
			string Wynik = "";
			
			if ((Indeks >= 0) && (Indeks < Waluta.Count))
			{
				Wynik = Waluta[Indeks].Symbol + " - " + Waluta[Indeks].Kraj;
			}
				return Wynik;
		}
		void Listy()
		{
			string Tekst;
			
			comboBox1.Sorted = false;
			comboBox1.Items.Clear();
			comboBox2.Sorted = false;
			comboBox2.Items.Clear();
			listBox1.Sorted = false;
			listBox1.Items.Clear();
			
			for (int i = 0; i < Waluta.Count; i++)
			{
				Tekst = Nazwa (i);
				comboBox1.Items.Add(Tekst);
				comboBox2.Items.Add(Tekst);
				listBox1.Items.Add(Tekst);
			}
			
			comboBox1.SelectedIndex = 0;
			comboBox2.SelectedIndex = 0;
		}
		
		void Przeliczniki()
		{
			decimal Wynik;
			int Indeks;
			
			Indeks = comboBox1.SelectedIndex;
			if((Indeks >= 0) && (Indeks < Waluta.Count))
			{
				Wynik = Waluta[Indeks].Kurs / Waluta[0].Kurs;
				label2.Text = "1 " + Waluta[Indeks].Symbol + " = " + Wynik + " " + Waluta[0].Symbol;
			}
			
			Indeks = comboBox2.SelectedIndex;
			if((Indeks >= 0) && (Indeks < Waluta.Count))
			{
				Wynik = Waluta[Indeks].Kurs / Waluta[0].Kurs;
				label4.Text = "1 " + Waluta[Indeks].Symbol + " = " + Wynik + " " + Waluta[0].Symbol;
			}
			
		}

        void AktualizacjaEdycji()
        {
            int Indeks = listBox1.SelectedIndex;
            if ((Indeks >= 1) && (Indeks < Waluta.Count))
            {
                S_Waluta Dane;
                Dane = Waluta[Indeks];
                Dane.Symbol = textBox2.Text;
                Dane.Kraj = textBox3.Text;
                Dane.Kurs = PobierzLiczbe(textBox4, 4);
                Waluta[Indeks] = Dane;
                listBox1.Items[Indeks] = Nazwa(Indeks);
            }
            Wprowadzanie();

        }
        public MainForm()
		{
			
			
			InitializeComponent();
			Plik = Environment.CurrentDirectory + "\\Waluty.txt";
			Waluta.Clear();
			S_Waluta Dane;

			Dane.Symbol = "PLN";
			Dane.Kraj = "Polska";
			Dane.Kurs = 1M;
			Waluta.Add(Dane);
			
            /*
			Dane.Symbol = "USD";
			Dane.Kraj = "USA";
			Dane.Kurs = 2.7661M;
			Waluta.Add(Dane);
			
			Dane.Symbol = "EUR";
			Dane.Kraj = "EUGiW";
			Dane.Kurs = 4.2M;
			Waluta.Add(Dane);
			*/

			Wczytaj();
			Listy();
			Przeliczniki();
			OdczytDoEdycji();
		}
		
		
		
		
		void TabPage2Click(object sender, EventArgs e)
		{
	
		}
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			Przeliczniki();
			Wyliczaj();
		}
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			Przeliczniki();
			Wyliczaj();
		}
		void ListBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			OdczytDoEdycji();
            Wprowadzanie();
		}

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Wprowadzanie();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Wprowadzanie();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Wprowadzanie();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AktualizacjaEdycji();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex==0) Listy();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            S_Waluta Dane;
            Dane.Symbol = "Nowy";
            Dane.Kraj = "Nowy";
            Dane.Kurs = 1M;
            Waluta.Add(Dane);
            Listy();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int Indeks = listBox1.SelectedIndex;

            if ((Indeks >= 1) && (Indeks < Waluta.Count))
            {
                Waluta.RemoveAt(Indeks);
                Listy();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Wyliczaj();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Zapisz();
        }
    }
}
