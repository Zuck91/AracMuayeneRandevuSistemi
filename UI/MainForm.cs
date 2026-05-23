using AracMuayeneRandevuSistemi.DAL;
using System.Data;

namespace AracMuayeneRandevuSistemi.UI
{
    public class MainForm : Form
    {
        private readonly DatabaseHelper db = new DatabaseHelper();
        private readonly TabControl tabControl = new TabControl();

        public MainForm()
        {
            Text = "Araç Muayene İstasyonu Randevu Sistemi";
            Width = 1150;
            Height = 720;
            StartPosition = FormStartPosition.CenterScreen;

            tabControl.Dock = DockStyle.Fill;
            Controls.Add(tabControl);

            ModulleriOlustur();
        }

        private void ModulleriOlustur()
        {
            ModülEkle(new ModuleDefinition
            {
                Baslik = "Araç Sahipleri",
                ListeleProc = "sp_AracSahibiListele",
                EkleProc = "sp_AracSahibiEkle",
                GuncelleProc = "sp_AracSahibiGuncelle",
                SilProc = "sp_AracSahibiSil",
                AnahtarKolon = "sahip_id",
                Alanlar = new()
                {
                    new FieldDefinition("p_sahip_id", "Sahip ID", true, false, true, true),
                    new FieldDefinition("p_ad", "Ad", false, true, true, false),
                    new FieldDefinition("p_soyad", "Soyad", false, true, true, false),
                    new FieldDefinition("p_tc_no", "TC No", false, true, false, false),
                    new FieldDefinition("p_telefon", "Telefon", false, true, true, false),
                    new FieldDefinition("p_eposta", "E-posta", false, true, true, false),
                    new FieldDefinition("p_adres", "Adres", false, true, true, false)
                }
            });

            ModülEkle(new ModuleDefinition
            {
                Baslik = "Araçlar",
                ListeleProc = "sp_AracListele",
                EkleProc = "sp_AracEkle",
                GuncelleProc = "sp_AracGuncelle",
                SilProc = "sp_AracSil",
                AnahtarKolon = "arac_id",
                Alanlar = new()
                {
                    new FieldDefinition("p_arac_id", "Araç ID", true, false, true, true),
                    new FieldDefinition("p_sahip_id", "Sahip ID", false, true, false, false),
                    new FieldDefinition("p_plaka", "Plaka", false, true, true, false),
                    new FieldDefinition("p_marka", "Marka", false, true, true, false),
                    new FieldDefinition("p_model", "Model", false, true, true, false),
                    new FieldDefinition("p_yil", "Yıl", false, true, true, false),
                    new FieldDefinition("p_yakit_turu", "Yakıt Türü", false, true, true, false),
                    new FieldDefinition("p_arac_tipi", "Araç Tipi", false, true, true, false)
                }
            });

            ModülEkle(new ModuleDefinition
            {
                Baslik = "İstasyonlar",
                ListeleProc = "sp_IstasyonListele",
                EkleProc = "sp_IstasyonEkle",
                GuncelleProc = "sp_IstasyonGuncelle",
                SilProc = "sp_IstasyonSil",
                AnahtarKolon = "istasyon_id",
                Alanlar = new()
                {
                    new FieldDefinition("p_istasyon_id", "İstasyon ID", true, false, true, true),
                    new FieldDefinition("p_istasyon_adi", "İstasyon Adı", false, true, true, false),
                    new FieldDefinition("p_il", "İl", false, true, true, false),
                    new FieldDefinition("p_ilce", "İlçe", false, true, true, false),
                    new FieldDefinition("p_adres", "Adres", false, true, true, false),
                    new FieldDefinition("p_telefon", "Telefon", false, true, true, false)
                }
            });

            ModülEkle(new ModuleDefinition
            {
                Baslik = "Personeller",
                ListeleProc = "sp_PersonelListele",
                EkleProc = "sp_PersonelEkle",
                GuncelleProc = "sp_PersonelGuncelle",
                SilProc = "sp_PersonelSil",
                AnahtarKolon = "personel_id",
                Alanlar = new()
                {
                    new FieldDefinition("p_personel_id", "Personel ID", true, false, true, true),
                    new FieldDefinition("p_istasyon_id", "İstasyon ID", false, true, true, false),
                    new FieldDefinition("p_ad", "Ad", false, true, true, false),
                    new FieldDefinition("p_soyad", "Soyad", false, true, true, false),
                    new FieldDefinition("p_gorev", "Görev", false, true, true, false),
                    new FieldDefinition("p_telefon", "Telefon", false, true, true, false)
                }
            });

            ModülEkle(new ModuleDefinition
            {
                Baslik = "Muayene Türleri",
                ListeleProc = "sp_MuayeneTuruListele",
                EkleProc = "sp_MuayeneTuruEkle",
                GuncelleProc = "sp_MuayeneTuruGuncelle",
                SilProc = "sp_MuayeneTuruSil",
                AnahtarKolon = "muayene_tur_id",
                Alanlar = new()
                {
                    new FieldDefinition("p_muayene_tur_id", "Muayene Tür ID", true, false, true, true),
                    new FieldDefinition("p_tur_adi", "Tür Adı", false, true, true, false),
                    new FieldDefinition("p_temel_ucret", "Temel Ücret", false, true, true, false),
                    new FieldDefinition("p_aciklama", "Açıklama", false, true, true, false)
                }
            });

            ModülEkle(new ModuleDefinition
            {
                Baslik = "Randevular",
                ListeleProc = "sp_RandevuListele",
                EkleProc = "sp_RandevuEkle",
                GuncelleProc = "sp_RandevuGuncelle",
                SilProc = "sp_RandevuSil",
                AnahtarKolon = "randevu_id",
                Alanlar = new()
                {
                    new FieldDefinition("p_randevu_id", "Randevu ID", true, false, true, true),
                    new FieldDefinition("p_arac_id", "Araç ID", false, true, true, false),
                    new FieldDefinition("p_istasyon_id", "İstasyon ID", false, true, true, false),
                    new FieldDefinition("p_muayene_tur_id", "Muayene Tür ID", false, true, true, false),
                    new FieldDefinition("p_randevu_tarih", "Randevu Tarih (2026-05-25)", false, true, true, false),
                    new FieldDefinition("p_randevu_saat", "Randevu Saat (10:00:00)", false, true, true, false),
                    new FieldDefinition("p_durum", "Durum", false, false, true, false),
                    new FieldDefinition("p_aciklama", "Açıklama", false, true, true, false)
                }
            });

            ModülEkle(new ModuleDefinition
            {
                Baslik = "Muayene Kayıtları",
                ListeleProc = "sp_MuayeneKaydiListele",
                EkleProc = "sp_MuayeneKaydiEkle",
                GuncelleProc = "sp_MuayeneKaydiGuncelle",
                SilProc = "sp_MuayeneKaydiSil",
                AnahtarKolon = "muayene_id",
                Alanlar = new()
                {
                    new FieldDefinition("p_muayene_id", "Muayene ID", true, false, true, true),
                    new FieldDefinition("p_randevu_id", "Randevu ID", false, true, false, false),
                    new FieldDefinition("p_personel_id", "Personel ID", false, true, true, false),
                    new FieldDefinition("p_sonuc", "Sonuç", false, true, true, false),
                    new FieldDefinition("p_kusur_aciklama", "Kusur Açıklama", false, true, true, false)
                }
            });

            ModülEkle(new ModuleDefinition
            {
                Baslik = "Ödemeler",
                ListeleProc = "sp_OdemeListele",
                EkleProc = "sp_OdemeEkle",
                GuncelleProc = "sp_OdemeGuncelle",
                SilProc = "sp_OdemeSil",
                AnahtarKolon = "odeme_id",
                Alanlar = new()
                {
                    new FieldDefinition("p_odeme_id", "Ödeme ID", true, false, true, true),
                    new FieldDefinition("p_randevu_id", "Randevu ID", false, true, false, false),
                    new FieldDefinition("p_tutar", "Tutar", false, true, true, false),
                    new FieldDefinition("p_odeme_turu", "Ödeme Türü", false, true, true, false),
                    new FieldDefinition("p_durum", "Durum", false, false, true, false)
                }
            });
        }

