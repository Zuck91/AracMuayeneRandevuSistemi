using AracMuayeneRandevuSistemi.DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AracMuayeneRandevuSistemi.UI
{
    public class AnaForm : Form
    {
        Veritabani vt = new Veritabani();
        TabControl sekmeler = new TabControl();

        public AnaForm()
        {
            Text = "Araç Muayene İstasyonu Randevu Sistemi";
            Width = 1100;
            Height = 700;
            StartPosition = FormStartPosition.CenterScreen;

            sekmeler.Dock = DockStyle.Fill;
            Controls.Add(sekmeler);

            EkranlariOlustur();
        }

        void EkranlariOlustur()
        {
            EkranEkle("Araç Sahipleri", "sp_AracSahibiListele", "sp_AracSahibiEkle", "sp_AracSahibiGuncelle", "sp_AracSahibiSil",
                new string[] { "p_sahip_id", "p_ad", "p_soyad", "p_tc_no", "p_telefon", "p_eposta", "p_adres" },
                new string[] { "Sahip ID", "Ad", "Soyad", "TC No", "Telefon", "E-posta", "Adres" },
                0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 0, 1, 2, 4, 5, 6 });

            EkranEkle("Araçlar", "sp_AracListele", "sp_AracEkle", "sp_AracGuncelle", "sp_AracSil",
                new string[] { "p_arac_id", "p_sahip_id", "p_plaka", "p_marka", "p_model", "p_yil", "p_yakit_turu", "p_arac_tipi" },
                new string[] { "Araç ID", "Sahip ID", "Plaka", "Marka", "Model", "Yıl", "Yakıt Türü", "Araç Tipi" },
                0, new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { 0, 2, 3, 4, 5, 6, 7 });

            EkranEkle("İstasyonlar", "sp_IstasyonListele", "sp_IstasyonEkle", "sp_IstasyonGuncelle", "sp_IstasyonSil",
                new string[] { "p_istasyon_id", "p_istasyon_adi", "p_il", "p_ilce", "p_adres", "p_telefon" },
                new string[] { "İstasyon ID", "İstasyon Adı", "İl", "İlçe", "Adres", "Telefon" },
                0, new int[] { 1, 2, 3, 4, 5 }, new int[] { 0, 1, 2, 3, 4, 5 });

            EkranEkle("Personeller", "sp_PersonelListele", "sp_PersonelEkle", "sp_PersonelGuncelle", "sp_PersonelSil",
                new string[] { "p_personel_id", "p_istasyon_id", "p_ad", "p_soyad", "p_gorev", "p_telefon" },
                new string[] { "Personel ID", "İstasyon ID", "Ad", "Soyad", "Görev", "Telefon" },
                0, new int[] { 1, 2, 3, 4, 5 }, new int[] { 0, 1, 2, 3, 4, 5 });

            EkranEkle("Muayene Türleri", "sp_MuayeneTuruListele", "sp_MuayeneTuruEkle", "sp_MuayeneTuruGuncelle", "sp_MuayeneTuruSil",
                new string[] { "p_muayene_tur_id", "p_tur_adi", "p_temel_ucret", "p_aciklama" },
                new string[] { "Muayene Tür ID", "Tür Adı", "Temel Ücret", "Açıklama" },
                0, new int[] { 1, 2, 3 }, new int[] { 0, 1, 2, 3 });

            EkranEkle("Randevular", "sp_RandevuListele", "sp_RandevuEkle", "sp_RandevuGuncelle", "sp_RandevuSil",
                new string[] { "p_randevu_id", "p_arac_id", "p_istasyon_id", "p_muayene_tur_id", "p_randevu_tarih", "p_randevu_saat", "p_durum", "p_aciklama" },
                new string[] { "Randevu ID", "Araç ID", "İstasyon ID", "Muayene Tür ID", "Tarih", "Saat", "Durum", "Açıklama" },
                0, new int[] { 1, 2, 3, 4, 5, 7 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });

            EkranEkle("Muayene Kayıtları", "sp_MuayeneKaydiListele", "sp_MuayeneKaydiEkle", "sp_MuayeneKaydiGuncelle", "sp_MuayeneKaydiSil",
                new string[] { "p_muayene_id", "p_randevu_id", "p_personel_id", "p_sonuc", "p_kusur_aciklama" },
                new string[] { "Muayene ID", "Randevu ID", "Personel ID", "Sonuç", "Kusur Açıklaması" },
                0, new int[] { 1, 2, 3, 4 }, new int[] { 0, 2, 3, 4 });

            EkranEkle("Ödemeler", "sp_OdemeListele", "sp_OdemeEkle", "sp_OdemeGuncelle", "sp_OdemeSil",
                new string[] { "p_odeme_id", "p_randevu_id", "p_tutar", "p_odeme_turu", "p_durum" },
                new string[] { "Ödeme ID", "Randevu ID", "Tutar", "Ödeme Türü", "Durum" },
                0, new int[] { 1, 2, 3 }, new int[] { 0, 2, 3, 4 });
        }

        void EkranEkle(string baslik, string listeleProc, string ekleProc, string guncelleProc, string silProc,
            string[] parametreler, string[] etiketler, int idIndex, int[] ekleIndexleri, int[] guncelleIndexleri)
        {
            TabPage sayfa = new TabPage(baslik);
            Panel solPanel = new Panel();
            DataGridView tablo = new DataGridView();
            Dictionary<string, TextBox> kutular = new Dictionary<string, TextBox>();

            solPanel.Dock = DockStyle.Left;
            solPanel.Width = 300;
            solPanel.Padding = new Padding(10);

            tablo.Dock = DockStyle.Fill;
            tablo.ReadOnly = true;
            tablo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tablo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            int y = 10;

            Label lblBaslik = new Label();
            lblBaslik.Text = baslik + " İşlemleri";
            lblBaslik.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblBaslik.Left = 10;
            lblBaslik.Top = y;
            lblBaslik.Width = 260;
            solPanel.Controls.Add(lblBaslik);

            y += 35;

            for (int i = 0; i < parametreler.Length; i++)
            {
                Label lbl = new Label();
                lbl.Text = etiketler[i];
                lbl.Left = 10;
                lbl.Top = y;
                lbl.Width = 260;
                solPanel.Controls.Add(lbl);

                y += 22;

                TextBox txt = new TextBox();
                txt.Left = 10;
                txt.Top = y;
                txt.Width = 260;

                if (i == idIndex)
                {
                    txt.Enabled = false;
                }

                solPanel.Controls.Add(txt);
                kutular.Add(parametreler[i], txt);

                y += 32;
            }

            Button btnEkle = ButonOlustur("Ekle", 10, y);
            Button btnGuncelle = ButonOlustur("Güncelle", 105, y);
            Button btnSil = ButonOlustur("Sil", 200, y);

            y += 38;

            Button btnListele = ButonOlustur("Listele", 10, y, 260);

            solPanel.Controls.Add(btnEkle);
            solPanel.Controls.Add(btnGuncelle);
            solPanel.Controls.Add(btnSil);
            solPanel.Controls.Add(btnListele);

            sayfa.Controls.Add(tablo);
            sayfa.Controls.Add(solPanel);
            sekmeler.TabPages.Add(sayfa);

            btnListele.Click += (s, e) => Listele(tablo, listeleProc);

            btnEkle.Click += (s, e) =>
            {
                try
                {
                    vt.Calistir(ekleProc, ParametreHazirla(parametreler, kutular, ekleIndexleri));
                    MessageBox.Show("Kayıt eklendi.");
                    Listele(tablo, listeleProc);
                }
                catch (Exception hata)
                {
                    MessageBox.Show(hata.Message);
                }
            };

            btnGuncelle.Click += (s, e) =>
            {
                try
                {
                    vt.Calistir(guncelleProc, ParametreHazirla(parametreler, kutular, guncelleIndexleri));
                    MessageBox.Show("Kayıt güncellendi.");
                    Listele(tablo, listeleProc);
                }
                catch (Exception hata)
                {
                    MessageBox.Show(hata.Message);
                }
            };

            btnSil.Click += (s, e) =>
            {
                try
                {
                    Dictionary<string, object> p = new Dictionary<string, object>();
                    p.Add(parametreler[idIndex], kutular[parametreler[idIndex]].Text);
                    vt.Calistir(silProc, p);
                    MessageBox.Show("Kayıt silindi.");
                    Listele(tablo, listeleProc);
                }
                catch (Exception hata)
                {
                    MessageBox.Show(hata.Message);
                }
            };

            tablo.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                for (int i = 0; i < parametreler.Length; i++)
                {
                    string kolon = parametreler[i].Replace("p_", "");

                    if (tablo.Columns.Contains(kolon))
                    {
                        kutular[parametreler[i]].Text = tablo.Rows[e.RowIndex].Cells[kolon].Value.ToString();
                    }
                }
            };

            Listele(tablo, listeleProc);
        }

        Button ButonOlustur(string yazi, int x, int y, int genislik = 85)
        {
            Button btn = new Button();
            btn.Text = yazi;
            btn.Left = x;
            btn.Top = y;
            btn.Width = genislik;
            btn.Height = 30;
            return btn;
        }

        void Listele(DataGridView tablo, string procedureAdi)
        {
            try
            {
                tablo.DataSource = vt.Listele(procedureAdi);
            }
            catch (Exception hata)
            {
                MessageBox.Show("Listeleme hatası: " + hata.Message);
            }
        }

        Dictionary<string, object> ParametreHazirla(string[] parametreler, Dictionary<string, TextBox> kutular, int[] kullanilacaklar)
        {
            Dictionary<string, object> degerler = new Dictionary<string, object>();

            foreach (int index in kullanilacaklar)
            {
                degerler.Add(parametreler[index], kutular[parametreler[index]].Text.Trim());
            }

            return degerler;
        }
    }
}