        private void ModülEkle(ModuleDefinition modül)
        {
            TabPage tab = new TabPage(modül.Baslik);

            Panel solPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 320,
                Padding = new Padding(10)
            };

            DataGridView grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            Dictionary<string, TextBox> kutular = new Dictionary<string, TextBox>();
            int y = 10;

            Label baslik = new Label
            {
                Text = modül.Baslik + " İşlemleri",
                Left = 10,
                Top = y,
                Width = 280,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            solPanel.Controls.Add(baslik);
            y += 35;

            foreach (var alan in modül.Alanlar)
            {
                Label lbl = new Label
                {
                    Text = alan.Etiket,
                    Left = 10,
                    Top = y,
                    Width = 280
                };
                solPanel.Controls.Add(lbl);
                y += 22;

                TextBox txt = new TextBox
                {
                    Name = alan.Parametre,
                    Left = 10,
                    Top = y,
                    Width = 280,
                    Enabled = !alan.SadeceSecimdenGelsin
                };
                solPanel.Controls.Add(txt);
                kutular.Add(alan.Parametre, txt);
                y += 34;
            }

            Button btnEkle = ButonOlustur("Ekle", 10, y);
            Button btnGuncelle = ButonOlustur("Güncelle", 110, y);
            Button btnSil = ButonOlustur("Sil", 210, y);
            y += 42;
            Button btnListele = ButonOlustur("Listele", 10, y, 280);

            solPanel.Controls.Add(btnEkle);
            solPanel.Controls.Add(btnGuncelle);
            solPanel.Controls.Add(btnSil);
            solPanel.Controls.Add(btnListele);

            tab.Controls.Add(grid);
            tab.Controls.Add(solPanel);
            tabControl.TabPages.Add(tab);

            btnListele.Click += (s, e) => Listele(modül, grid);

            btnEkle.Click += (s, e) =>
            {
                try
                {
                    Dictionary<string, object> parametreler = ParametreleriAl(modül, kutular, islem: "ekle");
                    db.Calistir(modül.EkleProc, parametreler);
                    MessageBox.Show("Kayıt eklendi.");
                    Listele(modül, grid);
                    KutulariTemizle(kutular);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ekleme hatası: " + ex.Message);
                }
            };

            btnGuncelle.Click += (s, e) =>
            {
                try
                {
                    Dictionary<string, object> parametreler = ParametreleriAl(modül, kutular, islem: "guncelle");
                    db.Calistir(modül.GuncelleProc, parametreler);
                    MessageBox.Show("Kayıt güncellendi.");
                    Listele(modül, grid);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Güncelleme hatası: " + ex.Message);
                }
            };

            btnSil.Click += (s, e) =>
            {
                try
                {
                    var anahtarAlan = modül.Alanlar.First(a => a.SilmeParametresi);
                    Dictionary<string, object> parametreler = new Dictionary<string, object>
                    {
                        { anahtarAlan.Parametre, kutular[anahtarAlan.Parametre].Text }
                    };

                    db.Calistir(modül.SilProc, parametreler);
                    MessageBox.Show("Kayıt silindi.");
                    Listele(modül, grid);
                    KutulariTemizle(kutular);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme hatası: " + ex.Message);
                }
            };

            grid.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                DataGridViewRow satir = grid.Rows[e.RowIndex];

                foreach (var alan in modül.Alanlar)
                {
                    string kolonAdi = KolonAdiBul(alan.Parametre);

                    if (satir.DataGridView.Columns.Contains(kolonAdi))
                    {
                        kutular[alan.Parametre].Text = satir.Cells[kolonAdi].Value?.ToString() ?? "";
                    }
                }
            };

            Listele(modül, grid);
        }

        private Button ButonOlustur(string yazi, int x, int y, int genislik = 90)
        {
            return new Button
            {
                Text = yazi,
                Left = x,
                Top = y,
                Width = genislik,
                Height = 32
            };
        }

        private void Listele(ModuleDefinition modül, DataGridView grid)
        {
            try
            {
                grid.DataSource = db.Listele(modül.ListeleProc);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Listeleme hatası: " + ex.Message);
            }
        }

        private Dictionary<string, object> ParametreleriAl(ModuleDefinition modül, Dictionary<string, TextBox> kutular, string islem)
        {
            Dictionary<string, object> parametreler = new Dictionary<string, object>();

            foreach (var alan in modül.Alanlar)
            {
                bool kullanilacak = islem == "ekle" ? alan.EklemedeVar : alan.GuncellemedeVar;

                if (kullanilacak)
                {
                    parametreler.Add(alan.Parametre, kutular[alan.Parametre].Text.Trim());
                }
            }

            return parametreler;
        }

        private void KutulariTemizle(Dictionary<string, TextBox> kutular)
        {
            foreach (var item in kutular)
            {
                item.Value.Clear();
            }
        }

        private string KolonAdiBul(string parametre)
        {
            string ad = parametre.Replace("p_", "");

            if (ad == "tc_no") return "tc_no";
            if (ad == "eposta") return "eposta";
            if (ad == "arac_sahibi") return "arac_sahibi";
            if (ad == "personel_ad_soyad") return "personel_ad_soyad";

            return ad;
        }
    }

    public class ModuleDefinition
    {
        public string Baslik { get; set; }
        public string ListeleProc { get; set; }
        public string EkleProc { get; set; }
        public string GuncelleProc { get; set; }
        public string SilProc { get; set; }
        public string AnahtarKolon { get; set; }
        public List<FieldDefinition> Alanlar { get; set; } = new();
    }

    public class FieldDefinition
    {
        public string Parametre { get; set; }
        public string Etiket { get; set; }
        public bool SadeceSecimdenGelsin { get; set; }
        public bool EklemedeVar { get; set; }
        public bool GuncellemedeVar { get; set; }
        public bool SilmeParametresi { get; set; }

        public FieldDefinition(string parametre, string etiket, bool sadeceSecimdenGelsin, bool eklemedeVar, bool guncellemedeVar, bool silmeParametresi)
        {
            Parametre = parametre;
            Etiket = etiket;
            SadeceSecimdenGelsin = sadeceSecimdenGelsin;
            EklemedeVar = eklemedeVar;
            GuncellemedeVar = guncellemedeVar;
            SilmeParametresi = silmeParametresi;
        }
    }
}
